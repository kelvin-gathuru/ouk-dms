using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.MediatR.Handlers;

public class AddWorkflowInstanceCommandHandler(IWorkflowRepository _workflowRepository,
    IDocumentRepository _documentRepository,
    IWorkflowStepRepository _workflowStepRepository,
    IWorkflowInstanceRepository _workflowInstanceRepository,
    IWorkflowStepInstanceRepository _workflowStepInstanceRepository,
    IWorkflowTransitionRepository _workflowTransitionRepository,
    UserInfoToken _userInfoToken,
    IUnitOfWork<DocumentContext> _uow,
    IMapper _mapper,
    IWorkflowTransitionInstanceRepository _workflowTransitionInstanceRepository,
    IUserRoleRepository _userRoleRepository,
    IHubContext<UserHub, IHubClient> hubContext,
    IConnectionMappingRepository connectionMappingRepository,
    IUserNotificationRepository _userNotificationRepository) : IRequestHandler<AddWorkflowInstanceCommand, ServiceResponse<WorkflowInstanceDto>>

{

    public async Task<ServiceResponse<WorkflowInstanceDto>> Handle(AddWorkflowInstanceCommand request, CancellationToken cancellationToken)
    {
        var workflowEntityExist = await _workflowRepository.FindBy(c => c.Id == request.WorkflowId && c.IsWorkflowSetup).FirstOrDefaultAsync();
        if (workflowEntityExist == null)
        {
            return ServiceResponse<WorkflowInstanceDto>.Return404();
        }
        var DocumentEntityExist = await _documentRepository.FindBy(d => d.Id == request.DocumentId).FirstOrDefaultAsync();
        if (DocumentEntityExist == null)
        {
            return ServiceResponse<WorkflowInstanceDto>.Return404();
        }

        var entity = _mapper.Map<DocumentManagement.Data.WorkflowInstance>(request);
        var workflowInstanceId = Guid.NewGuid();
        entity.Id = workflowInstanceId;
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.Status = WorkflowInstanceStatus.Initiated;
        entity.InitiatedId = _userInfoToken.Id;
        _workflowInstanceRepository.Add(entity);

        var workflowSteps = _workflowStepRepository.All.Where(c => c.WorkflowId == workflowEntityExist.Id).ToList();
        var transition = _workflowTransitionRepository.All.Where(t =>
        t.IsFirstTransaction == true && t.WorkflowId == request.WorkflowId
        ).FirstOrDefault();
        if (transition == null)
        {
            return ServiceResponse<WorkflowInstanceDto>.Return404();
        }
        var transactions = _workflowTransitionRepository.All
            .Include(c => c.WorkflowTransitionRoles)
            .Include(c => c.WorkflowTransitionUsers)
            .Where(t => t.FromStepId == transition.FromStepId)
            .ToList();

        List<NotificationWorkflowDto> lstNotificationWorkflow = new List<NotificationWorkflowDto>();
        for (var i = 0; i < transactions.Count; i++)
        {
            var toStep = workflowSteps.Where(c => c.Id == transactions[i].ToStepId).FirstOrDefault();
            var workflowTransitionRoles = transactions[i].WorkflowTransitionRoles.ToList();
            var workflowTransitionUsers = transactions[i].WorkflowTransitionUsers.ToList();
            if (workflowTransitionRoles != null && workflowTransitionRoles.Count() > 0)
            {
                var userIds = await _userRoleRepository.GetUsersByRoles(workflowTransitionRoles.Select(c => c.RoleId).ToList());
                foreach (var userId in userIds)
                {
                    lstNotificationWorkflow.Add(new NotificationWorkflowDto()
                    {
                        UserId = userId,
                        WorkflowInstanceId = workflowInstanceId,
                        Message = $"You have been assigned the task of \"{transactions[0].Name}\" the workflow \"{workflowEntityExist.Name}\" for the document named \"{DocumentEntityExist.Name}.\"",
                        DocumentId = DocumentEntityExist.Id
                    });
                }
            }
            if (workflowTransitionUsers.Count > 0)
            {
                foreach (var workflowUser in workflowTransitionUsers)
                {
                    if (lstNotificationWorkflow.Any(c => c.UserId == workflowUser.UserId))
                    {
                        continue;
                    }

                    lstNotificationWorkflow.Add(new NotificationWorkflowDto()
                    {
                        UserId = workflowUser.UserId,
                        WorkflowInstanceId = workflowInstanceId,
                        Message = $"You have been assigned the task of \"{transactions[0].Name}\" the workflow \"{workflowEntityExist.Name}\" for the document named \"{DocumentEntityExist.Name}.\"",
                        DocumentId = DocumentEntityExist.Id
                    });
                }
            }

            var workflowTransitionInstance = new Data.WorkflowTransitionInstance()
            {
                WorkflowTransitionId = transactions[i].Id,
                Status = WorkflowTransitionInstanceStatus.Initiated,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                WorkflowInstanceId = entity.Id,
            };
            _workflowTransitionInstanceRepository.Add(workflowTransitionInstance);
        }
        var workflowStepInstance = new WorkflowStepInstance()
        {
            Status = WorkflowStepInstanceStatus.InProgress,
            WorkflowInstanceId = entity.Id,
            UserId = _userInfoToken.Id,
            StepId = transition.FromStepId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
        _workflowStepInstanceRepository.Add(workflowStepInstance);
        if (lstNotificationWorkflow.Count > 0)
        {
            _userNotificationRepository.createWorkflowInstanceNotifications(lstNotificationWorkflow);

        }
        if (await _uow.SaveAsync() <= 0)
        {
            return ServiceResponse<WorkflowInstanceDto>.Return500();
        }
        try
        {
            if (lstNotificationWorkflow.Count > 0)
            {
                var users = lstNotificationWorkflow.Select(c => c.UserId).ToList();
                await _userNotificationRepository.SendNotification(users);
                var user = connectionMappingRepository.GetUserInfoById(_userInfoToken.Id);
                if (user != null)
                {
                    await hubContext.Clients.AllExcept(new List<string> { user.ConnectionId }).RefreshDocuments(DocumentEntityExist.CategoryId);
                    await hubContext.Clients.AllExcept(new List<string> { user.ConnectionId }).RefreshWorkflows();
                }
            }
        }
        catch (Exception)
        {

        }
        var entityDto = _mapper.Map<WorkflowInstanceDto>(entity);
        return ServiceResponse<WorkflowInstanceDto>.ReturnResultWith201(entityDto);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.MediatR.Handlers;

public class CancelWorkflowInstanceCommandHandler : IRequestHandler<CancelWorkflowInstanceQuery, ServiceResponse<bool>>
{
    private readonly IWorkflowInstanceRepository _workflowInstanceRepository;
    private readonly UserInfoToken _userInfoToken;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork<DocumentContext> _uow;
    private readonly IWorkflowTransitionRepository _workflowTransitionRepository;
    private readonly IWorkflowStepRepository _workflowStepRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IDocumentRepository _documentRepository;
    private readonly IUserNotificationRepository _userNotificationRepository;
    private readonly IWorkflowInstanceEmailSenderRepository _workflowInstanceEmailSenderRepository;
    private readonly IHubContext<UserHub, IHubClient> _hubContext;
    private readonly IConnectionMappingRepository _connectionMappingRepository;

    public CancelWorkflowInstanceCommandHandler(IWorkflowInstanceRepository workflowInstanceRepository, IMapper mapper, IUnitOfWork<DocumentContext> uow,
        UserInfoToken userInfoToken, IWorkflowStepRepository workflowStepRepository, IUserRoleRepository userRoleRepository,
        IDocumentRepository documentRepository, IWorkflowTransitionRepository workflowTransitionRepository, IUserNotificationRepository userNotificationRepository,
        IWorkflowInstanceEmailSenderRepository workflowInstanceEmailSenderRepository,
        IHubContext<UserHub, IHubClient> hubContext,
         IConnectionMappingRepository connectionMappingRepository)
    {
        _workflowInstanceRepository = workflowInstanceRepository;
        _mapper = mapper;
        _uow = uow;
        _userInfoToken = userInfoToken;
        _workflowStepRepository = workflowStepRepository;
        _userRoleRepository = userRoleRepository;
        _documentRepository = documentRepository;
        _userNotificationRepository = userNotificationRepository;
        _workflowTransitionRepository = workflowTransitionRepository;
        _workflowInstanceEmailSenderRepository = workflowInstanceEmailSenderRepository;
        _hubContext = hubContext;
        _connectionMappingRepository = connectionMappingRepository;

    }

    public async Task<ServiceResponse<bool>> Handle(CancelWorkflowInstanceQuery request, CancellationToken cancellationToken)
    {
        var entity = await _workflowInstanceRepository.All
            .Include(c => c.Workflow)
            .Include(c => c.WorkflowStepInstances)
            .FirstOrDefaultAsync(w => w.Id == request.Id);
        if (entity == null)
        {
            return ServiceResponse<bool>.Return409("Not found");
        }
        entity.Status = Data.WorkflowInstanceStatus.Cancelled;
        var document = await _documentRepository.FindBy(c => c.Id == entity.DocumentId).FirstOrDefaultAsync();
        var currentWorkflowStep = entity.WorkflowStepInstances.FirstOrDefault(c => c.Status == WorkflowStepInstanceStatus.InProgress);
        var transitions = _workflowTransitionRepository.All
            .Include(c => c.WorkflowTransitionRoles)
            .Include(c => c.WorkflowTransitionUsers)
            .Where(t => t.FromStepId == currentWorkflowStep.StepId)
            .ToList();
        if (currentWorkflowStep != null)
        {
            var workflowInstanceEmailSenders = _workflowInstanceEmailSenderRepository.All.Where(c => c.WorkflowStepInstanceId == currentWorkflowStep.Id).ToList();
            if (workflowInstanceEmailSenders.Count > 0)
            {
                _workflowInstanceEmailSenderRepository.RemoveRange(workflowInstanceEmailSenders);
            }
        }
        List<NotificationWorkflowDto> lstNotificationWorkflow = new List<NotificationWorkflowDto>();
        foreach (var transition in transitions)
        {
            var toStep = _workflowStepRepository.All.Where(c => c.Id == transition.ToStepId && c.WorkflowId == entity.WorkflowId).FirstOrDefault();
            if (transition.WorkflowTransitionRoles.Count > 0)
            {
                var userIds = await _userRoleRepository.GetUsersByRoles(transition.WorkflowTransitionRoles.Select(c => c.RoleId).ToList());
                foreach (var userId in userIds)
                {
                    lstNotificationWorkflow.Add(new NotificationWorkflowDto()
                    {
                        UserId = userId,
                        WorkflowInstanceId = entity.Id,
                        Message = $"Task of \"{transition.Name}\" the workflow \"{entity.Workflow.Name}\" have been cancelled for the document named \"{document.Name}.\"",
                        DocumentId = document.Id
                    });
                }
            }
            if (transition.WorkflowTransitionUsers.Count > 0)
            {
                foreach (var workflowUser in transition.WorkflowTransitionUsers)
                {
                    if (lstNotificationWorkflow.Any(c => c.UserId == workflowUser.UserId))
                    {
                        continue;
                    }

                    lstNotificationWorkflow.Add(new NotificationWorkflowDto()
                    {
                        UserId = workflowUser.UserId,
                        WorkflowInstanceId = entity.Id,
                        Message = $"Task of \"{transition.Name}\" the workflow \"{entity.Workflow.Name}\" have been cancelled for the document named \"{document.Name}.\"",
                        DocumentId = document.Id
                    });
                }
            }

        }

        foreach (var item in entity.WorkflowStepInstances)
        {
            if (item.Status == WorkflowStepInstanceStatus.InProgress)
            {
                item.Status = WorkflowStepInstanceStatus.Cancelled;
            }
        };
        if (lstNotificationWorkflow.Count > 0)
        {
            _userNotificationRepository.createWorkflowInstanceNotifications(lstNotificationWorkflow);
        }

        _workflowInstanceRepository.Update(entity);

        if (await _uow.SaveAsync() <= 0)
        {
            return ServiceResponse<bool>.Return500();
        }
        try
        {
            if (lstNotificationWorkflow.Count > 0)
            {
                var users = lstNotificationWorkflow.Select(c => c.UserId).ToList();
                await _userNotificationRepository.SendNotification(users);
            }

            var user = _connectionMappingRepository.GetUserInfoById(_userInfoToken.Id);

            if (user != null)
            {
                await _hubContext.Clients.AllExcept(new List<string> { user.ConnectionId }).RefreshDocuments(document.CategoryId);
                await _hubContext.Clients.AllExcept(new List<string> { user.ConnectionId }).RefreshWorkflows();
            }

        }
        catch (Exception)
        {

        }
        var result = _mapper.Map<WorkflowInstanceDto>(entity);
        return ServiceResponse<bool>.ReturnResultWith200(true);
    }
}
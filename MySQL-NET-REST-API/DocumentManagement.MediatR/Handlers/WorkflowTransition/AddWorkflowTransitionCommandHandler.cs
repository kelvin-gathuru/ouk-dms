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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;

public class AddWorkflowTransitionCommandHandler(
    IWorkflowTransitionRepository _workflowTransitionRepository,
    IWorkflowRepository _workflowRepository,
    IWorkflowStepRepository _workflowStepRepository,
    IUnitOfWork<DocumentContext> _uow,
    IWorkflowTransitionRoleRepository workflowTransitionRoleRepository,
    IWorkflowTransitionUserRepository workflowTransitionUserRepository,
    IMapper _mapper,
     IHubContext<UserHub, IHubClient> hubContext,
         IConnectionMappingRepository connectionMappingRepository,
         UserInfoToken _userInfo) : IRequestHandler<AddWorkflowTransitionCommand, ServiceResponse<List<WorkflowTransitionDto>>>
{
    public async Task<ServiceResponse<List<WorkflowTransitionDto>>> Handle(AddWorkflowTransitionCommand request, CancellationToken cancellationToken)
    {
        var workflowExist = await _workflowRepository.FindBy(c => c.Id == request.WorkflowTransitions.FirstOrDefault().WorkflowId).FirstOrDefaultAsync();
        if (workflowExist == null)
        {
            return ServiceResponse<List<WorkflowTransitionDto>>.Return404("Workflow is not exist.");
        }
        if (request.WorkflowTransitions == null || request.WorkflowTransitions.Count() == 0)
        {
            return ServiceResponse<List<WorkflowTransitionDto>>.Return404();
        }
        var entities = _mapper.Map<List<WorkflowTransition>>(request.WorkflowTransitions);
        var workflowTransitionRoles = new List<WorkflowTransitionRole>();
        var workflowTransitionUsers = new List<WorkflowTransitionUser>();

        for (var i = 0; i < entities.Count; i++)
        {
            var id = Guid.NewGuid();
            entities[i].Id = id;
            entities[i].CreatedAt = DateTime.UtcNow;
            entities[i].UpdatedAt = DateTime.UtcNow;
            entities[i].OrderNo = i;
            if (request.WorkflowTransitions[i].RoleIds != null && request.WorkflowTransitions[i].RoleIds.Count > 0)
            {
                foreach (var item in request.WorkflowTransitions[i].RoleIds)
                {
                    var workflowTransitionRole = new WorkflowTransitionRole();
                    workflowTransitionRole.WorkflowTransitionId = id;
                    workflowTransitionRole.RoleId = item;
                    workflowTransitionRoles.Add(workflowTransitionRole);
                }
            }
            if (request.WorkflowTransitions[i].UserIds != null && request.WorkflowTransitions[i].UserIds.Count > 0)
            {
                foreach (var item in request.WorkflowTransitions[i].UserIds)
                {
                    var workflowTransitionUser = new WorkflowTransitionUser();
                    workflowTransitionUser.WorkflowTransitionId = id;
                    workflowTransitionUser.UserId = item;
                    workflowTransitionUsers.Add(workflowTransitionUser);
                }
            }
        }

        _workflowTransitionRepository.AddRange(entities);
        if (workflowTransitionRoles.Count > 0)
        {
            workflowTransitionRoleRepository.AddRange(workflowTransitionRoles);
        }
        if (workflowTransitionUsers.Count > 0)
        {
            workflowTransitionUserRepository.AddRange(workflowTransitionUsers);
        }
        List<Guid> stepIds = new List<Guid>();
        for (var i = 0; i < request.WorkflowTransitions.Count; i++)
        {
            var transition = request.WorkflowTransitions[i];
            var currentTransition = request.WorkflowTransitions.Where(c => c.FromStepId == transition.ToStepId).FirstOrDefault();
            if (currentTransition == null)
            {
                if (!stepIds.Any(c => c == transition.ToStepId.Value))
                {
                    stepIds.Add(transition.ToStepId.Value);
                }
            }
        }
        workflowExist.IsWorkflowSetup = true;
        _workflowRepository.Update(workflowExist);
        var uniqueStepIds = stepIds.Distinct().ToList();
        for (var i = 0; i < uniqueStepIds.Count; i++)
        {
            var step = await _workflowStepRepository.FindBy(c => c.Id == uniqueStepIds[i]).FirstOrDefaultAsync();
            if (step != null)
            {
                step.IsFinal = true;
                _workflowStepRepository.Update(step);
            }
        }

        if (await _uow.SaveAsync() <= 0)
        {
            return ServiceResponse<List<WorkflowTransitionDto>>.Return500();
        }
        try
        {
            var user = connectionMappingRepository.GetUserInfoById(_userInfo.Id);

            if (user != null)
            {
                await hubContext.Clients.AllExcept(new List<string> { user.ConnectionId }).RefreshWorkflowSettings();
            }
        }
        catch (Exception)
        {

        }
        var entityDto = _mapper.Map<List<WorkflowTransitionDto>>(entities);
        return ServiceResponse<List<WorkflowTransitionDto>>.ReturnResultWith201(entityDto);
    }
}
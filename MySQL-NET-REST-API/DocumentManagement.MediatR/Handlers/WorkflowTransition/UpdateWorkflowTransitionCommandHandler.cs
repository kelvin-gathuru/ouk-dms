using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
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

public class UpdateWorkflowTransitionCommandHandler(
    IWorkflowTransitionRepository _workflowTransitionRepository,
    IWorkflowTransitionUserRepository workflowTransitionUserRepository,
    IWorkflowTransitionRoleRepository workflowTransitionRoleRepository,
    IUnitOfWork<DocumentContext> _uow,
    IWorkflowStepRepository _workflowStepRepository,
    IMapper _mapper,
    IHubContext<UserHub, IHubClient> hubContext,
         IConnectionMappingRepository connectionMappingRepository,
         UserInfoToken _userInfo)
    : IRequestHandler<UpdateWorkflowTransitionCommand, ServiceResponse<List<WorkflowTransitionDto>>>
{
    public async Task<ServiceResponse<List<WorkflowTransitionDto>>> Handle(UpdateWorkflowTransitionCommand request, CancellationToken cancellationToken)
    {
        var workflowId = request.WorkflowTransitions.Select(c => c.WorkflowId).FirstOrDefault();
        var workflowTransitions = await _workflowTransitionRepository.All
            .Include(c => c.WorkflowTransitionUsers)
            .Include(c => c.WorkflowTransitionRoles)
            .OrderBy(c => c.OrderNo)
            .Where(c => c.WorkflowId == workflowId).ToListAsync();

        if (workflowTransitions.Count == 0)
        {
            return ServiceResponse<List<WorkflowTransitionDto>>.Return404();
        }
        List<WorkflowTransitionUser> lstWorkflowTransitionUsers = new List<WorkflowTransitionUser>();
        List<WorkflowTransitionRole> lstWorkflowTransitionRoles = new List<WorkflowTransitionRole>();
        for (var i = 0; i < workflowTransitions.Count; i++)
        {
            if (workflowTransitions[i].WorkflowTransitionUsers.Count > 0)
            {
                lstWorkflowTransitionUsers.AddRange(workflowTransitions[i].WorkflowTransitionUsers);
            }
            if (workflowTransitions[i].WorkflowTransitionRoles.Count > 0)
            {
                lstWorkflowTransitionRoles.AddRange(workflowTransitions[i].WorkflowTransitionRoles);
            }
        }
        if (lstWorkflowTransitionUsers.Count > 0)
        {
            workflowTransitionUserRepository.RemoveRange(lstWorkflowTransitionUsers);

        }
        if (lstWorkflowTransitionRoles.Count > 0)
        {
            workflowTransitionRoleRepository.RemoveRange(lstWorkflowTransitionRoles);
        }

        workflowTransitions.ForEach(e =>
        {
            e.Name = request.WorkflowTransitions.Find(c => c.Id == e.Id).Name;
            e.IsFirstTransaction = request.WorkflowTransitions.Find(c => c.Id == e.Id).IsFirstTransaction;
            e.UpdatedAt = DateTime.UtcNow;
            e.Days = request.WorkflowTransitions.Find(c => c.Id == e.Id).Days;
            e.Hours = request.WorkflowTransitions.Find(c => c.Id == e.Id).Hours;
            e.Minutes = request.WorkflowTransitions.Find(c => c.Id == e.Id).Minutes;
            e.OrderNo = request.WorkflowTransitions.Find(c => c.Id == e.Id).OrderNo;
            e.IsUploadDocumentVersion = request.WorkflowTransitions.Find(c => c.Id == e.Id).IsUploadDocumentVersion;
            e.IsSignatureRequired = request.WorkflowTransitions.Find(c => c.Id == e.Id).IsSignatureRequired;
            e.Color = request.WorkflowTransitions.Find(c => c.Id == e.Id).Color;
            e.WorkflowTransitionRoles = request.WorkflowTransitions.Where(c => c.Id == e.Id).FirstOrDefault().RoleIds.Count > 0 ? request.WorkflowTransitions
                                        .Where(wt => wt.Id == e.Id)
                                        .FirstOrDefault()
                                        .RoleIds.Select(rl => new WorkflowTransitionRole
                                        {
                                            WorkflowTransitionId = e.Id,
                                            RoleId = rl
                                        }).ToList()
                                        : new List<WorkflowTransitionRole>();
            e.WorkflowTransitionUsers = request.WorkflowTransitions.Where(c => c.Id == e.Id).FirstOrDefault().UserIds.Count > 0 ? request.WorkflowTransitions
                                        .Where(wt => wt.Id == e.Id)
                                        .FirstOrDefault()
                                        .UserIds.Select(rl => new WorkflowTransitionUser
                                        {
                                            WorkflowTransitionId = e.Id,
                                            UserId = rl
                                        }).ToList()
                                        : new List<WorkflowTransitionUser>();
        });

        List<Guid> stepIds = new List<Guid>();
        for (var i = 0; i < workflowTransitions.Count; i++)
        {
            var transition = request.WorkflowTransitions[i];
            var currentTransition = request.WorkflowTransitions.Where(c => c.FromStepId == transition.ToStepId).FirstOrDefault();
            if (currentTransition == null)
            {
                stepIds.Add(transition.ToStepId.Value);
            }
        }
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
        _workflowTransitionRepository.UpdateRange(workflowTransitions);
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

        var entityDto = _mapper.Map<List<WorkflowTransitionDto>>(workflowTransitions);
        return ServiceResponse<List<WorkflowTransitionDto>>.ReturnResultWith201(entityDto);
    }
}

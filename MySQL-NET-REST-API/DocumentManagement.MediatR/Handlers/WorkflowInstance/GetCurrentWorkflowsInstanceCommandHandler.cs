using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;

public class GetCurrentWorkflowsInstanceCommandHandler(IWorkflowInstanceRepository _workflowInstanceRepository, IUserRoleRepository userRoleRepository, UserInfoToken userInfoToken)
    : IRequestHandler<GetCurrentWorkflowsInstanceQuery, ServiceResponse<List<CurrentWorkflowDataDto>>>
{
    public async Task<ServiceResponse<List<CurrentWorkflowDataDto>>> Handle(GetCurrentWorkflowsInstanceQuery request, CancellationToken cancellationToken)
    {
        var roles = userRoleRepository.All.Where(u => u.UserId == userInfoToken.Id).Select(r => r.RoleId).ToList();

        var entities = await _workflowInstanceRepository.All
            .Include(W => W.InitiatedBy)
            .Include(w => w.Workflow)
               .ThenInclude(ws => ws.WorkflowSteps)
            .Include(w => w.Workflow)
               .ThenInclude(ws => ws.WorkflowSteps)
            .Include(w => w.Workflow)
                .ThenInclude(c => c.WorkflowTransitions)
                    .ThenInclude(c => c.WorkflowTransitionRoles)
            .Include(w => w.Workflow)
                .ThenInclude(c => c.WorkflowTransitions)
                    .ThenInclude(c => c.WorkflowTransitionUsers)
            .Include(w => w.Document)
            .Include(c => c.WorkflowTransitionInstances)
                .ThenInclude(c => c.PerformBy)
            .Include(c => c.WorkflowTransitionInstances)
                .ThenInclude(c => c.WorkflowTransition)
            .Include(c => c.WorkflowStepInstances.Where(c => c.Status == Data.WorkflowStepInstanceStatus.InProgress))
            .Where(w => w.Status != Data.WorkflowInstanceStatus.Completed && w.Status != Data.WorkflowInstanceStatus.Cancelled && w.Workflow.WorkflowTransitions.Any(v => v.WorkflowTransitionRoles.Any(r => roles.Contains(r.RoleId)) || v.WorkflowTransitionUsers.Any(r => r.UserId == userInfoToken.Id)))
            .AsNoTracking()
            .AsSplitQuery()
            .ToListAsync();

        var currentWorkflowDataDtos = entities.Select(c => new CurrentWorkflowDataDto
        {
            WorkflowId = c.WorkflowId,
            WorkflowName = c.Workflow?.Name,
            WorkflowInstanceId = c.Id,
            WorkflowStepInstanceId = c.WorkflowStepInstances.FirstOrDefault()?.Id,
            WorkflowInstanceStatus = c.Status,
            WorkflowInitiatedDate = c.CreatedAt,
            WorkflowStepId = c.WorkflowStepInstances.FirstOrDefault()?.StepId,
            WorkflowStepName = c.Workflow?.WorkflowSteps.FirstOrDefault(ws => ws.Id == c.WorkflowStepInstances.FirstOrDefault()?.StepId)?.StepName,
            DocumentId = c.DocumentId,
            DocumentName = c.Document?.Name,
            DocumentNumber = c.Document?.DocumentNumber,
            DocumentUrl = c.Document?.Url,
            InitiatedUser = c.InitiatedBy != null ? $"{c.InitiatedBy.FirstName} {c.InitiatedBy.LastName}" : null,
            PerformBy = c.WorkflowTransitionInstances.OrderByDescending(wt => wt.UpdatedAt).FirstOrDefault().PerformBy != null ? $"{c.WorkflowTransitionInstances.OrderByDescending(wt => wt.UpdatedAt).FirstOrDefault().PerformBy.FirstName} {c.WorkflowTransitionInstances.OrderByDescending(wt => wt.UpdatedAt).FirstOrDefault().PerformBy.LastName}" : null,
            LastTransition = c.WorkflowTransitionInstances.OrderByDescending(wt => wt.UpdatedAt).FirstOrDefault(d => d.Status == WorkflowTransitionInstanceStatus.Completed)?.WorkflowTransition.Name,
            LastTransitionSteps = c.WorkflowTransitionInstances.OrderByDescending(wt => wt.UpdatedAt).FirstOrDefault(d => d.Status == WorkflowTransitionInstanceStatus.Completed) == null
            ? "" : c.Workflow.WorkflowSteps.FirstOrDefault(d => d.Id == c.WorkflowTransitionInstances.OrderByDescending(wt => wt.UpdatedAt).FirstOrDefault(d => d.Status == WorkflowTransitionInstanceStatus.Completed)?.WorkflowTransition.FromStepId)?.StepName + " - "
            + c.Workflow.WorkflowSteps.FirstOrDefault(d => d.Id == c.WorkflowTransitionInstances.OrderByDescending(wt => wt.UpdatedAt).FirstOrDefault(d => d.Status == WorkflowTransitionInstanceStatus.Completed)?.WorkflowTransition.ToStepId)?.StepName,
            UpdatedAt = c.UpdatedAt,
            LastTransitionComment = c.WorkflowTransitionInstances.OrderByDescending(wt => wt.UpdatedAt).FirstOrDefault()?.Comment,
            WorkflowTransitions = c.Workflow?.WorkflowTransitions.Where(t => t.FromStepId == c.WorkflowStepInstances.FirstOrDefault()?.StepId)
                .Select(d => new CurrentWorkflowTransitionDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    AllowRoleToPerformTransition = d.WorkflowTransitionRoles?.Any(r => roles.Contains(r.RoleId)) ?? false,
                    AllowUserToPerformTransition = d.WorkflowTransitionUsers?.Any(rc => rc.UserId == userInfoToken.Id) ?? false,
                    Comment = c.WorkflowStepInstances.FirstOrDefault()?.Comment,
                    IsSignatureRequired = d.IsSignatureRequired,
                    SignatureBy = c.Document?.SignById,
                    IsUserSignRequired = c.Document?.SignById == userInfoToken.Id ? false : true,
                    IsUploadDocumentVersion = d.IsUploadDocumentVersion,
                    Color = d.Color,
                    FromToStepName = c.Workflow?.WorkflowSteps.FirstOrDefault(ws => ws.Id == d.FromStepId)?.StepName + " - " + c.Workflow?.WorkflowSteps.FirstOrDefault(ws => ws.Id == d.ToStepId)?.StepName,
                })
                .Where(c => c.AllowRoleToPerformTransition || c.AllowUserToPerformTransition)
                .ToList()
        }).Where(c => c.WorkflowTransitions != null && c.WorkflowTransitions.Count > 0).ToList();


        return ServiceResponse<List<CurrentWorkflowDataDto>>.ReturnResultWith200(currentWorkflowDataDtos);
    }
}
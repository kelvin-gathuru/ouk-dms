using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagement.Repository;

public class AllWorkflowInstanceList : List<CurrentWorkflowDataDto>
{
    private readonly List<Guid> roles;
    private readonly Guid currentUserId;

    public int Skip { get; private set; }
    public int TotalPages { get; private set; }
    public int PageSize { get; private set; }
    public int TotalCount { get; private set; }

    public AllWorkflowInstanceList(List<CurrentWorkflowDataDto> items, int count, int skip, int pageSize)
    {
        TotalCount = count;
        PageSize = pageSize;
        Skip = skip;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        AddRange(items);
    }

    public AllWorkflowInstanceList(List<Guid> roles, Guid currentUserId)
    {
        this.roles = roles;
        this.currentUserId = currentUserId;
    }

    public async Task<AllWorkflowInstanceList> Create(IQueryable<WorkflowInstance> source, int skip, int pageSize)
    {
        var count = await GetCount(source);
        var dtoList = await GetDtos(source, skip, pageSize);
        var dtoPageList = new AllWorkflowInstanceList(dtoList, count, skip, pageSize);
        return dtoPageList;
    }

    public async Task<int> GetCount(IQueryable<WorkflowInstance> source)
    {
        return await source.AsNoTracking().CountAsync();
    }

    public async Task<List<CurrentWorkflowDataDto>> GetDtos(IQueryable<WorkflowInstance> source, int skip, int pageSize)
    {
        try
        {
            var workflowInstances = await source
                .Skip(skip)
                .Take(pageSize)
                .AsNoTracking()
                .AsSplitQuery()
                .ToListAsync();

            var dtos = workflowInstances.Select(c => new CurrentWorkflowDataDto
            {
                WorkflowId = c.WorkflowId,
                WorkflowName = c.Workflow?.Name,
                WorkflowInstanceId = c.Id,
                WorkflowStepInstanceId = c.WorkflowStepInstances
                    .OrderByDescending(ws => ws.CreatedAt)
                    .FirstOrDefault()?.Id,
                WorkflowInstanceStatus = c.Status,
                WorkflowStepId = c.WorkflowStepInstances
                    .OrderByDescending(ws => ws.CreatedAt)
                    .FirstOrDefault()?.StepId,
                WorkflowStepName = c.WorkflowStepInstances
                    .OrderByDescending(ws => ws.CreatedAt)
                    .FirstOrDefault()?.WorkflowStep.StepName,
                WorkflowStepInstanceStatus = (Data.WorkflowStepInstanceStatus)(c.WorkflowStepInstances
                    .OrderByDescending(ws => ws.CreatedAt)
                    .FirstOrDefault()?.Status),
                DocumentId = c.DocumentId,
                DocumentName = c.Document?.Name,
                DocumentNumber = c.Document?.DocumentNumber,
                DocumentUrl = c.Document?.Url,
                IsDocumentDeleted = c.Document.IsDeleted,
                InitiatedUser = c.InitiatedBy != null ? $"{c.InitiatedBy.FirstName} {c.InitiatedBy.LastName}" : null,
                PerformBy = c.WorkflowTransitionInstances
                    .OrderByDescending(wt => wt.UpdatedAt)
                    .FirstOrDefault()?.PerformBy != null ? $"{c.WorkflowTransitionInstances
                    .OrderByDescending(wt => wt.UpdatedAt)
                    .FirstOrDefault()?.PerformBy.FirstName} {c.WorkflowTransitionInstances
                    .OrderByDescending(wt => wt.UpdatedAt)
                    .FirstOrDefault()?.PerformBy.LastName}" : null,
                WorkflowInitiatedDate = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                LastTransition = c.WorkflowTransitionInstances.OrderByDescending(wt => wt.UpdatedAt).FirstOrDefault(d => d.Status == WorkflowTransitionInstanceStatus.Completed)?.WorkflowTransition.Name,
                LastTransitionSteps = c.WorkflowTransitionInstances.OrderByDescending(wt => wt.UpdatedAt).FirstOrDefault(d => d.Status == WorkflowTransitionInstanceStatus.Completed) == null
            ? "" : c.Workflow.WorkflowSteps.FirstOrDefault(d => d.Id == c.WorkflowTransitionInstances.OrderByDescending(wt => wt.UpdatedAt).FirstOrDefault(d => d.Status == WorkflowTransitionInstanceStatus.Completed)?.WorkflowTransition.FromStepId)?.StepName + " - "
            + c.Workflow.WorkflowSteps.FirstOrDefault(d => d.Id == c.WorkflowTransitionInstances.OrderByDescending(wt => wt.UpdatedAt).FirstOrDefault(d => d.Status == WorkflowTransitionInstanceStatus.Completed)?.WorkflowTransition.ToStepId)?.StepName,

                WorkflowTransitions = c.Workflow.WorkflowTransitions
                    .Where(t => t.FromStepId == c.WorkflowStepInstances
                        .FirstOrDefault(ws => ws.Status == Data.WorkflowStepInstanceStatus.InProgress)?.StepId)
                    .Select(d => new CurrentWorkflowTransitionDto
                    {
                        Id = d.Id,
                        Name = d.Name,
                        AllowRoleToPerformTransition = d.WorkflowTransitionRoles
                            .Any(r => roles.Contains(r.RoleId)),
                        AllowUserToPerformTransition = d.WorkflowTransitionUsers
                            .Any(r => r.UserId == currentUserId),
                        Comment = c.WorkflowStepInstances
                            .FirstOrDefault(wf => wf.StepId == d.FromStepId)?.Comment,
                        IsSignatureRequired = d.IsSignatureRequired,
                        SignatureBy = c.Document?.SignById,
                        IsUserSignRequired = c.Document?.SignById == currentUserId ? false : true,
                        IsUploadDocumentVersion = d.IsUploadDocumentVersion,
                        Color = d.Color,
                        FromToStepName = c.Workflow?.WorkflowSteps.FirstOrDefault(ws => ws.Id == d.FromStepId)?.StepName + " - " + c.Workflow?.WorkflowSteps.FirstOrDefault(ws => ws.Id == d.ToStepId)?.StepName,
                    })
                    .ToList()
            }).ToList();

            return dtos;
        }
        catch (Exception ex)
        {
            throw new DataException("Error while getting workflow instances", ex);
        }
    }
}

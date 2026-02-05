using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagement.Repository;

public class WorkflowLogList : List<WorkflowTransitionLogDto>
{
    public int Skip { get; private set; }
    public int TotalPages { get; private set; }
    public int PageSize { get; private set; }
    public int TotalCount { get; private set; }

    public WorkflowLogList(List<WorkflowTransitionLogDto> items, int count, int skip, int pageSize)
    {
        TotalCount = count;
        PageSize = pageSize;
        Skip = skip;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        AddRange(items);
    }

    public WorkflowLogList()
    {
    }

    public async Task<WorkflowLogList> Create(IQueryable<WorkflowTransitionInstance> source, int skip, int pageSize)
    {
        var count = await GetCount(source);
        var dtoList = await GetDtos(source, skip, pageSize);
        var dtoPageList = new WorkflowLogList(dtoList, count, skip, pageSize);
        return dtoPageList;
    }

    public async Task<int> GetCount(IQueryable<WorkflowTransitionInstance> source)
    {
        return await source
             .AsNoTracking()
             .AsSplitQuery()
            .CountAsync();
    }

    public async Task<List<WorkflowTransitionLogDto>> GetDtos(IQueryable<WorkflowTransitionInstance> source, int skip, int pageSize)
    {
        var entities = await source
            .Skip(skip)
            .Take(pageSize)
            .AsNoTracking()
            .AsSplitQuery()
            .Select(c => new WorkflowTransitionLogDto
            {
                WorkflowInstanceId = c.WorkflowInstanceId,
                WorkflowId = c.WorkflowInstance.Workflow.Id,
                WorkflowName = c.WorkflowInstance.Workflow.Name,
                DocumentId = c.WorkflowInstance.Document.Id,
                DocumentName = c.WorkflowInstance.Document != null ? c.WorkflowInstance.Document.Name : null,
                DocumentNumber = c.WorkflowInstance.Document != null ? c.WorkflowInstance.Document.DocumentNumber : null,
                IsDocumentDeleted = c.WorkflowInstance.Document.IsDeleted,
                DocumentUrl = c.WorkflowInstance.Document != null ? c.WorkflowInstance.Document.Url : null,
                WorkflowInstanceStatus = c.WorkflowInstance.Status,
                TransitionName = c.WorkflowTransition.Name,
                InitiatedBy = c.WorkflowInstance.InitiatedBy != null ? $"{c.WorkflowInstance.InitiatedBy.FirstName} {c.WorkflowInstance.InitiatedBy.LastName}" : null,
                Steps = c.WorkflowTransition.FromWorkflowStep.StepName + " -> " + c.WorkflowTransition.ToWorkflowStep.StepName,
                WorkflowTransitionInstanceStatus = c.Status,
                InititatedAt = c.WorkflowInstance.CreatedAt,
                TransitionDate = c.Status == Data.Entities.WorkflowTransitionInstanceStatus.Completed ? c.UpdatedAt : null,
                PerformBy = c.PerformBy != null ? $"{c.PerformBy.FirstName} {c.PerformBy.LastName}" : null,
                Comment = c.Comment
            }).ToListAsync();

        return entities;
    }
}

using AutoMapper;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;

public class GetVisualWorkflowInstanceCommandHandler(IWorkflowInstanceRepository workflowInstanceRepository, IWorkflowTransitionRepository workflowTransitionRepository, IMapper mapper) : IRequestHandler<GetVisualWorkflowInstanceCommand, ServiceResponse<VisualWorkflow>>
{
    public async Task<ServiceResponse<VisualWorkflow>> Handle(GetVisualWorkflowInstanceCommand request, CancellationToken cancellationToken)
    {
        var workflowInstance = await workflowInstanceRepository.All
             .Include(c => c.Workflow)
                .ThenInclude(c => c.WorkflowSteps)
              .Include(c => c.Workflow)
                .ThenInclude(c => c.WorkflowTransitions)
                    .ThenInclude(c => c.WorkflowTransitionRoles)
                          .ThenInclude(c => c.Role)
              .Include(c => c.Workflow)
                .ThenInclude(c => c.WorkflowTransitions)
                    .ThenInclude(c => c.WorkflowTransitionUsers)
                      .ThenInclude(c => c.User)
              //.ThenInclude(c => c.WorkflowStepRoles)
              //    .ThenInclude(c => c.Role)
              .Include(c => c.Workflow)
                .ThenInclude(c => c.WorkflowSteps)
             //.ThenInclude(c => c.WorkflowStepUsers)
             //    .ThenInclude(c => c.User)
             .Include(c => c.Workflow)
                .ThenInclude(c => c.WorkflowTransitions)
                   .ThenInclude(c => c.FromWorkflowStep)
              .Include(c => c.Workflow)
                .ThenInclude(c => c.WorkflowTransitions)
                   .ThenInclude(c => c.ToWorkflowStep)
             .Include(c => c.WorkflowStepInstances)
                .ThenInclude(c => c.User)
             .Include(c => c.WorkflowTransitionInstances)
                .ThenInclude(c => c.PerformBy)
             .Include(c => c.InitiatedBy)
             .Include(c => c.Document)
             .IgnoreQueryFilters()
             .AsNoTracking()
             .AsSplitQuery()
             .FirstOrDefaultAsync(c => c.Id == request.WorkflowInstanceId);

        var fromStepId = workflowInstance.WorkflowStepInstances.FirstOrDefault(c => c.Status == WorkflowStepInstanceStatus.InProgress)?.StepId;
        var pedingTranstions = new List<PendingTransition>();
        if (fromStepId != null && fromStepId != Guid.Empty && workflowInstance.Status != WorkflowInstanceStatus.Completed)
        {
            pedingTranstions = await workflowTransitionRepository.GetPendingTransitions(fromStepId.Value);
        }

        if (workflowInstance != null)
        {
            var visualWorkflow = new VisualWorkflow
            {
                WorkflowId = workflowInstance.WorkflowId,
                WorkflowName = workflowInstance.Workflow.Name,
                WorkflowInstanceStatus = workflowInstance.Status,
                InitiatedBy = workflowInstance.InitiatedBy != null ? workflowInstance.InitiatedBy.FirstName + ' ' + workflowInstance.InitiatedBy.LastName : "",
                WorkflowDescription = workflowInstance.Workflow.Description,
                DocumentId = workflowInstance.DocumentId,
                DocumentName = workflowInstance.Document.Name,
                DocumentNumber = workflowInstance.Document.DocumentNumber,
                CreatedAt = workflowInstance.CreatedAt,
                UpdatedAt = workflowInstance.UpdatedAt,
                PendingWorkflowTransitions = pedingTranstions.Count() == 0 ? new List<WorkflowTransitionDto>() : pedingTranstions.Select(c =>
                {
                    var fromStep = workflowInstance.Workflow?.WorkflowSteps?.FirstOrDefault(d => d.Id == c.FromStepId);
                    var toStep = workflowInstance.Workflow?.WorkflowSteps?.FirstOrDefault(d => d.Id == c.ToStepId);
                    var transition = workflowInstance.Workflow?.WorkflowTransitions?.FirstOrDefault(wft => wft.Id == c.TransitionId);

                    var assignRoles = transition?.WorkflowTransitionRoles != null && transition.WorkflowTransitionRoles.Any()
                        ? string.Join(",", transition.WorkflowTransitionRoles.Where(r => r.Role != null).Select(r => r.Role.Name))
                        : "";

                    var assignUsers = transition?.WorkflowTransitionUsers != null && transition.WorkflowTransitionUsers.Any()
                        ? string.Join(",", transition.WorkflowTransitionUsers.Where(u => u.User != null).Select(u => $"{u.User.FirstName} {u.User.LastName}"))
                        : "";

                    var userInstance = workflowInstance.WorkflowStepInstances?.FirstOrDefault(d => d.StepId == c.ToStepId)?.User;
                    var userDto = userInstance != null ? mapper.Map<UserDto>(userInstance) : null;

                    return new WorkflowTransitionDto
                    {
                        Id = c.TransitionId,
                        Name = c.TransitionName,
                        FromStepId = c.FromStepId,
                        FromStepName = fromStep?.StepName ?? "",
                        ToStepId = c.ToStepId,
                        ToStepName = toStep?.StepName ?? "",
                        CreatedAt = null,
                        AssignRoles = assignRoles,
                        AssignUsers = assignUsers,
                        CompletedAt = null,
                        Status = WorkflowTransitionInstanceStatus.InProgress,
                        User = userDto,
                        Comment = ""
                    };
                }).ToList(),
                CompletedWorkflowTransitionInstances = workflowInstance.WorkflowTransitionInstances.Where(c => c.Status != WorkflowTransitionInstanceStatus.Initiated).ToList().Count == 0 ? new List<WorkflowTransitionDto>() : workflowInstance.WorkflowTransitionInstances.Where(c => c.Status != WorkflowTransitionInstanceStatus.Initiated).Select(c => new WorkflowTransitionDto
                {
                    Id = c.WorkflowTransitionId,
                    Name = workflowInstance.Workflow.WorkflowTransitions.Where(wft => wft.Id == c.WorkflowTransitionId).FirstOrDefault()?.Name,
                    FromStepId = workflowInstance.Workflow.WorkflowTransitions.FirstOrDefault(d => d.Id == c.WorkflowTransitionId).FromWorkflowStep.Id,
                    FromStepName = workflowInstance.Workflow.WorkflowTransitions.FirstOrDefault(d => d.Id == c.WorkflowTransitionId).FromWorkflowStep.StepName,
                    ToStepId = workflowInstance.Workflow.WorkflowTransitions.Where(wft => wft.Id == c.WorkflowTransitionId).FirstOrDefault().ToStepId,
                    ToStepName = workflowInstance.Workflow.WorkflowTransitions.Where(wft => wft.Id == c.WorkflowTransitionId).FirstOrDefault().ToWorkflowStep.StepName,
                    CreatedAt = c.CreatedAt,
                    CompletedAt = c.UpdatedAt,
                    Status = c.Status,
                    User = c.PerformById != null ? mapper.Map<UserDto>(c.PerformBy) : null,
                    Comment = c.Comment
                }).ToList(),
                Nodes = workflowInstance.Workflow.WorkflowSteps.Select(c => new Node
                {
                    Id = c.Id,
                    label = c.StepName,
                    Timestamp = workflowInstance.WorkflowStepInstances.FirstOrDefault(d => d.StepId == c.Id)?.CreatedAt.ToString(),
                    Comment = workflowInstance.WorkflowStepInstances.FirstOrDefault(d => d.StepId == c.Id)?.Comment,
                    data = new WorkflowStepDto
                    {
                        Id = c.Id,
                        StepName = c.StepName,
                        Status = workflowInstance.WorkflowStepInstances.FirstOrDefault(d => d.StepId == c.Id)?.Status ?? Data.WorkflowStepInstanceStatus.InProgress,
                        CreatedAt = workflowInstance.WorkflowStepInstances.FirstOrDefault(d => d.StepId == c.Id)?.CreatedAt,
                        UpdatedAt = c.UpdatedAt,
                        CompletedAt = workflowInstance.WorkflowStepInstances.FirstOrDefault(d => d.StepId == c.Id)?.CompletedAt,
                        WorkflowId = c.WorkflowId,
                        UserId = workflowInstance.WorkflowStepInstances.FirstOrDefault(d => d.StepId == c.Id)?.UserId,
                        User = workflowInstance.WorkflowStepInstances.FirstOrDefault(d => d.StepId == c.Id)?.User != null ? mapper.Map<UserDto>(workflowInstance.WorkflowStepInstances.FirstOrDefault(d => d.StepId == c.Id)?.User) : null,
                        Color = workflowInstance.WorkflowStepInstances.FirstOrDefault(d => d.StepId == c.Id)?.Status == WorkflowStepInstanceStatus.Completed ? "white" : "black"
                    }
                }).ToList(),
                Links = workflowInstance.Workflow.WorkflowTransitions.Select(c => new Link
                {
                    Source = c.FromStepId,
                    Target = c.ToStepId,
                    Label = c.Name,
                    Status = pedingTranstions.Count > 0 ? pedingTranstions.Where(d => d.TransitionId == c.Id).FirstOrDefault() != null ? WorkflowTransitionInstanceStatus.InProgress
                    : workflowInstance.WorkflowTransitionInstances.FirstOrDefault(d => d.WorkflowTransitionId == c.Id)?.Status ?? WorkflowTransitionInstanceStatus.InProgress
                    : workflowInstance.WorkflowTransitionInstances.FirstOrDefault(d => d.WorkflowTransitionId == c.Id)?.Status ?? WorkflowTransitionInstanceStatus.InProgress
                }).ToList(),
                CustomColors = workflowInstance.Workflow.WorkflowSteps.Select(c => new CustomColor
                {
                    Name = c.StepName,
                    Value = pedingTranstions.Count > 0 ? pedingTranstions.Where(d => d.FromStepId == c.Id).FirstOrDefault() != null ? "red" :
                  workflowInstance.WorkflowStepInstances.FirstOrDefault(d => d.StepId == c.Id)?.Status == WorkflowStepInstanceStatus.Completed ? "green" : "red"
                  : workflowInstance.WorkflowStepInstances.FirstOrDefault(d => d.StepId == c.Id)?.Status == WorkflowStepInstanceStatus.Completed ? "green" : "red"

                }).ToList(),
            };
            return ServiceResponse<VisualWorkflow>.ReturnResultWith200(visualWorkflow);
        }
        return ServiceResponse<VisualWorkflow>.ReturnResultWith204();
    }
}

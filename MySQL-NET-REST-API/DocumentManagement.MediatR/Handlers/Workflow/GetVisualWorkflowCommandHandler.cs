using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;

public class GetVisualWorkflowCommandHandler(IWorkflowRepository _workflowRepository) : IRequestHandler<GetVisualWorkflowCommand, ServiceResponse<VisualWorkflow>>
{
    public async Task<ServiceResponse<VisualWorkflow>> Handle(GetVisualWorkflowCommand request, CancellationToken cancellationToken)
    {
        var entity = await _workflowRepository.All
            .Include(a => a.WorkflowSteps)
            .Include(a => a.WorkflowTransitions)
                .ThenInclude(c => c.WorkflowTransitionRoles)
                    .ThenInclude(c => c.Role)
            .Include(a => a.WorkflowTransitions)
                .ThenInclude(c => c.WorkflowTransitionUsers)
                    .ThenInclude(c => c.User)
            .FirstOrDefaultAsync(w => w.Id == request.WorkflowId);


        if (entity != null)
        {
            var visualWorkflow = new VisualWorkflow
            {
                WorkflowId = entity.Id,
                WorkflowName = entity.Name,
                WorkflowDescription = entity.Description,
                PendingWorkflowTransitions = entity.WorkflowTransitions.Count() == 0 ? new List<WorkflowTransitionDto>() : entity.WorkflowTransitions.OrderBy(c => c.OrderNo).Select(c => new WorkflowTransitionDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    FromStepId = c.FromStepId,
                    FromStepName = entity.WorkflowSteps.FirstOrDefault(d => d.Id == c.FromStepId).StepName,
                    ToStepId = c.ToStepId,
                    ToStepName = entity.WorkflowSteps.FirstOrDefault(d => d.Id == c.ToStepId).StepName,
                    CreatedAt = null,

                    AssignRoles = c.WorkflowTransitionRoles.Select(d => d.RoleId).ToList().Count > 0 ?
                                    string.Join(",", c.WorkflowTransitionRoles.Select(d => d.Role.Name).ToList()) : "",
                    AssignUsers = c.WorkflowTransitionUsers.Select(d => d.UserId).ToList().Count > 0 ?
                                    string.Join(",", c.WorkflowTransitionUsers.Select(d => d.User.FirstName + ' ' + d.User.LastName).ToList()) : "",
                    CompletedAt = null,
                    Status = WorkflowTransitionInstanceStatus.InProgress,
                    Comment = ""
                }).ToList(),
                Nodes = entity.WorkflowSteps.Select(c => new Node
                {
                    Id = c.Id,
                    label = c.StepName,
                    Timestamp = entity.WorkflowSteps.FirstOrDefault(d => d.Id == c.Id)?.CreatedAt.ToString(),
                }).ToList(),
                Links = entity.WorkflowTransitions.OrderBy(c => c.OrderNo).Select(c => new Link
                {
                    Source = c.FromStepId,
                    Target = c.ToStepId,
                    Label = c.Name,
                }).ToList(),
                CustomColors = entity.WorkflowSteps.Select(c => new CustomColor
                {
                    Name = c.StepName,
                    Value = "#6777ef"
                }).ToList(),
            };
            return ServiceResponse<VisualWorkflow>.ReturnResultWith200(visualWorkflow);
        }
        return ServiceResponse<VisualWorkflow>.ReturnResultWith204();
    }
}

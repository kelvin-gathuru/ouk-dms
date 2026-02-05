using System;
using System.Collections.Generic;

namespace DocumentManagement.Data.Dto;

public class WorkflowStepDto
{
    public Guid? Id { get; set; }
    public Guid WorkflowId { get; set; }
    public WorkflowDto Workflow { get; set; }
    public bool IsSignatureRequired { get; set; }
    public string StepName { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public Guid? UserId { get; set; }
    public UserDto User { get; set; }
    public string Color { get; set; }
    public int OrderNo { get; set; }
    public WorkflowStepInstanceStatus Status { get; set; } = WorkflowStepInstanceStatus.InProgress;
    public List<WorkflowStepInstanceDto> WorkflowStepInstances { get; set; } = new List<WorkflowStepInstanceDto>();
    public List<WorkflowTransitionDto> WorkflowTransitions { get; set; } = new List<WorkflowTransitionDto>();
}

using System;
using System.Collections.Generic;

namespace DocumentManagement.Data.Dto;

public class CurrentWorkflowDataDto
{
    public Guid? WorkflowId { get; set; }
    public Guid WorkflowInstanceId { get; set; }
    public string WorkflowName { get; set; }
    public WorkflowInstanceStatus WorkflowInstanceStatus { get; set; }
    public Guid? DocumentId { get; set; }
    public string DocumentName { get; set; }
    public string DocumentNumber { get; set; }
    public string DocumentUrl { get; set; }
    public bool IsDocumentDeleted { get; set; }
    public Guid? WorkflowStepId { get; set; }
    public Guid? WorkflowStepInstanceId { get; set; }
    public string WorkflowStepName { get; set; }
    public string InitiatedUser { get; set; }
    public string PerformBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int OrderNo { get; set; }
    public string Color { get; set; }
    public string LastTransition { get; set; }
    public DateTime WorkflowInitiatedDate { get; set; }
    public string LastTransitionSteps { get; set; }
    public string LastTransitionComment { get; set; }
    public WorkflowStepInstanceStatus WorkflowStepInstanceStatus { get; set; }
    public List<CurrentWorkflowTransitionDto> WorkflowTransitions { get; set; } = new List<CurrentWorkflowTransitionDto>();
}

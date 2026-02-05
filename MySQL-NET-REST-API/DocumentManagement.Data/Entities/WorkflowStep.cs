using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace DocumentManagement.Data;

public class WorkflowStep
{
    public Guid Id { get; set; }
    public Guid WorkflowId { get; set; }
    [ForeignKey("WorkflowId")]
    public Workflow Workflow { get; set; }
    public string StepName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    //public bool IsSignatureRequired { get; set; }
    public bool IsFinal { get; set; }
    public int OrderNo { get; set; } = 0;
    public ICollection<WorkflowStepInstance> WorkflowStepInstances { get; set; }
    public ICollection<WorkflowTransition> FromWorkflowTransitions { get; set; }
    public ICollection<WorkflowTransition> ToWorkflowTransitions { get; set; }
    //public ICollection<WorkflowStepRole> WorkflowStepRoles { get; set; } = new List<WorkflowStepRole>();
    //public ICollection<WorkflowStepUser> WorkflowStepUsers { get; set; } = new List<WorkflowStepUser>();
}

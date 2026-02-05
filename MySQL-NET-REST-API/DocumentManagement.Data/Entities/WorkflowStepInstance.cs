using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManagement.Data;

public class WorkflowStepInstance
{
    public Guid Id { get; set; }
    public Guid WorkflowInstanceId { get; set; }
    [ForeignKey("WorkflowInstanceId")]
    public WorkflowInstance WorkflowInstance { get; set; }
    public Guid StepId { get; set; }
    [ForeignKey("StepId")]
    public WorkflowStep WorkflowStep { get; set; }
    public Guid UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }
    public WorkflowStepInstanceStatus Status { get; set; }
    public DateTime CompletedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Comment { get; set; }
}

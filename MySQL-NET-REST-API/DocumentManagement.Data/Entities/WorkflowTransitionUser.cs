using System;

namespace DocumentManagement.Data.Entities;
public class WorkflowTransitionUser
{
    public Guid WorkflowTransitionId { get; set; }
    public Guid UserId { get; set; }
    public WorkflowTransition WorkflowTransition { get; set; }
    public User User { get; set; }
}

using System;

namespace DocumentManagement.Data.Entities;
public class WorkflowTransitionRole
{
    public Guid WorkflowTransitionId { get; set; }
    public Guid RoleId { get; set; }
    public WorkflowTransition WorkflowTransition { get; set; }
    public Role Role { get; set; }

}

using System;

namespace DocumentManagement.Data.Dto;
public class WorkflowTransitionRoleDto
{
    public Guid? WorkflowTransitionId { get; set; }
    public Guid? RoleId { get; set; }
    public WorkflowTransitionDto WorkflowTransition { get; set; }
    public RoleDto Role { get; set; }
}

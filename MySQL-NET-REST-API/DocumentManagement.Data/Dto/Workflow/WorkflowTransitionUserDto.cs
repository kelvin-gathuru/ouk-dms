using System;

namespace DocumentManagement.Data.Dto;
public class WorkflowTransitionUserDto
{
    public Guid? WorkflowTransitionId { get; set; }
    public Guid? UserId { get; set; }
    public WorkflowTransitionDto WorkflowTransition { get; set; }
    public UserDto User { get; set; }
}

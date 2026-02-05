using DocumentManagement.Data.Entities;
using System;
using System.Collections.Generic;

namespace DocumentManagement.Data.Dto;

public class WorkflowTransitionDto
{
    public Guid Id { get; set; }
    public Guid WorkflowId { get; set; }
    public string Name { get; set; }
    public Workflow Workflow { get; set; }
    public Guid FromStepId { get; set; }
    public string FromStepName { get; set; }
    public Guid ToStepId { get; set; }
    public string ToStepName { get; set; }
    public WorkflowStep WorkflowStep { get; set; }
    public string Condition { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string AssignRoles { get; set; }
    public string AssignUsers { get; set; }
    public WorkflowTransitionInstanceStatus? Status { get; set; }
    public UserDto User { get; set; }
    public string Comment { get; set; }
    public int Days { get; set; }
    public int Minutes { get; set; }
    public int Hours { get; set; }
    public bool IsUploadDocumentVersion { get; set; }
    public bool IsSignatureRequired { get; set; }
    public int OrderNo { get; set; }
    public string Color { get; set; }
    public List<WorkflowTransitionUserDto> WorkflowTransitionUsers { get; set; } = new List<WorkflowTransitionUserDto>();
    public List<WorkflowTransitionRoleDto> WorkflowTransitionRoles { get; set; } = new List<WorkflowTransitionRoleDto>();
}

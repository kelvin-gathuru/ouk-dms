using DocumentManagement.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManagement.Data;

public class WorkflowTransition
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid WorkflowId { get; set; }
    [ForeignKey("WorkflowId")]
    public Workflow Workflow { get; set; }
    public Guid FromStepId { get; set; }
    [ForeignKey("FromStepId")]
    public WorkflowStep FromWorkflowStep { get; set; }
    public Guid ToStepId { get; set; }
    [ForeignKey("ToStepId")]
    public WorkflowStep ToWorkflowStep { get; set; }
    public string Condition { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsFirstTransaction { get; set; } = false;
    public int? Days { get; set; }
    public int? Hours { get; set; }
    public int? Minutes { get; set; }
    public bool IsUploadDocumentVersion { get; set; }
    public bool IsSignatureRequired { get; set; }
    public string Color { get; set; }
    public int OrderNo { get; set; }
    public ICollection<WorkflowTransitionRole> WorkflowTransitionRoles { get; set; } = new List<WorkflowTransitionRole>();
    public ICollection<WorkflowTransitionUser> WorkflowTransitionUsers { get; set; } = new List<WorkflowTransitionUser>();
}

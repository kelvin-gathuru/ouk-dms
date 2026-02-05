using System;
using System.Collections.Generic;

namespace DocumentManagement.Data.Dto;

public class WorkflowTransitionDataDto
{
    public Guid? Id { get; set; }
    public Guid? WorkflowId { get; set; }
    public string Name { get; set; }
    public Guid? FromStepId { get; set; }
    public Guid? ToStepId { get; set; }
    public bool IsFirstTransaction { get; set; }
    public int Days { get; set; } = 0;
    public int Minutes { get; set; } = 0;
    public int Hours { get; set; } = 0;
    public bool IsUploadDocumentVersion { get; set; }
    public List<Guid> RoleIds { get; set; }
    public List<Guid> UserIds { get; set; }
    public bool IsSignatureRequired { get; set; }
    public string Color { get; set; }
    public int OrderNo { get; set; }
}

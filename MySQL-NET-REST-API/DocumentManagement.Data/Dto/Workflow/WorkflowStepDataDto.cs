using System;

namespace DocumentManagement.Data.Dto;

public class WorkflowStepDataDto
{
    public Guid? Id { get; set; }
    public Guid? WorkflowId { get; set; }
    public string StepName { get; set; }
    public int OrderNo { get; set; }
    //public List<Guid> RoleIds { get; set; }
    //public List<Guid> UserIds { get; set; }
    //public bool IsSignatureRequired { get; set; }

}
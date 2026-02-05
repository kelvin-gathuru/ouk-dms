using System;

namespace DocumentManagement.Data.Dto;

public class CurrentWorkflowTransitionDto
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public bool AllowRoleToPerformTransition { get; set; }
    public bool AllowUserToPerformTransition { get; set; }
    public string Comment { get; set; }
    public bool IsSignatureRequired { get; set; }
    public bool IsUserSignRequired { get; set; }
    public Guid? SignatureBy { get; set; }
    public bool IsUploadDocumentVersion { get; set; }
    public string Color { get; set; }
    public int OrderNo { get; set; }
    public string FromToStepName { get; set; }

}

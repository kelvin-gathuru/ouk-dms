using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class PerformWorkflowTransitionWithSignatureAndDocumentCommand : IRequest<ServiceResponse<bool>>
    {

        public Guid WorkflowInstanceId { get; set; }
        public Guid TransitionId { get; set; }
        public Guid WorkflowStepInstanceId { get; set; }
        public bool IsUploadDocumentVersion { get; set; }
        public bool IsSignatureRequired { get; set; }
        public string Comment { get; set; }
        public string Signature { get; set; }
        public Guid? DocumentId { get; set; }
        public Guid? DocumentVersionId { get; set; }
    }
}

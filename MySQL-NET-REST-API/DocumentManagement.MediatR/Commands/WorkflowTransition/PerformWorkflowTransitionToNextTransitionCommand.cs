using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class PerformWorkflowTransitionToNextTransitionCommand: IRequest<ServiceResponse<bool>>
    {
        public Guid TransitionId { get; set; }
        public Guid WorkflowInstanceId { get; set; }
        public Guid WorkflowStepInstanceId { get; set; }
        public string Comment { get; set; }
        public string Signature { get; set; }
        public string Url { get; set; }
        public string Extension { get; set; }
        public bool IsSignatureRequired { get; set; }
        public bool IsUploadDocumentVersion { get; set; }
    }
}

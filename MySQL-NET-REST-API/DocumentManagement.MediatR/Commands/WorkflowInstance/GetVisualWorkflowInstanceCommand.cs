using System;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;

namespace DocumentManagement.MediatR.Commands
{
    public class GetVisualWorkflowInstanceCommand: IRequest<ServiceResponse<VisualWorkflow>>
    {
        public Guid WorkflowInstanceId { get; set; }
    }
}

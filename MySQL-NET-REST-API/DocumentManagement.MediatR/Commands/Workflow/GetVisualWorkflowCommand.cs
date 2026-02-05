using System;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;

namespace DocumentManagement.MediatR.Commands
{
    public class GetVisualWorkflowCommand : IRequest<ServiceResponse<VisualWorkflow>>
    {
        public Guid WorkflowId { get; set; }
    }
}

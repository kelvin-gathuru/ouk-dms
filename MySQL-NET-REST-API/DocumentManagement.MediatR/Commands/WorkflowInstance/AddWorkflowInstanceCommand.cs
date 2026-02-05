using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class AddWorkflowInstanceCommand: IRequest<ServiceResponse<WorkflowInstanceDto>>
    {
        public Guid WorkflowId { get; set; }
        public Guid DocumentId { get; set; }
    }
}

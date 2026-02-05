using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class UpdateWorkflowInstanceCommand : IRequest<ServiceResponse<WorkflowInstanceDto>>
    {
        public Guid Id { get; set; }
        public Guid WorkflowId { get; set; }
        public Guid DocumentId { get; set; }
        public Guid UserId { get; set; }
        public WorkflowInstanceStatus Status { get; set; }
    }
}

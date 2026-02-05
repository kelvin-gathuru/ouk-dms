using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Queries
{
    public class GetWorkflowInstanceQuery : IRequest<ServiceResponse<WorkflowInstanceDto>>
    {
        public Guid Id { get; set; }
    }
}

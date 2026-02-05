using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Queries
{
    public class GetWorkflowQuery : IRequest<ServiceResponse<WorkflowDto>>
    {
        public Guid Id { get; set; }
    }
}

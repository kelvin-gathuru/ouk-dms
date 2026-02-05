using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Queries
{
    public class GetWorkflowStepQuery : IRequest<ServiceResponse<WorkflowStepDto>>
    {
        public Guid Id { get; set; }
    }
}
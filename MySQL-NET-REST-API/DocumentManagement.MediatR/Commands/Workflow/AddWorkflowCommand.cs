using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class AddWorkflowCommand : IRequest<ServiceResponse<WorkflowDto>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System;
using System.Collections.Generic;

namespace DocumentManagement.MediatR.Commands
{
    public class UpdateWorkflowTransitionCommand : IRequest<ServiceResponse<List<WorkflowTransitionDto>>>
    {
        public List<WorkflowTransitionDataDto> WorkflowTransitions { get; set; }
        
    }
}

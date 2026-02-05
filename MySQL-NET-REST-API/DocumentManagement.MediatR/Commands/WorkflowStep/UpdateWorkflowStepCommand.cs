using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System;
using System.Collections.Generic;

namespace DocumentManagement.MediatR.Commands
{
    public class UpdateWorkflowStepCommand : IRequest<ServiceResponse<List<WorkflowStepDto>>>
    {
        public List<WorkflowStepDataDto> WorkflowSteps { get; set; }
    }
}

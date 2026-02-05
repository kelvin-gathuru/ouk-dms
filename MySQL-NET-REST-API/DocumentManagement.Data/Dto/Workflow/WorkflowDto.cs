using System.Collections.Generic;
using System;

namespace DocumentManagement.Data.Dto
{
    public class WorkflowDto
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsWorkflowSetup { get; set; }
        public UserDto User { get; set; }
        public List<WorkflowStepDto> WorkflowSteps { get; set; } = new List<WorkflowStepDto>();
        public List<WorkflowInstanceDto> WorkflowInstances { get; set; } = new List<WorkflowInstanceDto>();
        public List<WorkflowTransitionDto> WorkflowTransitions { get; set; } = new List<WorkflowTransitionDto>();
    }
}

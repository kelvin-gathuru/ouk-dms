using System;
using System.Collections.Generic;

namespace DocumentManagement.Data.Dto
{
    public class WorkflowInstanceDto
    {
        public Guid Id { get; set; }
        public Guid? WorkflowId { get; set; }
        public Guid? DocumentId { get; set; }
        public DocumentDto Document { get; set; }
        public WorkflowDto Workflow { get; set; }
        public WorkflowInstanceStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<WorkflowStepInstanceDto> WorkflowStepInstances { get; set; } = new List<WorkflowStepInstanceDto>();
        public List<WorkflowTransitionDto> WorkflowTransitions { get; set; } = new List<WorkflowTransitionDto>();
    }
}

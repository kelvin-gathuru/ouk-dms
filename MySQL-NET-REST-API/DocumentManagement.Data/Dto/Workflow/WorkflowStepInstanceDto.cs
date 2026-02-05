using System;

namespace DocumentManagement.Data.Dto
{
    public class WorkflowStepInstanceDto
    {
        public Guid Id { get; set; }
        public Guid WorkflowInstanceId { get; set; }
        public Guid? UserId { get; set; }
        public WorkflowInstanceDto WorkflowInstance { get; set; }
        public Guid StepId { get; set; }
        public UserDto User { get; set; }
        public WorkflowStepDto WorkflowStep { get; set; }
        public WorkflowStepInstanceStatus Status { get; set; }
        public DateTime CompletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}

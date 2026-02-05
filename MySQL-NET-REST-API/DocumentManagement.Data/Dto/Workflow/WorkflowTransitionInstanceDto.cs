using DocumentManagement.Data.Entities;
using System;

namespace DocumentManagement.Data.Dto
{
    public class WorkflowTransitionInstanceDto
    {
        public Guid Id { get; set; }
        public WorkflowTransitionInstanceStatus Status { get; set; }
        public Guid WorkflowTransitionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Comment { get; set; }
        public Guid WorkflowInstanceId { get; set; }
        public string TransitionName { get; set; }
    }
}

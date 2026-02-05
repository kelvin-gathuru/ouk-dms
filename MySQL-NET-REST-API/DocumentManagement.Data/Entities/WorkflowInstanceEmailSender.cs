using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace DocumentManagement.Data.Entities
{
    public class WorkflowInstanceEmailSender
    {
        public Guid Id { get; set; }
        public Guid WorkflowStepInstanceId { get; set; }
        [ForeignKey("WorkflowStepInstanceId")]
        public WorkflowStepInstance WorkflowStepInstance { get; set; }
        public Guid WorkflowTransitionId { get; set; }
        [ForeignKey("WorkflowTransitionId")]
        public WorkflowTransition WorkflowTransition { get; set; }
        public DateTime CreatedAt { get; set; }
      
    }
}

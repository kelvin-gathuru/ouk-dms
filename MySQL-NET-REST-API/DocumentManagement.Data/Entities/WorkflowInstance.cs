using DocumentManagement.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManagement.Data
{
    public class WorkflowInstance
    {
        public Guid Id { get; set; }
        public Guid WorkflowId { get; set; }
      
        public Guid DocumentId { get; set; }
        [ForeignKey("WorkflowId")]
        public Workflow Workflow { get; set; }
        [ForeignKey("DocumentId")]
        public Document Document { get; set; }
        public WorkflowInstanceStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid? InitiatedId { get; set; }
        [ForeignKey("InitiatedId")]
        public User InitiatedBy { get; set; }
        public ICollection<WorkflowStepInstance> WorkflowStepInstances { get; set; }
        public ICollection<WorkflowTransitionInstance> WorkflowTransitionInstances { get; set; }
    }
}

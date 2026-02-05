using DocumentManagement.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManagement.Data
{
    public class WorkflowTransitionInstance
    {
        public Guid Id { get; set; }

        public WorkflowTransitionInstanceStatus Status { get; set; }
        public Guid WorkflowTransitionId { get; set; }
        [ForeignKey("WorkflowTransitionId")]
        public WorkflowTransition WorkflowTransition { get; set; }
        public Guid WorkflowInstanceId { get; set; }
        [ForeignKey("WorkflowInstanceId")]
        public WorkflowInstance WorkflowInstance { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Comment { get; set; }
        public Guid? PerformById { get; set; }
        [ForeignKey("PerformById")]
        public User PerformBy { get; set; }
    }
}

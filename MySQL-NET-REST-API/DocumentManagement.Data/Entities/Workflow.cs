using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManagement.Data
{
    public class Workflow : BaseEntity
    { 
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsWorkflowSetup { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public ICollection<WorkflowStep> WorkflowSteps { get; set; }
        public ICollection<WorkflowInstance> WorkflowInstances { get; set; }
        public ICollection<WorkflowTransition> WorkflowTransitions { get; set; }
    }
}

using DocumentManagement.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManagement.Data.Dto
{
    public class WorkflowLogDto
    {
        public string WorkflowName { get; set; }
        public WorkflowInstanceStatus WorkflowInstanceStatus { get; set; }
        public Guid WorkflowInstanceId { get; set; }
        public Guid DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string TransitionName { get; set; }
        public string WorkflowStep { get; set; }
        public string PerformBy { get; set; }
        public WorkflowTransitionInstanceStatus WorkflowTransitionInstanceStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}

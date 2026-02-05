using System;
using DocumentManagement.Data.Entities;

namespace DocumentManagement.Data.Dto
{
    public class WorkflowTransitionLogDto
    {
        public Guid WorkflowInstanceId { get; set; }
        public Guid WorkflowId { get; set; }
        public string WorkflowName { get; set; }
        public Guid DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentUrl { get; set; }
        public WorkflowInstanceStatus WorkflowInstanceStatus { get; set; }
        public string TransitionName { get; set; }
        public string InitiatedBy { get; set; }
        public string Steps { get; set; }
        public WorkflowTransitionInstanceStatus WorkflowTransitionInstanceStatus { get; set; }
        public DateTime InititatedAt { get; set; }
        public DateTime? TransitionDate { get; set; }
        public bool IsDocumentDeleted { get; set; }
        public string PerformBy { get; set; }
        public string Comment { get; set; }
    }
}

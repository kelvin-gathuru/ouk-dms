using DocumentManagement.Data.Entities;
using System;
using System.Collections.Generic;

namespace DocumentManagement.Data.Dto
{
    public class VisualWorkflow
    {
        public Guid WorkflowId { get; set; }
        public string WorkflowName { get; set; }
        public string WorkflowDescription { get; set; }
        public WorkflowInstanceStatus WorkflowInstanceStatus { get; set; }
        public string InitiatedBy { get; set; }
        public Guid DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<WorkflowTransitionDto> PendingWorkflowTransitions { get; set; }
        public List<WorkflowTransitionDto> CompletedWorkflowTransitionInstances { get; set; }
        public List<Node> Nodes { get; set; }
        public List<Link> Links { get; set; }
        public List<CustomColor> CustomColors { get; set; }
    }

    public class Node
    {
        public Guid Id { get; set;}
        public string label { get; set; }
        public WorkflowStepDto data { get; set; }
        public string Timestamp { get; set; }
        public string Comment { get; set; }
    }

    public class Link
    {
        public Guid Source { get; set; }
        public Guid Target { get; set; }
        public string Label { get; set; }
        public WorkflowTransitionInstanceStatus  Status { get; set; }
    }
    public class CustomColor
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}

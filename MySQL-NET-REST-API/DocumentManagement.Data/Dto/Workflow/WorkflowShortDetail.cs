using System;

namespace DocumentManagement.Data.Dto
{
    public class WorkflowShortDetail
    {
        public Guid WorkflowId { get; set; }
        public Guid WorkflowInstanceId { get; set; }
        public string WorkflowName { get; set; }
        public WorkflowInstanceStatus WorkflowInstaceStatus { get; set; }
    }
}

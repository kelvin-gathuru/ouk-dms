namespace DocumentManagement.Data.Resources
{
    public class WorkflowLogResource : ResourceParameter
    {
        public WorkflowLogResource() : base("WorkflowInstanceStatus")
        {
        }
        public string WorkflowId { get; set; }
        public WorkflowInstanceStatus WorkflowInstanceStatus { get; set; }
        public string DocumentId { get; set; }
    }
}

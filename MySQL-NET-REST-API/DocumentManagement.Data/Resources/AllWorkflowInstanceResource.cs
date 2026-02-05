using DocumentManagement.Data.Dto;
using System;
using System.Collections.Generic;

namespace DocumentManagement.Data.Resources
{
    public class AllWorkflowInstanceResource : ResourceParameter
    {
        public AllWorkflowInstanceResource() : base("WorkflowInstanceStatus")
        {
        }
        public string WorkflowId { get; set; }
        public WorkflowInstanceStatus WorkflowInstanceStatus { get; set; }
        public string DocumentId { get; set; }
    }
}

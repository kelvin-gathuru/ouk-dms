using System;

namespace DocumentManagement.Data.Dto
{
    public class NotificationWorkflowDto
    {
        public Guid UserId { get; set; }
        public string Message { get; set; }
        public Guid WorkflowInstanceId { get; set; }
        public Guid DocumentId { get; set; }
    }
}

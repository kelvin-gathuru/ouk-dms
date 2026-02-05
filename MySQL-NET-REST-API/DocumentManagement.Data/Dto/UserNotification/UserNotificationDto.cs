using System;

namespace DocumentManagement.Data.Dto
{
    public class UserNotificationDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public Guid? DocumentId { get; set; }
        public string DocumentName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Url { get; set; }
        public DocumentDto Document { get; set; }
        public NotificationsType NotificationsType { get; set; }
        public string WorkflowName { get; set; }
        public Guid? WorkflowInstanceId { get; set; }
        public string DocumentNumber { get; set; }

    }
}

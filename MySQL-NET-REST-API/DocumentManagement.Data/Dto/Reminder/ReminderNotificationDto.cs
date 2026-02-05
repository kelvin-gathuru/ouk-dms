using System;

namespace DocumentManagement.Data.Dto
{
    public class ReminderNotificationDto
    {
        public Guid Id { get; set; }
        public Guid ReminderId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime FetchDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsEmailNotification { get; set; }
        public ReminderDto Reminder { get; set; }
    }
}

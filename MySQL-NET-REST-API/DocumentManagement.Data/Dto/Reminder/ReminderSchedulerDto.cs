using System;

namespace DocumentManagement.Data.Dto
{
    public class ReminderSchedulerDto
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

using System;

namespace DocumentManagement.Data.Dto
{
    public class DailyReminderDto
    {
        public Guid? Id { get; set; }
        public Guid? ReminderId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public bool IsActive { get; set; }
    }
}

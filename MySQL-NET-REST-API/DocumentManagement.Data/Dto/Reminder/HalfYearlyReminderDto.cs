using System;

namespace DocumentManagement.Data.Dto
{
    public class HalfYearlyReminderDto
    {
        public Guid? Id { get; set; }
        public Guid? ReminderId { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public QuarterEnum Quarter { get; set; }
    }
}

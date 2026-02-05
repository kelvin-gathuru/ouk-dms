using System;

namespace DocumentManagement.Data.Dto
{
    public class CalenderReminderDto
    {
        public Guid RemiderId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Title { get; set; }
    }
}

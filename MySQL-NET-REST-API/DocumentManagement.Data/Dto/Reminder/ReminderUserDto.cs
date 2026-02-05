using System;

namespace DocumentManagement.Data.Dto
{
    public class ReminderUserDto
    {
        public Guid? ReminderId { get; set; }
        public Guid UserId { get; set; }
    }
}

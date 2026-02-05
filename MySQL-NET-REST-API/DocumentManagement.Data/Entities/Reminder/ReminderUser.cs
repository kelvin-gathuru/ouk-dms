using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManagement.Data
{
    public class ReminderUser
    {
        public Guid ReminderId { get; set; }
        public Guid UserId { get; set; }
        public Reminder Reminder { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}

using DocumentManagement.Helper;

namespace DocumentManagement.Data.Resources
{
    public class ReminderResource : ResourceParameters
    {
        public ReminderResource() : base("CreatedDate")
        {
        }
        public string Subject { get; set; }
        public string Message { get; set; }
        public Frequency? Frequency { get; set; }
    }
}

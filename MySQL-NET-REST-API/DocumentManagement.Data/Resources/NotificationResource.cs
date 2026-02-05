namespace DocumentManagement.Data.Resources
{
    public class NotificationResource : ResourceParameter
    {
        public NotificationResource() : base("CreatedDate")
        {
        }

        public string Name { get; set; }
    }
}

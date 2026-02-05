namespace DocumentManagement.Data.Resources
{
    public class NLogResource : ResourceParameter
    {
        public NLogResource() : base("Logged")
        {
        }

        public string Message { get; set; }

    }
}

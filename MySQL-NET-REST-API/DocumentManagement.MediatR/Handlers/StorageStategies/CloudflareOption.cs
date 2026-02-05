namespace DocumentManagement.MediatR.Handlers
{
    public class CloudflareOption
    {
        public string AccountId { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string BucketName { get; set; }
        public string R2Endpoint { get; set; }
    }
}

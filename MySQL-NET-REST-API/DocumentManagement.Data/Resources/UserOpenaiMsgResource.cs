namespace DocumentManagement.Data.Resources;
public class UserOpenaiMsgResource : ResourceParameter
{
    public UserOpenaiMsgResource() : base("CreatedDate")
    {
    }
    public string Title { get; set; }
    public string PromptInput { get; set; }
}

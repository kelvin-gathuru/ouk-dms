using System;

namespace DocumentManagement.Data.Dto;
public class UserOpenaiMsgResponseDto
{
    public Guid Id { get; set; }
    public string AiResponse { get; set; }
    public string Title { get; set; }
    public string PromptInput { get; set; }
    public string Language { get; set; }
    public int MaximumLength { get; set; }
    public decimal Creativity { get; set; }
    public string ToneOfVoice { get; set; }
    public string SelectedModel { get; set; }
    public DateTime CreatedDate { get; set; }
}

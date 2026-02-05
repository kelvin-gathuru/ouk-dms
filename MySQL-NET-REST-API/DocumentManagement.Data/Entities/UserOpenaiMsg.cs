using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManagement.Data.Entities;
public class UserOpenaiMsg : BaseEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string PromptInput { get; set; }
    public string Language { get; set; }
    public int MaximumLength { get; set; }
    public decimal Creativity { get; set; }
    public string ToneOfVoice { get; set; }
    public string SelectedModel { get; set; }
    [ForeignKey("CreatedBy")]
    public User CreatedByUser { get; set; }
    public string AiResponse { get; set; }

}

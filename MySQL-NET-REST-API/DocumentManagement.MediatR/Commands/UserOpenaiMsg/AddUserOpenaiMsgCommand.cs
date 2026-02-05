using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;

namespace DocumentManagement.MediatR.Commands;
public class AddUserOpenaiMsgCommand : IRequest<ServiceResponse<UserOpenaiMsgDto>>
{
    public string Title { get; set; }
    public string PromptInput { get; set; }
    public string Language { get; set; }
    public int MaximumLength { get; set; }
    public decimal Creativity { get; set; }
    public string ToneOfVoice { get; set; }
    public string SelectedModel { get; set; }
}

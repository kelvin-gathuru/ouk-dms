using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.MediatR.Handlers;
public class GetUserOpenaiMsgResponseByIdCommandHandler(IUserOpenaiMsgRepository _userOpenaiMsgRepository) : IRequestHandler<GetUserOpenaiMsgResponseByIdCommand, ServiceResponse<UserOpenaiMsgResponseDto>>
{
    public async Task<ServiceResponse<UserOpenaiMsgResponseDto>> Handle(GetUserOpenaiMsgResponseByIdCommand request, CancellationToken cancellationToken)
    {
        var userOpenaiMsg = await _userOpenaiMsgRepository.All.Where(c => c.Id == request.Id).FirstOrDefaultAsync();
        if (userOpenaiMsg == null)
        {
            return ServiceResponse<UserOpenaiMsgResponseDto>.Return404("Open ai message not found");
        }
        var responseDto = new UserOpenaiMsgResponseDto
        {
            Id = userOpenaiMsg.Id,
            AiResponse = userOpenaiMsg.AiResponse,
            Title = userOpenaiMsg.Title,
            PromptInput = userOpenaiMsg.PromptInput,
            Language = userOpenaiMsg.Language,
            MaximumLength = userOpenaiMsg.MaximumLength,
            Creativity = userOpenaiMsg.Creativity,
            ToneOfVoice = userOpenaiMsg.ToneOfVoice,
            SelectedModel = userOpenaiMsg.SelectedModel,
            CreatedDate = userOpenaiMsg.CreatedDate
        };
        return ServiceResponse<UserOpenaiMsgResponseDto>.ReturnResultWith200(responseDto);
    }
}

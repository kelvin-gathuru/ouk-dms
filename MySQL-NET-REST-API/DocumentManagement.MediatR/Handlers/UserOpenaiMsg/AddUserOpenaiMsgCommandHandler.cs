using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;
public class AddUserOpenaiMsgCommandHandler(
    IUserOpenaiMsgRepository userOpenaiMsgRepository,
    IMapper mapper,
    IUnitOfWork<DocumentContext> uow) : IRequestHandler<AddUserOpenaiMsgCommand, ServiceResponse<UserOpenaiMsgDto>>
{


    public async Task<ServiceResponse<UserOpenaiMsgDto>> Handle(AddUserOpenaiMsgCommand request, CancellationToken cancellationToken)
    {
        var userOpenaiMsg = new UserOpenaiMsg
        {
            Title = request.Title,
            PromptInput = request.PromptInput,
            Language = request.Language,
            MaximumLength = request.MaximumLength,
            Creativity = request.Creativity,
            ToneOfVoice = request.ToneOfVoice,
            SelectedModel = request.SelectedModel
        };

        userOpenaiMsgRepository.Add(userOpenaiMsg);

        var saveResult = await uow.SaveAsync();
        if (saveResult <= 0)
        {
            return ServiceResponse<UserOpenaiMsgDto>.Return500();
        }

        // Detach the entity to ensure it's fully persisted and committed to the database
        uow.Context.Entry(userOpenaiMsg).State = EntityState.Detached;

        var entityDto = mapper.Map<UserOpenaiMsgDto>(userOpenaiMsg);
        return ServiceResponse<UserOpenaiMsgDto>.ReturnResultWith201(entityDto);

    }
}



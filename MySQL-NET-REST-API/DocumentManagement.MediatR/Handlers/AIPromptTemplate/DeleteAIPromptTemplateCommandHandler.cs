using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;
public class DeleteAIPromptTemplateCommandHandler(IAIPromptTemplateRepository aIPromptTemplateRepository,
     IUnitOfWork<DocumentContext> _uow) : IRequestHandler<DeleteAIPromptTemplateCommand, ServiceResponse<bool>>
{
    public async Task<ServiceResponse<bool>> Handle(DeleteAIPromptTemplateCommand request, CancellationToken cancellationToken)
    {
        var entityExist = await aIPromptTemplateRepository.All.FirstOrDefaultAsync(c => c.Id == request.Id);
        if (entityExist == null)
        {
            return ServiceResponse<bool>.Return409("Name already exists.");
        }

        aIPromptTemplateRepository.Remove(entityExist);
        if (await _uow.SaveAsync() <= 0)
        {
            return ServiceResponse<bool>.Return500();
        }
        return ServiceResponse<bool>.ReturnResultWith201(true);
    }
}

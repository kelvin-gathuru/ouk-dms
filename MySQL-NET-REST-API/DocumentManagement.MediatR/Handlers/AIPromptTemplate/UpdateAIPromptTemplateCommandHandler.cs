using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;
public class UpdateAIPromptTemplateCommandHandler(
     IAIPromptTemplateRepository aIPromptTemplateRepository,
     IUnitOfWork<DocumentContext> _uow) : IRequestHandler<UpdateAIPromptTemplateCommand, ServiceResponse<AIPromptTemplate>>
{
    public async Task<ServiceResponse<AIPromptTemplate>> Handle(UpdateAIPromptTemplateCommand request, CancellationToken cancellationToken)
    {
        var entityExist = await aIPromptTemplateRepository.FindBy(c => c.Name == request.Name && c.Id != request.Id).FirstOrDefaultAsync();
        if (entityExist != null)
        {
            return ServiceResponse<AIPromptTemplate>.Return409("Name already exists for other template.");
        }
        var entity = await aIPromptTemplateRepository.FindBy(c => c.Id == request.Id).FirstOrDefaultAsync();
        if (entity == null)
        {
            return ServiceResponse<AIPromptTemplate>.Return404("AI Prompt Template is not found.");
        }
        entity.Id = request.Id;
        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.PromptInput = request.PromptInput;
        entity.ModifiedDate = DateTime.UtcNow;

        aIPromptTemplateRepository.Update(entity);

        if (await _uow.SaveAsync() <= 0)
        {
            return ServiceResponse<AIPromptTemplate>.Return500();
        }
        return ServiceResponse<AIPromptTemplate>.ReturnResultWith201(entity);
    }
}

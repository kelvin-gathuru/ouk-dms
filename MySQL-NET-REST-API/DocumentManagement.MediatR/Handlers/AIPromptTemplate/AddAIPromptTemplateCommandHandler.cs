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
public class AddAIPromptTemplateCommandHandler(
    IAIPromptTemplateRepository aIPromptTemplateRepository,
     IUnitOfWork<DocumentContext> _uow) : IRequestHandler<AddAIPromptTemplateCommand, ServiceResponse<AIPromptTemplate>>
{
    public async Task<ServiceResponse<AIPromptTemplate>> Handle(AddAIPromptTemplateCommand request, CancellationToken cancellationToken)
    {
        var entityExist = await aIPromptTemplateRepository.FindBy(c => c.Name == request.Name).FirstOrDefaultAsync();
        if (entityExist != null)
        {
            return ServiceResponse<AIPromptTemplate>.Return409("Name already exists.");
        }

        var aIPromptTemplate = new AIPromptTemplate
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            PromptInput = request.PromptInput,
            ModifiedDate = DateTime.UtcNow
        };
        aIPromptTemplateRepository.Add(aIPromptTemplate);
        if (await _uow.SaveAsync() <= 0)
        {
            return ServiceResponse<AIPromptTemplate>.Return500();
        }
        return ServiceResponse<AIPromptTemplate>.ReturnResultWith201(aIPromptTemplate);
    }
}

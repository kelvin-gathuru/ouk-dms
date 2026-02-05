using DocumentManagement.Data.Entities;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;
public class GetAllAIPromptTemplateCommandHandler(IAIPromptTemplateRepository aIPromptTemplateRepository) : IRequestHandler<GetAllAIPromptTemplateCommand, ServiceResponse<List<AIPromptTemplate>>>
{
    public async Task<ServiceResponse<List<AIPromptTemplate>>> Handle(GetAllAIPromptTemplateCommand request, CancellationToken cancellationToken)
    {
        var aIPromptTemplates = await aIPromptTemplateRepository.All.ToListAsync(cancellationToken: cancellationToken);
        //if (aIPromptTemplates == null || aIPromptTemplates.Count == 0)
        //{
        //    return ServiceResponse<List<AIPromptTemplate>>.Return404("AI Prompt Template not found.");
        //}
        return ServiceResponse<List<AIPromptTemplate>>.ReturnResultWith200(aIPromptTemplates);
    }
}

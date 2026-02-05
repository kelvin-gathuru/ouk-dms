using DocumentManagement.Data.Entities;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;
public class GetAIPromptTemplateCommandHandler(IAIPromptTemplateRepository aIPromptTemplateRepository) : IRequestHandler<GetAIPromptTemplateCommand, ServiceResponse<AIPromptTemplate>>
{
    public async Task<ServiceResponse<AIPromptTemplate>> Handle(GetAIPromptTemplateCommand request, CancellationToken cancellationToken)
    {
        var aIPromptTemplate = await aIPromptTemplateRepository.All.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken: cancellationToken);
        if (aIPromptTemplate == null)
        {
            return ServiceResponse<AIPromptTemplate>.Return404("AI Prompt Template not found.");
        }
        return ServiceResponse<AIPromptTemplate>.ReturnResultWith200(aIPromptTemplate);
    }
}

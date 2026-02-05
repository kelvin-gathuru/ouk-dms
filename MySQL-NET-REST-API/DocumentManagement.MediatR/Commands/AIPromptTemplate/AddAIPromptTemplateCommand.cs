using DocumentManagement.Data.Entities;
using DocumentManagement.Helper;
using MediatR;

namespace DocumentManagement.MediatR.Commands;
public class AddAIPromptTemplateCommand : IRequest<ServiceResponse<AIPromptTemplate>>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string PromptInput { get; set; }
}

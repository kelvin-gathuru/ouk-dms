using DocumentManagement.Data.Entities;
using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands;
public class UpdateAIPromptTemplateCommand : IRequest<ServiceResponse<AIPromptTemplate>>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string PromptInput { get; set; }
}

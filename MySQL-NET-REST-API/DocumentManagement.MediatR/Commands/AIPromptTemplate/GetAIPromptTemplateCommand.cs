using DocumentManagement.Data.Entities;
using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands;
public class GetAIPromptTemplateCommand : IRequest<ServiceResponse<AIPromptTemplate>>
{
    public Guid Id { get; set; }
}

using System;
using DocumentManagement.Helper;
using MediatR;

namespace DocumentManagement.MediatR.Commands;
public class AddUpdateOpenAIApiKeyCommand : IRequest<ServiceResponse<bool>>
{
    public Guid Id { get; set; }
    public string OpenAIAPIKey { get; set; }
    public string Company { get; set; }
}

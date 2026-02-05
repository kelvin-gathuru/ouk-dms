using System;
using DocumentManagement.Helper;
using MediatR;

namespace DocumentManagement.MediatR.Commands;
public class DeleteDocumentMetaTagCommand : IRequest<ServiceResponse<bool>>
{
    public Guid Id { get; set; }
}
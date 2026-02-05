using DocumentManagement.Data.Dto;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands;

public class DeleteDocumentCommand : IRequest<DocumentDto>
{
    public Guid Id { get; set; }
    public bool IsRetention { get; set; } = false;
}

using DocumentManagement.Data.Dto;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands;

public class ArchiveDocumentCommand : IRequest<DocumentDto>
{
    public Guid DocumentId { get; set; }
    public bool IsRetention { get; set; } = false;
}

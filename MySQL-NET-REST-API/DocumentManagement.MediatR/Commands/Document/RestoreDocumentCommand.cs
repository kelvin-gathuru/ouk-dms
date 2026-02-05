using System;
using DocumentManagement.Data.Dto;
using MediatR;

namespace DocumentManagement.MediatR.Commands
{
    public class RestoreDocumentCommand : IRequest<DocumentDto>
    {
        public Guid DocumentId { get; set; }
    }
}

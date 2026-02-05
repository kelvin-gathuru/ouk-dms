using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class AddDocumentTokenCommand: IRequest<string>
    {
        public Guid DocumentId { get; set; }
    }
}

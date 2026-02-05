using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class DeleteDocumentTokenCommand : IRequest<bool>
    {
        public Guid Token { get; set; }
    }
}

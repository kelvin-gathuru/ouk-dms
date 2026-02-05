using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class CheckDocumentLinkPasswordCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string Password { get; set; }
    }
}

using System;
using MediatR;

namespace DocumentManagement.MediatR.Commands
{
    public class GetDocumentPathByTokenCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public Guid Token { get; set; }
        public bool IsPublic { get; set; }
        public bool IsFileRequest { get; set; }
    }
}

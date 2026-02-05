using DocumentManagement.Data.Entities;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class GetDocumentTokenCommand : IRequest<DocumentToken>
    {
        public Guid Token { get; set; }
    }
}

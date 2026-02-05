using DocumentManagement.Data.Dto;
using MediatR;
using System;
using System.Collections.Generic;

namespace DocumentManagement.MediatR.Commands
{
    public class GetDocumentVersionCommand : IRequest<List<DocumentVersionDto>>
    {
        public Guid Id { get; set; }
    }
}

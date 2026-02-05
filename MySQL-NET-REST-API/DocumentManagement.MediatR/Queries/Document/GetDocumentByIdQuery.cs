using DocumentManagement.Data.Dto;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Queries
{
    public class GetDocumentByIdQuery : IRequest<DocumentDto>
    {
        public Guid Id { get; set; }
    }
}

using DocumentManagement.Data.Dto;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Queries
{
    public class GetDocumentShareableLinkQuery : IRequest<DocumentShareableLinkDto>
    {
        public Guid Id { get; set; }
    }
}

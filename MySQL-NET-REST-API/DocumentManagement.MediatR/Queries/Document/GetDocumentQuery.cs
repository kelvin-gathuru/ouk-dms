using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Queries
{
    public class GetDocumentQuery : IRequest<ServiceResponse<DocumentDto>>
    {
        public Guid Id { get; set; }
    }
}

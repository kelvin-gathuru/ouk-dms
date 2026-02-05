using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Queries
{
    public class GetDocumentStatusQuery : IRequest<ServiceResponse<DocumentStatusDto>>
    {
        public Guid Id { get; set; }
    }
}

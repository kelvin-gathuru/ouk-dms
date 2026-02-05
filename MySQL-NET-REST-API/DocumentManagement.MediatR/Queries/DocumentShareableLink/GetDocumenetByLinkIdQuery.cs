using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Queries
{
    public class GetDocumenetByLinkIdQuery : IRequest<ServiceResponse<DocumentShareableLinkDto>>
    {
        public Guid Id { get; set; }
    }
}

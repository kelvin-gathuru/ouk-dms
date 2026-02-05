using DocumentManagement.Data.Dto;
using MediatR;
using System;
using System.Collections.Generic;

namespace DocumentManagement.MediatR.Queries
{
    public class GetDocumentPermissionQuery : IRequest<List<DocumentPermissionDto>>
    {
        public Guid DocumentId { get; set; }
    }

}

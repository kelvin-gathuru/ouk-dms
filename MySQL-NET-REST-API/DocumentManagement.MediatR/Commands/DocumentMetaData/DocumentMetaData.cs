using DocumentManagement.Data.Dto;
using MediatR;
using System;
using System.Collections.Generic;

namespace DocumentManagement.MediatR.Commands
{
    public class GetDocumentMetaDataByIdQuery : IRequest<List<DocumentMetaDataDto>>
    {
        public Guid DocumentId { get; set; }
    }
}

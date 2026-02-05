using DocumentManagement.Data.Dto;
using MediatR;
using System;
using System.Collections.Generic;

namespace DocumentManagement.MediatR.Commands
{
    public class GetDocumentCommentByIdQuery : IRequest<List<DocumentCommentDto>>
    {
        public Guid DocumentId { get; set; }
    }
}

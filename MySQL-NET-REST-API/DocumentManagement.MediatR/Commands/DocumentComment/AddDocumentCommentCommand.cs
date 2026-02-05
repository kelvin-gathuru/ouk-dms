using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class AddDocumentCommentCommand : IRequest<ServiceResponse<DocumentCommentDto>>
    {
        public Guid DocumentId { get; set; }
        public string Comment { get; set; }
    }
}

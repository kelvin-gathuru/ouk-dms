using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class DeleteDocumentCommentCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid Id { get; set; }
    }
}

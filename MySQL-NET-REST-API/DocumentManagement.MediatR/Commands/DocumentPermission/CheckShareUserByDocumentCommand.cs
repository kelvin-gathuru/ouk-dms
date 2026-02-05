using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class CheckShareUserByDocumentCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid DocumentId { get; set; }
    }
}

using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class RestoreDocumentVersionCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid DocumentId { get; set; }
        public Guid Id { get; set; }
    }
}

using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class DeleteDocumentShareableLinkCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid Id { get; set; }
    }
}

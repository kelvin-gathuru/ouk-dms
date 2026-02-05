using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class DeleteDocumentStatusCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid Id { get; set; }
    }
}

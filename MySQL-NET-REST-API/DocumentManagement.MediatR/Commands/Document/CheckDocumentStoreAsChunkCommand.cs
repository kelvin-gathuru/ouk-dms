using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class CheckDocumentStoreAsChunkCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid DocumentId { get; set; }
    }
}

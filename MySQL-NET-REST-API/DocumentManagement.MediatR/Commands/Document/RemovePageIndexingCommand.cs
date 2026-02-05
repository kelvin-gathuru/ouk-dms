using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class RemovePageIndexingCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid DocumentId { get; set; }
    }
}

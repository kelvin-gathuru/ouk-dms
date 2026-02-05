using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class AddPageIndexingCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid DocumentId { get; set; }
    }
}


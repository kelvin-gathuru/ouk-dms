using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class DocumentSignByUserCommand: IRequest<bool>
    {
        public Guid DocumentId { get; set; }
    }
}

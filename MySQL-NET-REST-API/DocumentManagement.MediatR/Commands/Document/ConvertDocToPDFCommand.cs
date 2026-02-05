using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class ConvertDocToPDFCommand: IRequest<bool>
    {
        public Guid DocumentId { get; set; }

    }
}

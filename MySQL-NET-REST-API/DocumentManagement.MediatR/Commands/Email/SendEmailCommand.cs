using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class SendEmailCommand : IRequest<bool>
    {
        public string Subject { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public Guid DocumentId { get; set; }

    }
}

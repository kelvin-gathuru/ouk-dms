using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.CommandAndQuery
{
    public class DeleteReminderCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid Id { get; set; }
    }
}

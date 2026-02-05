using DocumentManagement.Data.Dto;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class MarkAsReadCommand : IRequest<UserNotificationDto>
    {
        public Guid Id { get; set; }
    }
}

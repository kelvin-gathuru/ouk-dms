using DocumentManagement.Data.Dto;
using MediatR;

namespace DocumentManagement.MediatR.Commands
{
    public class MarkAllAsReadCommand : IRequest<UserNotificationDto>
    {
    }
}

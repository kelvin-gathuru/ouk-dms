using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class MarkAllAsReadHandler : IRequestHandler<MarkAllAsReadCommand, UserNotificationDto>
    {
        private readonly IUserNotificationRepository _userNotificationRepository;
        public MarkAllAsReadHandler(IUserNotificationRepository userNotificationRepository)
        {
            _userNotificationRepository = userNotificationRepository;
        }
        public async Task<UserNotificationDto> Handle(MarkAllAsReadCommand request, CancellationToken cancellationToken)
        {
            await _userNotificationRepository.MarkAllAsRead();
            return new UserNotificationDto();
        }

    }
}

using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class MarkAsReadHandler : IRequestHandler<MarkAsReadCommand, UserNotificationDto>
    {
        private readonly IUserNotificationRepository _userNotificationRepository;
        public MarkAsReadHandler(IUserNotificationRepository userNotificationRepository)
        {
            _userNotificationRepository = userNotificationRepository;
        }
        public async Task<UserNotificationDto> Handle(MarkAsReadCommand request, CancellationToken cancellationToken)
        {
            await _userNotificationRepository.MarkAsRead(request.Id);
            return new UserNotificationDto();
        }

    }
}

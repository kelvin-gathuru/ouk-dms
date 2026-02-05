using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers.UserNotification
{
    public class GetAllUserNotificationQueryHandler : IRequestHandler<GetAllUserNotificationQuery, NotificationList>
    {
        private readonly IUserNotificationRepository _userNotificationRepository;
        public GetAllUserNotificationQueryHandler(
           IUserNotificationRepository userNotificationRepository
            )
        {
            _userNotificationRepository = userNotificationRepository;

        }
        public async Task<NotificationList> Handle(GetAllUserNotificationQuery request, CancellationToken cancellationToken)
        {
            return await _userNotificationRepository.GetUserNotifications(request.NotificationResource);
        }
    }
}

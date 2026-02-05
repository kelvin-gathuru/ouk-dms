using DocumentManagement.Data.Resources;
using DocumentManagement.Repository;
using MediatR;

namespace DocumentManagement.MediatR.Queries
{
    public class GetAllUserNotificationQuery : IRequest<NotificationList>
    {
        public NotificationResource NotificationResource { get; set; }
    }
}

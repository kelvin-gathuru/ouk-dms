using DocumentManagement.Data;
using DocumentManagement.Data.Resources;
using DocumentManagement.Helper;
using MediatR;

namespace DocumentManagement.MediatR.CommandAndQuery
{
    public class GetAllReminderNotificationQuery : IRequest<PagedList<ReminderScheduler>>
    {
        public ReminderResource ReminderResource { get; set; }
    }
}

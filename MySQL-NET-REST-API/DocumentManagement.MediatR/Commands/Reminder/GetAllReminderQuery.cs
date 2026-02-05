using DocumentManagement.Data.Resources;
using DocumentManagement.Repository;
using MediatR;

namespace DocumentManagement.MediatR.CommandAndQuery
{
    public class GetAllReminderQuery : IRequest<ReminderList>
    {
        public ReminderResource ReminderResource { get; set; }
    }
}

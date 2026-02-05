using DocumentManagement.Data.Resources;
using DocumentManagement.Repository;
using MediatR;

namespace DocumentManagement.MediatR.Commands
{
    public class GetReminderForLoginUserCommand : IRequest<ReminderList>
    {
        public ReminderResource ReminderResource { get; set; }
    }
}

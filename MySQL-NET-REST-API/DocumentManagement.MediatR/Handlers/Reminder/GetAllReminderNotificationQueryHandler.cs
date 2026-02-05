using DocumentManagement.Data;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.CommandAndQuery;
using DocumentManagement.Repository;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetAllReminderNotificationQueryHandler : IRequestHandler<GetAllReminderNotificationQuery, PagedList<ReminderScheduler>>
    {
        private readonly IReminderSchedulerRepository _reminderSchedulerRepository;

        public GetAllReminderNotificationQueryHandler(
            IReminderSchedulerRepository reminderSchedulerRepository
           )
        {
            _reminderSchedulerRepository = reminderSchedulerRepository;
        }

        public async Task<PagedList<ReminderScheduler>> Handle(GetAllReminderNotificationQuery request, CancellationToken cancellationToken)
        {
            return await _reminderSchedulerRepository.GetReminders(request.ReminderResource);
        }
    }
}

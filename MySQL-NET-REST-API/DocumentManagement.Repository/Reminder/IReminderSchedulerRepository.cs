using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Data;
using DocumentManagement.Data.Resources;
using DocumentManagement.Helper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DocumentManagement.Repository
{
    public interface IReminderSchedulerRepository : IGenericRepository<ReminderScheduler>
    {
        Task<bool> AddMultiReminder(List<Reminder> reminders);
        Task<PagedList<ReminderScheduler>> GetReminders(ReminderResource reminderResource);
        Task<bool> MarkAsRead();
    }
}

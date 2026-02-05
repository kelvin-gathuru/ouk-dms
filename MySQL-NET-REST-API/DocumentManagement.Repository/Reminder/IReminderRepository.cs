using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Data;
using DocumentManagement.Data.Resources;
using System.Threading.Tasks;

namespace DocumentManagement.Repository
{
    public interface IReminderRepository : IGenericRepository<Reminder>
    {
        Task<ReminderList> GetReminders(ReminderResource reminderResource);

        Task<ReminderList> GetRemindersForLoginUser(ReminderResource reminderResource);
    }
}

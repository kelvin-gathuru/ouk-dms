using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Domain;

namespace DocumentManagement.Repository
{
    public class DailyReminderRepository : GenericRepository<DailyReminder, DocumentContext>,
        IDailyReminderRepository
    {
        public DailyReminderRepository(
            IUnitOfWork<DocumentContext> uow) : base(uow)
        {
        }
    }
}

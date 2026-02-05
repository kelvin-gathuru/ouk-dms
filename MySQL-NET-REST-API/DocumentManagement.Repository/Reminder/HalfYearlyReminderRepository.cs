using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Domain;

namespace DocumentManagement.Repository
{
    public class HalfYearlyReminderRepository : GenericRepository<HalfYearlyReminder, DocumentContext>,
        IHalfYearlyReminderRepository
    {
        public HalfYearlyReminderRepository(
            IUnitOfWork<DocumentContext> uow) : base(uow)
        {
        }
    }
}

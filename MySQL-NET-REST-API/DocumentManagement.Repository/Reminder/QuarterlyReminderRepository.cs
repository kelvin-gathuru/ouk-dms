using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Domain;

namespace DocumentManagement.Repository
{
    public class QuarterlyReminderRepository : GenericRepository<QuarterlyReminder, DocumentContext>,
    IQuarterlyReminderRepository
    {
        public QuarterlyReminderRepository(
            IUnitOfWork<DocumentContext> uow) : base(uow)
        {
        }
    }
}

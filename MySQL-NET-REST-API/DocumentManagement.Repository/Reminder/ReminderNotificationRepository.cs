using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Domain;

namespace DocumentManagement.Repository
{
    public class ReminderNotificationRepository
        : GenericRepository<ReminderNotification, DocumentContext>, IReminderNotificationRepository
    {
        public ReminderNotificationRepository(
            IUnitOfWork<DocumentContext> uow
            ) : base(uow)
        {
        }
    }
}

using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Domain;

namespace DocumentManagement.Repository
{
    public class EmailSMTPSettingRepository : GenericRepository<EmailSMTPSetting, DocumentContext>,
           IEmailSMTPSettingRepository
    {
        public EmailSMTPSettingRepository(
            IUnitOfWork<DocumentContext> uow
            ) : base(uow)
        {
        }
    }
}

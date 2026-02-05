using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;


namespace DocumentManagement.Repository
{
    public class WorkflowInstanceEmailSenderRepository : GenericRepository<WorkflowInstanceEmailSender, DocumentContext>, IWorkflowInstanceEmailSenderRepository
    {
        public WorkflowInstanceEmailSenderRepository(IUnitOfWork<DocumentContext> uow) : base(uow)
        {
        }
    }
}


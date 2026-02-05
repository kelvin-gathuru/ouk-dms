using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Domain;

namespace DocumentManagement.Repository
{
    public class WorkflowStepUserRepository : GenericRepository<WorkflowStepUser, DocumentContext>, IWorkflowStepUserRepository
    {
        public WorkflowStepUserRepository(IUnitOfWork<DocumentContext> uow) : base(uow)
        {

        }
    }
}

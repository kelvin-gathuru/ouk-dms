using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Domain;

namespace DocumentManagement.Repository
{
    public class WorkflowRepository : GenericRepository<Workflow, DocumentContext>, IWorkflowRepository
    {
        public WorkflowRepository(IUnitOfWork<DocumentContext> uow) : base(uow)
        {
        }
    }
}


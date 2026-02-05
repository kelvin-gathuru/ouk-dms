using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Domain;

namespace DocumentManagement.Repository
{
    public class WorkflowStepInstanceRepository : GenericRepository<WorkflowStepInstance, DocumentContext>, IWorkflowStepInstanceRepository
    {
        public WorkflowStepInstanceRepository(IUnitOfWork<DocumentContext> uow) : base(uow)
        {
        }
    }
}
using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Domain;

namespace DocumentManagement.Repository
{
    public class WorkflowStepRepository : GenericRepository<WorkflowStep, DocumentContext>, IWorkflowStepRepository
    {
        public WorkflowStepRepository(IUnitOfWork<DocumentContext> uow) : base(uow)
        {
        }
    }
}
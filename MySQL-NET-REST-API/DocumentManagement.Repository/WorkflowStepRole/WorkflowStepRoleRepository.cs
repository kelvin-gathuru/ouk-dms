using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Domain;

namespace DocumentManagement.Repository
{
    public class WorkflowStepRoleRepository : GenericRepository<WorkflowStepRole, DocumentContext>, IWorkflowStepRoleRepository
    {
        public WorkflowStepRoleRepository(IUnitOfWork<DocumentContext> uow) : base(uow)
        {
        }
    }
}
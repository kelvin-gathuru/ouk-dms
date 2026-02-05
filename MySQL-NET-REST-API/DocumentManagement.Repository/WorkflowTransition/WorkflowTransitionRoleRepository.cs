using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;

namespace DocumentManagement.Repository;
public class WorkflowTransitionRoleRepository : GenericRepository<WorkflowTransitionRole, DocumentContext>, IWorkflowTransitionRoleRepository
{
    public WorkflowTransitionRoleRepository(IUnitOfWork<DocumentContext> uow) : base(uow)
    {

    }
}


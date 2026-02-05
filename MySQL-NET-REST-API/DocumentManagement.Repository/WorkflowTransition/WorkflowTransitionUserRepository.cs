using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;

namespace DocumentManagement.Repository;
public class WorkflowTransitionUserRepository : GenericRepository<WorkflowTransitionUser, DocumentContext>, IWorkflowTransitionUserRepository
{
    public WorkflowTransitionUserRepository(IUnitOfWork<DocumentContext> uow) : base(uow)
    {

    }
}

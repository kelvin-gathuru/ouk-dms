using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Domain;

namespace DocumentManagement.Repository
{
    public class DocumentUserPermissionRepository : GenericRepository<DocumentUserPermission, DocumentContext>,
       IDocumentUserPermissionRepository
    {
        public DocumentUserPermissionRepository(
            IUnitOfWork<DocumentContext> uow
            ) : base(uow)
        {
        }
    }
}

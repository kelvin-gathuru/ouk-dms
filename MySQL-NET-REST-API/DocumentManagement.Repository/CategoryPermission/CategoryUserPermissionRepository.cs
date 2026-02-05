using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;

namespace DocumentManagement.Repository
{
    public class CategoryUserPermissionRepository : GenericRepository<CategoryUserPermission, DocumentContext>, ICategoryUserPermissionRepository
    {
        public CategoryUserPermissionRepository(
                       IUnitOfWork<DocumentContext> uow
                       ) : base(uow)
        {

        }
    }
}

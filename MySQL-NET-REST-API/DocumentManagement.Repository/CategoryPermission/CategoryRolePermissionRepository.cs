using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;


namespace DocumentManagement.Repository
{
    public class CategoryRolePermissionRepository : GenericRepository<CategoryRolePermission, DocumentContext>, ICategoryRolePermissionRepository
    {
        public CategoryRolePermissionRepository(
                                  IUnitOfWork<DocumentContext> uow
                                  ) : base(uow)
        {

        }
    }
}

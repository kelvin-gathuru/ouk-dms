using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Domain;

namespace DocumentManagement.Repository
{
    public class AllowFileExtensionRepository : GenericRepository<AllowFileExtension, DocumentContext>, IAllowFileExtensionRepository
    {
        public AllowFileExtensionRepository(IUnitOfWork<DocumentContext> uow) : base(uow)
        {
        }
    }
}

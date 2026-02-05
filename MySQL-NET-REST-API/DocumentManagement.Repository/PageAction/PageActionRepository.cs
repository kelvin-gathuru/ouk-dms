using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;

namespace DocumentManagement.Repository;
public class PageActionRepository : GenericRepository<PageAction, DocumentContext>,
      IPageActionRepository
{
    public PageActionRepository(
        IUnitOfWork<DocumentContext> uow
        ) : base(uow)
    {
    }
}


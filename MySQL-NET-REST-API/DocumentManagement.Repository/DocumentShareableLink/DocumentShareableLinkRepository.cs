using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Domain;

namespace DocumentManagement.Repository
{
    public class DocumentShareableLinkRepository : GenericRepository<DocumentShareableLink, DocumentContext>, IDocumentShareableLinkRepository
    {
        public DocumentShareableLinkRepository(IUnitOfWork<DocumentContext> uow) : base(uow)
        {
        }
    }
}

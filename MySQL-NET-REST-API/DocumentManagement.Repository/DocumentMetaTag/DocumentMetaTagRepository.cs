using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Domain;

namespace DocumentManagement.Repository;
public class DocumentMetaTagRepository : GenericRepository<DocumentMetaTag, DocumentContext>, IDocumentMetaTagRepository
{
    public DocumentMetaTagRepository(IUnitOfWork<DocumentContext> uow) : base(uow)
    {
    }
}

using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Domain;

namespace DocumentManagement.Repository
{
    public class DocumentMetaDataRepository : GenericRepository<DocumentMetaData, DocumentContext>, IDocumentMetaDataRepository
    {
        public DocumentMetaDataRepository(IUnitOfWork<DocumentContext> uow) : base(uow)
        {
        }
    }
}

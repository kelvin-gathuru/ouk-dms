
using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Domain;

namespace DocumentManagement.Repository
{
    public class DocumentChunkRepository : GenericRepository<DocumentChunk, DocumentContext>,
           IDocumentChunkRepository
    {
        public DocumentChunkRepository(
            IUnitOfWork<DocumentContext> uow
            ) : base(uow)
        {

        }
    }

}
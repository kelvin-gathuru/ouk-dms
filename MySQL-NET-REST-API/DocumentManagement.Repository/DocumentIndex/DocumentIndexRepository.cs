using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;

namespace DocumentManagement.Repository
{
    public class DocumentIndexRepository : GenericRepository<DocumentIndex, DocumentContext>,
           IDocumentIndexRepository
    {
        public DocumentIndexRepository(
            IUnitOfWork<DocumentContext> uow
            ) : base(uow)
        {

        }
    }
}
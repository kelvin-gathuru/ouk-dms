using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;

namespace DocumentManagement.Repository
{
    public class DocumentTokenRepository : GenericRepository<DocumentToken, DocumentContext>,
            IDocumentTokenRepository
    {
        public DocumentTokenRepository(
            IUnitOfWork<DocumentContext> uow
            ) : base(uow)
        {
        }
    }
}

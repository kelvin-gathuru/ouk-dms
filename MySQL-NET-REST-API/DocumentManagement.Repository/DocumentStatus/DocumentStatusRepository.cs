using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;

namespace DocumentManagement.Repository
{
    public class DocumentStatusRepository : GenericRepository<DocumentStatus, DocumentContext>, IDocumentStatusRepository
    {
        public DocumentStatusRepository(IUnitOfWork<DocumentContext> uow) : base(uow)
        {
        }
    }
}

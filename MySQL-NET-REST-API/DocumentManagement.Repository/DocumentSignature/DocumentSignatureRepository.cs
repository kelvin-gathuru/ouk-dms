using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Domain;

namespace DocumentManagement.Repository
{
    public class DocumentSignatureRepository : GenericRepository<DocumentSignature, DocumentContext>, IDocumentSignatureRepository
    {
        public DocumentSignatureRepository(IUnitOfWork<DocumentContext> uow) : base(uow)
        {
        }
    }
}
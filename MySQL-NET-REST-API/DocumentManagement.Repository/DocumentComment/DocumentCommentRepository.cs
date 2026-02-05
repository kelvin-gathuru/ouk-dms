using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Domain;


namespace DocumentManagement.Repository
{
    public class DocumentCommentRepository : GenericRepository<DocumentComment, DocumentContext>, IDocumentCommentRepository
    {
        public DocumentCommentRepository(IUnitOfWork<DocumentContext> uow) : base(uow)
        {
        }
    }
}

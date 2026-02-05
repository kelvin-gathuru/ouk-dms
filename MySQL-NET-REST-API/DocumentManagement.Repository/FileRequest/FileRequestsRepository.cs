using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;

namespace DocumentManagement.Repository
{
    public class FileRequestsRepository : GenericRepository<FileRequest, DocumentContext>, IFileRequestsRepository
    {
        public FileRequestsRepository(IUnitOfWork<DocumentContext> uow) : base(uow)
        {
        }
    }
}

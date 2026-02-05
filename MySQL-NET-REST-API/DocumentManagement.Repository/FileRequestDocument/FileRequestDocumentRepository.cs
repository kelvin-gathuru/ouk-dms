using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;

namespace DocumentManagement.Repository
{
    public class FileRequestDocumentRepository : GenericRepository<FileRequestDocument, DocumentContext>, IFileRequestDocumentRepository
    {
        public FileRequestDocumentRepository(IUnitOfWork<DocumentContext> uow) : base(uow)
        {
        }
    }
}


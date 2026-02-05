using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;

namespace DocumentManagement.Repository;
public class ArchiveRetentionRepository(IUnitOfWork<DocumentContext> uow) : GenericRepository<ArchiveRetention, DocumentContext>(uow), IArchiveRetentionRepository
{
}

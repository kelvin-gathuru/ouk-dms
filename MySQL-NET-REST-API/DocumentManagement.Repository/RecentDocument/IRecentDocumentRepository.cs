using System.Threading.Tasks;
using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Data.Entities;
using DocumentManagement.Data.Resources;

namespace DocumentManagement.Repository;
public interface IRecentDocumentRepository : IGenericRepository<DocumentAuditTrail>
{
    Task<RecentDocumentList> GetRecentDocuments(DocumentResource documentResource);
}
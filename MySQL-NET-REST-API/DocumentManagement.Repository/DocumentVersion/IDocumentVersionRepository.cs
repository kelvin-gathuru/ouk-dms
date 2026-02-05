using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Data;
using System.Threading.Tasks;
using System;

namespace DocumentManagement.Repository
{
    public interface IDocumentVersionRepository : IGenericRepository<DocumentVersion>
    {
        Task<int> GetDocumentVersionCount(Guid documentId);
        Task UpdateCurrentVersion(Guid documentId);
        void UpdateDetachedAndAttached(DocumentVersion entity);
    }
}

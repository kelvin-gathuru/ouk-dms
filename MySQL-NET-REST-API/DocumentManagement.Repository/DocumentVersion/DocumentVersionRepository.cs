using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagement.Repository
{
    public class DocumentVersionRepository : GenericRepository<DocumentVersion, DocumentContext>, IDocumentVersionRepository
    {
        public DocumentVersionRepository(IUnitOfWork<DocumentContext> uow) : base(uow)
        {
        }

        public async Task<int> GetDocumentVersionCount(Guid documentId)
        {
            return await All.Where(c => c.DocumentId == documentId).CountAsync();
        }

        public async Task UpdateCurrentVersion (Guid documentId)
        {
            var version= await All.Where(c => c.DocumentId == documentId && c.IsCurrentVersion == true).FirstOrDefaultAsync();
            version.IsCurrentVersion = false;
            Update(version);
        }

        public void UpdateDetachedAndAttached(DocumentVersion entity)
        {
            // Check if the entity is already being tracked
            var existingEntity = _uow.Context.DocumentVersions.Local
                .FirstOrDefault(d => d.Id == entity.Id);
            if (existingEntity != null)
            {
                // Detach the existing entity to avoid tracking conflicts
                _uow.Context.Entry(existingEntity).State = EntityState.Detached;
            }
            // Now attach and mark as modified
            _uow.Context.Attach(entity);
            _uow.Context.Entry(entity).State = EntityState.Modified;
        }
    }
}
using System;
using System.Linq;
using System.Threading.Tasks;
using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Data.Resources;
using DocumentManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.Repository;
public class RecentDocumentRepository : GenericRepository<DocumentAuditTrail, DocumentContext>, IRecentDocumentRepository
{
    private readonly IPropertyMappingService _propertyMappingService;
    private readonly UserInfoToken _userInfoToken;

    public RecentDocumentRepository(IPropertyMappingService propertyMappingService, IUnitOfWork<DocumentContext> uow, UserInfoToken userInfoToken) : base(uow)
    {
        _propertyMappingService = propertyMappingService;
        _uow = uow;
        _userInfoToken = userInfoToken;
    }

    public async Task<RecentDocumentList> GetRecentDocuments(DocumentResource documentResource)
    {
        var documentOperations = new[]
        {
            DocumentOperation.Read,DocumentOperation.Created,DocumentOperation.Modified,
            DocumentOperation.Archived,DocumentOperation.Restored,DocumentOperation.Download,
            DocumentOperation.Added_Version,DocumentOperation.Restored_Version,DocumentOperation.Download,
            DocumentOperation.Added_Signature,DocumentOperation.Deleted,DocumentOperation.Added_Folder,
            DocumentOperation.Edited_Folder,DocumentOperation.Archived_Folder,DocumentOperation.Restored_Folder,
            DocumentOperation.Deleted_Folder
        };

        var collectionBeforePaging = AllIncluding(c => c.CreatedByUser, d => d.Document, c => c.Document.Category, c => c.Category)
            .Where(c => documentOperations.Contains(c.OperationName) && c.CreatedBy == _userInfoToken.Id);

        collectionBeforePaging =
            collectionBeforePaging
            .IgnoreQueryFilters()
            .ApplySort(documentResource.OrderBy,
            _propertyMappingService.GetPropertyMapping<DocumentAuditTrailDto, DocumentAuditTrail>());
        if (!string.IsNullOrWhiteSpace(documentResource.Name))
        {
            collectionBeforePaging = collectionBeforePaging
                .Where(c => EF.Functions.Like(c.Document.Name, $"%{documentResource.Name}%"));
        }
        if (!string.IsNullOrWhiteSpace(documentResource.DocumentNumber))
        {
            collectionBeforePaging = collectionBeforePaging
                .Where(c => EF.Functions.Like(c.Document.DocumentNumber, $"{documentResource.DocumentNumber}%"));
        }
        if (!string.IsNullOrWhiteSpace(documentResource.Id))
        {
            collectionBeforePaging = collectionBeforePaging
                .Where(c => c.DocumentId == Guid.Parse(documentResource.Id));
        }
        if (!string.IsNullOrWhiteSpace(documentResource.CategoryId))
        {
            collectionBeforePaging = collectionBeforePaging
                .Where(c => c.CategoryId == Guid.Parse(documentResource.CategoryId));
        }
        if (!string.IsNullOrWhiteSpace(documentResource.Operation))
        {
            collectionBeforePaging = collectionBeforePaging
                .Where(c => c.OperationName == Enum.Parse<DocumentOperation>(documentResource.Operation));
        }
        var recentDocumentList = new RecentDocumentList();
        return await recentDocumentList.Create(
            collectionBeforePaging,
            documentResource.Skip,
            documentResource.PageSize
        );
    }
}

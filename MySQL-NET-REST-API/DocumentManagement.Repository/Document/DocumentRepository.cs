using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Data.Resources;
using DocumentManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.Repository;

public class DocumentRepository : GenericRepository<Document, DocumentContext>,
      IDocumentRepository
{
    private readonly IPropertyMappingService _propertyMappingService;
    readonly IUserRepository _userRepository;
    private readonly UserInfoToken _userInfoToken;
    private readonly IMapper _mapper;
    private readonly IDocumentRolePermissionRepository _documentRolePermissionRepository;
    private readonly IDocumentUserPermissionRepository _documentUserPermissionRepository;
    private readonly ICategoryUserPermissionRepository _categoryUserPermissionRepository;
    private readonly ICategoryRolePermissionRepository _categoryRolePermissionRepository;
    private readonly IDocumentMetaTagRepository _documentMetaTagRepository;
    private readonly ICategoryRepository _categoryRepository;
    public DocumentRepository(
        IUnitOfWork<DocumentContext> uow,
        IPropertyMappingService propertyMappingService,
        IUserRepository userRepository,
        UserInfoToken userInfoToken,
        IMapper mapper,
        IDocumentRolePermissionRepository documentRolePermissionRepository,
        IDocumentUserPermissionRepository documentUserPermissionRepository,
        ICategoryRolePermissionRepository categoryRolePermissionRepository,
        ICategoryUserPermissionRepository categoryUserPermissionRepository,
        ICategoryRepository categoryRepository
        ) : base(uow)
    {
        _propertyMappingService = propertyMappingService;
        _userRepository = userRepository;
        _userInfoToken = userInfoToken;
        _mapper = mapper;
        _documentRolePermissionRepository = documentRolePermissionRepository;
        _documentUserPermissionRepository = documentUserPermissionRepository;
        _categoryUserPermissionRepository = categoryUserPermissionRepository;
        _categoryRolePermissionRepository = categoryRolePermissionRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<DocumentList> GetDocuments(DocumentResource documentResource)
    {

        var collectionBeforePaging = AllIncluding(
            c => c.User,
            cs => cs.Category,
            cs => cs.DocumentStatus,
            cs => cs.StorageSetting,
            cs => cs.Client,
            cs => cs.SignBy,
            cs => cs.DocumentMetaDatas,
            cs => cs.DocumentComments);
        collectionBeforePaging = collectionBeforePaging.Include(c => c.WorkflowInstances).ThenInclude(c => c.Workflow)

         .Where(c => !c.Category.IsDeleted && !c.Category.IsArchive && !c.IsArchive);
        collectionBeforePaging =
           collectionBeforePaging.ApplySort(documentResource.OrderBy,
           _propertyMappingService.GetPropertyMapping<DocumentDto, Document>());

        collectionBeforePaging = collectionBeforePaging.Where(c => c.IsArchive == documentResource.IsArchive && c.IsAllChunkUploaded);



        if (!string.IsNullOrWhiteSpace(documentResource.Name))
        {
            collectionBeforePaging = collectionBeforePaging
                .Where(c => EF.Functions.Like(c.Name, $"%{documentResource.Name}%") || EF.Functions.Like(c.Description, $"%{documentResource.Name}%"));
        }
        if (!string.IsNullOrWhiteSpace(documentResource.DocumentNumber))
        {
            collectionBeforePaging = collectionBeforePaging
                .Where(c => EF.Functions.Like(c.DocumentNumber, $"{documentResource.DocumentNumber}%"));
        }

        if (!string.IsNullOrWhiteSpace(documentResource.CategoryId))
        {
            collectionBeforePaging = collectionBeforePaging
                .Where(c => c.CategoryId == Guid.Parse(documentResource.CategoryId));
        }

        if (!string.IsNullOrWhiteSpace(documentResource.DocumentStatusId))
        {
            collectionBeforePaging = collectionBeforePaging.
                Where(c => c.DocumentStatusId == Guid.Parse(documentResource.DocumentStatusId));
        }
        if (!string.IsNullOrWhiteSpace(documentResource.ClientId))
        {
            collectionBeforePaging = collectionBeforePaging.
                Where(c => c.ClientId == Guid.Parse(documentResource.ClientId));
        }
        if (!string.IsNullOrWhiteSpace(documentResource.StorageSettingId))
        {
            collectionBeforePaging = collectionBeforePaging.
                Where(c => c.StorageSettingId == Guid.Parse(documentResource.StorageSettingId));
        }

        if (!string.IsNullOrWhiteSpace(documentResource.MetaTagsTypeId))
        {
            if (documentResource.StartDate.HasValue && documentResource.EndDate.HasValue)
            {
                var startDate = documentResource.StartDate.Value;
                var endDate = documentResource.EndDate.Value;

                DateTime minDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0);
                DateTime maxDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

                var tagId = Guid.Parse(documentResource.MetaTagsTypeId);

                collectionBeforePaging = collectionBeforePaging
                            .Where(c => c.DocumentMetaDatas.Any(dm =>
                                dm.DocumentMetaTagId == tagId &&
                                dm.MetaTagDate >= minDate &&
                                dm.MetaTagDate <= maxDate));

            }
            else if (!string.IsNullOrWhiteSpace(documentResource.MetaTags))
            {

                collectionBeforePaging = collectionBeforePaging
                .Where(c => c.DocumentMetaDatas.Any(c => c.Metatag.ToLower() == documentResource.MetaTags.ToLower() && c.DocumentMetaTagId == Guid.Parse(documentResource.MetaTagsTypeId)));

            }

        }

        if (documentResource.CreateDate.HasValue)
        {
            //string[] formats = documentResource.CreateDateString.Split("/");

            //documentResource.CreateDate = new DateTime(int.Parse(formats[2]), int.Parse(formats[0]), int.Parse(formats[1]));
            var minDate = DateTime.SpecifyKind(documentResource.CreateDate.Value, DateTimeKind.Utc);
            var maxDate = minDate.AddDays(1).AddTicks(-1);

            collectionBeforePaging = collectionBeforePaging
                .Where(c => c.CreatedDate >= minDate && c.CreatedDate <= maxDate);


        }

        var documentList = new DocumentList();
        return await documentList.Create(
            collectionBeforePaging,
            documentResource.Skip,
            documentResource.PageSize
            );
    }

    public async Task<DocumentList> GetArchiveDocuments(DocumentResource documentResource)
    {
        var collectionBeforePaging = AllIncluding(c => c.User,
            cs => cs.Category,
            cs => cs.DocumentStatus,
            cs => cs.StorageSetting,
            cs => cs.Client,
            cs => cs.SignBy,
            cs => cs.DocumentComments,
            cs => cs.ArchiveBy);
        collectionBeforePaging = collectionBeforePaging.Include(c => c.WorkflowInstances)
            .ThenInclude(c => c.Workflow)
            .Where(c => !c.Category.IsDeleted && (c.Category.IsArchive || c.IsArchive));

        // Check if the user is not a super admin  
        if (!_userInfoToken.IsSuperAdmin)
        {
            collectionBeforePaging = collectionBeforePaging.Where(c => c.CreatedBy == _userInfoToken.Id || c.ArchiveById == _userInfoToken.Id);
        }

        collectionBeforePaging = collectionBeforePaging.ApplySort(documentResource.OrderBy,
            _propertyMappingService.GetPropertyMapping<DocumentDto, Document>());

        collectionBeforePaging = collectionBeforePaging.Where(c => c.IsAllChunkUploaded);

        if (!string.IsNullOrWhiteSpace(documentResource.Name))
        {
            collectionBeforePaging = collectionBeforePaging
                .Where(c => EF.Functions.Like(c.Name, $"%{documentResource.Name}%") || EF.Functions.Like(c.Description, $"%{documentResource.Name}%"));
        }
        if (!string.IsNullOrWhiteSpace(documentResource.DocumentNumber))
        {
            collectionBeforePaging = collectionBeforePaging
                .Where(c => EF.Functions.Like(c.DocumentNumber, $"{documentResource.DocumentNumber}%"));
        }

        if (!string.IsNullOrWhiteSpace(documentResource.MetaTagsTypeId))
        {
            if (documentResource.StartDate.HasValue && documentResource.EndDate.HasValue)
            {
                var startDate = documentResource.StartDate.Value;
                var endDate = documentResource.EndDate.Value;

                DateTime minDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0);
                DateTime maxDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

                var tagId = Guid.Parse(documentResource.MetaTagsTypeId);

                collectionBeforePaging = collectionBeforePaging
                            .Where(c => c.DocumentMetaDatas.Any(dm =>
                                dm.DocumentMetaTagId == tagId &&
                                dm.MetaTagDate >= minDate &&
                                dm.MetaTagDate <= maxDate));

            }
            else if (!string.IsNullOrWhiteSpace(documentResource.MetaTags))
            {

                collectionBeforePaging = collectionBeforePaging
                .Where(c => c.DocumentMetaDatas.Any(c => c.Metatag.ToLower() == documentResource.MetaTags.ToLower() && c.DocumentMetaTagId == Guid.Parse(documentResource.MetaTagsTypeId)));

            }

        }

        if (!string.IsNullOrWhiteSpace(documentResource.CategoryId))
        {
            collectionBeforePaging = collectionBeforePaging
                .Where(c => c.CategoryId == Guid.Parse(documentResource.CategoryId));
        }

        if (!string.IsNullOrWhiteSpace(documentResource.DocumentStatusId))
        {
            collectionBeforePaging = collectionBeforePaging.
                Where(c => c.DocumentStatusId == Guid.Parse(documentResource.DocumentStatusId));
        }
        if (!string.IsNullOrWhiteSpace(documentResource.ClientId))
        {
            collectionBeforePaging = collectionBeforePaging.
                Where(c => c.ClientId == Guid.Parse(documentResource.ClientId));
        }
        if (!string.IsNullOrWhiteSpace(documentResource.StorageSettingId))
        {
            collectionBeforePaging = collectionBeforePaging.
                Where(c => c.StorageSettingId == Guid.Parse(documentResource.StorageSettingId));
        }

        if (documentResource.CreateDate.HasValue)
        {
            //string[] formats = documentResource.CreateDateString.Split("/");

            //documentResource.CreateDate = new DateTime(int.Parse(formats[2]), int.Parse(formats[0]), int.Parse(formats[1]));
            var minDate = DateTime.SpecifyKind(documentResource.CreateDate.Value, DateTimeKind.Utc);
            var maxDate = minDate.AddDays(1).AddTicks(-1);

            collectionBeforePaging = collectionBeforePaging
                .Where(c => c.CreatedDate >= minDate && c.CreatedDate <= maxDate);


        }

        var documentList = new DocumentList();
        return await documentList.Create(
            collectionBeforePaging,
            documentResource.Skip,
            documentResource.PageSize
            );
    }

    public async Task<string> GenerateDocumentNumberAsync()
    {
        int year = DateTime.UtcNow.Year;
        int nextNumber = await All.Where(d => d.DocumentNumber.StartsWith($"{year}-"))
            .CountAsync() + 1;
        return $"{year}-{nextNumber:D5}"; // Example: 2024-00001
    }

    public async Task<DocumentList> GetDocumentsLibrary(string email, DocumentResource documentResource)
    {

        var today = DateTime.UtcNow;
        var user = await _userRepository.AllIncluding(c => c.UserRoles).FirstOrDefaultAsync(c => c.Id == _userInfoToken.Id);
        var userRoles = user.UserRoles.Select(c => c.RoleId).ToList();


        var collectionBeforePaging = AllIncluding(c => c.User,
            c => c.Category,
            c => c.DocumentRolePermissions,
            c => c.DocumentUserPermissions,
            c => c.DocumentStatus,
            c => c.StorageSetting,
            cs => cs.SignBy,
            cs => cs.DocumentComments)
            .Include(c => c.WorkflowInstances)
                .ThenInclude(c => c.Workflow)
                .AsQueryable();
        collectionBeforePaging = collectionBeforePaging.Where(c => !c.Category.IsDeleted && !c.Category.IsArchive && !c.IsArchive);

        collectionBeforePaging = collectionBeforePaging
     .ApplySort(documentResource.OrderBy, _propertyMappingService.GetPropertyMapping<DocumentDto, Document>());


        if (!string.IsNullOrWhiteSpace(documentResource.CategoryId))
        {
            collectionBeforePaging = collectionBeforePaging
                .Where(c => c.CategoryId == Guid.Parse(documentResource.CategoryId));
        }

        collectionBeforePaging = collectionBeforePaging.Where(d =>


        d.DocumentUserPermissions.Any(c => c.UserId == user.Id && (!c.IsTimeBound || (c.IsTimeBound && c.StartDate < today && c.EndDate > today)))

        || d.DocumentRolePermissions.Any(c => userRoles.Contains(c.RoleId)
                                          && (!c.IsTimeBound || (c.IsTimeBound && c.StartDate < today && c.EndDate > today)))

        ||

        d.Category.CategoryUserPermissions.Any(c => c.UserId == user.Id &&
                   (!c.IsTimeBound || (c.StartDate < today && c.EndDate > today)))
        ||
                    d.Category.CategoryRolePermissions.Any(c => userRoles.Contains(c.RoleId) &&
                    (!c.IsTimeBound || (c.StartDate < today && c.EndDate > today)))


                                          );





        collectionBeforePaging = collectionBeforePaging
               .Where(c => c.IsAllChunkUploaded);
        if (!string.IsNullOrWhiteSpace(documentResource.Name))
        {
            collectionBeforePaging = collectionBeforePaging
                .Where(c => EF.Functions.Like(c.Name, $"%{documentResource.Name}%") || EF.Functions.Like(c.Description, $"%{documentResource.Name}%"));
        }

        if (!string.IsNullOrWhiteSpace(documentResource.DocumentNumber))
        {
            collectionBeforePaging = collectionBeforePaging
                .Where(c => EF.Functions.Like(c.DocumentNumber, $"{documentResource.DocumentNumber}%"));
        }

        if (!string.IsNullOrWhiteSpace(documentResource.MetaTagsTypeId))
        {
            if (documentResource.StartDate.HasValue && documentResource.EndDate.HasValue)
            {
                var startDate = documentResource.StartDate.Value;
                var endDate = documentResource.EndDate.Value;

                DateTime minDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0);
                DateTime maxDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

                var tagId = Guid.Parse(documentResource.MetaTagsTypeId);

                collectionBeforePaging = collectionBeforePaging
                            .Where(c => c.DocumentMetaDatas.Any(dm =>
                                dm.DocumentMetaTagId == tagId &&
                                dm.MetaTagDate >= minDate &&
                                dm.MetaTagDate <= maxDate));

            }
            else if (!string.IsNullOrWhiteSpace(documentResource.MetaTags))
            {

                collectionBeforePaging = collectionBeforePaging
                .Where(c => c.DocumentMetaDatas.Any(c => c.Metatag.ToLower() == documentResource.MetaTags.ToLower() && c.DocumentMetaTagId == Guid.Parse(documentResource.MetaTagsTypeId)));

            }

        }

        if (!string.IsNullOrWhiteSpace(documentResource.DocumentStatusId))
        {
            collectionBeforePaging = collectionBeforePaging.
                Where(c => c.DocumentStatusId == Guid.Parse(documentResource.DocumentStatusId));
        }
        if (!string.IsNullOrWhiteSpace(documentResource.ClientId))
        {
            collectionBeforePaging = collectionBeforePaging.
                Where(c => c.ClientId == Guid.Parse(documentResource.ClientId));
        }

        if (!string.IsNullOrWhiteSpace(documentResource.StorageSettingId))
        {
            collectionBeforePaging = collectionBeforePaging.
                Where(c => c.StorageSettingId == Guid.Parse(documentResource.StorageSettingId));
        }
        var documentList = new DocumentList();
        return await documentList.CreateDocumentLibrary(
            collectionBeforePaging,
            documentResource.Skip,
            documentResource.PageSize
            );
    }

    public async Task<DocumentDto> GetDocumentById(Guid Id)
    {
        var document = await All.Where(d => d.Id == Id).FirstOrDefaultAsync();
        if (document == null)
        {
            return null;
        }

        var result = _mapper.Map<DocumentDto>(document);
        return result;
    }
    public async Task<DocumentDto> GetDocumentSharedUsersRolesById(Guid Id)
    {
        var today = DateTime.UtcNow;
        var user = await _userRepository.AllIncluding(c => c.UserRoles).FirstOrDefaultAsync(c => c.Id == _userInfoToken.Id);
        var userRoles = user.UserRoles.Select(c => c.RoleId).ToList();
        var collectionBeforePaging = All
                                    .Where(d => !d.IsArchive && (d.DocumentUserPermissions.Any(c => c.DocumentId == Id && c.UserId == user.Id && (!c.IsTimeBound || (c.IsTimeBound && c.StartDate < today && c.EndDate > today)))
                                                || (d.DocumentRolePermissions.Any(c => c.DocumentId == Id && userRoles.Contains(c.RoleId) && (!c.IsTimeBound || (c.IsTimeBound && c.StartDate < today && c.EndDate > today))))));

        var document = await collectionBeforePaging.FirstOrDefaultAsync();

        if (document == null)
        {
            return null;
        }

        var result = _mapper.Map<DocumentDto>(document);
        result.IsAllowDownload = _documentUserPermissionRepository.All.Any(c => c.DocumentId == Id && c.IsAllowDownload && c.UserId == user.Id && (!c.IsTimeBound || (c.IsTimeBound && c.StartDate < today && c.EndDate > today)));
        if (result.IsAllowDownload)
        {
            return result;
        }
        result.IsAllowDownload = _documentRolePermissionRepository.All.Any(c => c.DocumentId == Id && c.IsAllowDownload && userRoles.Contains(c.RoleId) && (!c.IsTimeBound || (c.IsTimeBound && c.StartDate < today && c.EndDate > today)));
        return result;
    }

    public async Task<int> UpdateIsSharedByCategoryIdAsync(List<Guid> affectedCategoryIds, bool isShared)
    {
        return await All
            .Where(d => affectedCategoryIds.Contains(d.CategoryId))
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(d => d.IsShared, isShared));
    }


    public async Task UpdateDocumentSharingFlagAsync(List<Guid> affectedCategoryIds)
    {
        var documents = await All
            .Include(d => d.DocumentUserPermissions)
            .Include(d => d.DocumentRolePermissions)
            .Where(d => affectedCategoryIds.Contains(d.CategoryId) && d.IsShared)
            .Select(d => new
            {
                d.Id,
                d.CategoryId,
                IsSharedWithUser = d.DocumentUserPermissions.Any(),
                IsSharedWithRole = d.DocumentRolePermissions.Any(),
            })
            .ToListAsync();

        if (documents.Count() == 0)
            return;

        var remainingUserPermissions = await _categoryUserPermissionRepository.All
            .Where(p => affectedCategoryIds.Contains(p.CategoryId))
            .ToListAsync();

        var remainingRolePermissions = await _categoryRolePermissionRepository.All
            .Where(p => affectedCategoryIds.Contains(p.CategoryId))
            .ToListAsync();

        var docIdsToUnshare = documents
            .Where(doc =>
                !doc.IsSharedWithUser &&
                !doc.IsSharedWithRole &&
                !remainingUserPermissions.Any(p => p.CategoryId == doc.CategoryId) &&
                !remainingRolePermissions.Any(p => p.CategoryId == doc.CategoryId))
            .Select(d => d.Id)
            .ToList();

        if (docIdsToUnshare.Any())
        {
            await All
                .Where(d => docIdsToUnshare.Contains(d.Id))
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(d => d.IsShared, false));
        }
    }

    public async Task<bool> HasCategorySharedPermissionsAsync(Guid categoryId)
    {
        return await _categoryUserPermissionRepository.All.AnyAsync(x => x.CategoryId == categoryId)
            || await _categoryRolePermissionRepository.All.AnyAsync(x => x.CategoryId == categoryId);
    }



    public async Task<string> GetDocumentName(string name, Guid categoryId, int index = 0)
    {
        var modifiedName = "";
        var extension = Path.GetExtension(name);
        var fileName = name.Replace(extension, "");
        if (index != 0)
        {
            fileName = fileName + "(" + index + ")";
            modifiedName = fileName + extension;
        }
        else
        {
            fileName = name;
            modifiedName = name;
        }
        var distinationDocument = await FindBy(c => c.CategoryId == categoryId && c.Name == modifiedName)
                                           .FirstOrDefaultAsync();
        if (distinationDocument != null)
        {
            return await GetDocumentName(name, distinationDocument.CategoryId, ++index);
        }
        else
        {
            return modifiedName;
        }
    }
}

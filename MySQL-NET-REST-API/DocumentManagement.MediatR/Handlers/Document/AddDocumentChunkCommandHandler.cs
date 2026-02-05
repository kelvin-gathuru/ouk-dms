using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Handlers.StorageStategies;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;

public class AddDocumentChunkCommandHandler(
    IDocumentRepository _documentRepository,
    IUnitOfWork<DocumentContext> _uow,
    IMapper _mapper,
    UserInfoToken _userInfo,
    StorageServiceFactory _storeageServiceFactory,
    IStorageSettingRepository _storageSettingRepository,
    IDocumentIndexRepository _documentIndexRepository,
    IDocumentUserPermissionRepository _documentUserPermissionRepository,
    IDocumentAuditTrailRepository _documentAuditTrailRepository,
    IDocumentVersionRepository documentVersionRepository
    ) : IRequestHandler<AddDocumentChunkCommand, ServiceResponse<DocumentDto>>
{
    public async Task<ServiceResponse<DocumentDto>> Handle(AddDocumentChunkCommand request, CancellationToken cancellationToken)
    {
        var entityExist = await _documentRepository.FindBy(c => c.Name == request.Name && c.CategoryId == request.CategoryId).FirstOrDefaultAsync();
        if (entityExist != null)
        {
            return ServiceResponse<DocumentDto>.ReturnFailed(409, "Document already exist.");
        }
        var storeageSetting = await _storageSettingRepository.GetStorageSettingByIdOrLocal(request.StorageSettingId);
        var storageService = _storeageServiceFactory.GetStorageService(storeageSetting.StorageType);
        var keyValut = KeyGenerator.GenerateKeyAndIV();
        //var url = UploadFile(request.Files[0]);
        var entity = _mapper.Map<Document>(request);
        entity.Id = Guid.NewGuid();
        entity.CreatedBy = _userInfo.Id;
        entity.CreatedDate = DateTime.UtcNow;
        entity.Url = request.Url;
        entity.Key = storeageSetting.EnableEncryption ? keyValut.Item1 : null;
        entity.IV = storeageSetting.EnableEncryption ? keyValut.Item2 : null;
        entity.StorageType = storeageSetting.StorageType;
        entity.StorageSettingId = storeageSetting.Id;
        entity.IsChunk = true;
        entity.IsAllChunkUploaded = false;
        entity.Extension = request.Extension;
        entity.DocumentNumber = await _documentRepository.GenerateDocumentNumberAsync();
        var extension = request.Extension;
        if (!extension.StartsWith("."))
            extension = "." + extension;
        entity.Url = Guid.NewGuid() + extension;
        try
        {
            if (!string.IsNullOrEmpty(request.DocumentMetaDataString))
            {
                var metaData = JsonConvert.DeserializeObject<List<DocumentMetaDataDto>>(request.DocumentMetaDataString);

                var metaDataFilter = metaData.Where(c => c.DocumentMetaTagId != null && (c.Metatag != "" || c.MetaTagDate != null)).ToList();
                if (metaDataFilter.Count > 0)
                {
                    entity.DocumentMetaDatas = _mapper.Map<List<Data.DocumentMetaData>>(metaDataFilter);
                }

            }
        }
        catch
        {
            // igonre
        }

        try
        {
            if (!string.IsNullOrEmpty(request.DocumentUserPermissionString))
            {
                var documentUserPermissions = JsonConvert.DeserializeObject<List<DocumentUserPermissionDto>>(request.DocumentUserPermissionString);
                entity.DocumentUserPermissions = _mapper.Map<List<Data.DocumentUserPermission>>(documentUserPermissions);
            }

        }
        catch
        {
            // igonre
        }

        try
        {
            if (!string.IsNullOrEmpty(request.DocumentRolePermissionString))
            {
                var documentRolePermissions = JsonConvert.DeserializeObject<List<DocumentRolePermissionDto>>(request.DocumentRolePermissionString);
                entity.DocumentRolePermissions = _mapper.Map<List<Data.DocumentRolePermission>>(documentRolePermissions);
            }
        }
        catch
        {
            // igonre
        }
        entity.IsAddedPageIndxing = false;
        if (entity.OnExpiryAction != null && entity.RetentionPeriodInDays != null && entity.RetentionPeriodInDays > 0)
        {
            entity.RetentionDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(entity.RetentionPeriodInDays.Value));
        }
        else
        {
            entity.RetentionDate = null;
        }
        if (entity.OnExpiryAction != null && entity.RetentionPeriodInDays != null && entity.RetentionPeriodInDays > 0)
        {
            entity.RetentionDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(entity.RetentionPeriodInDays.Value));
        }
        else
        {
            entity.RetentionDate = null;
        }

        var version = new DocumentVersion
        {
            DocumentId = entity.Id,
            Url = entity.Url,
            Key = entity.Key,
            IV = entity.IV,
            IsCurrentVersion = true,
            VersionNumber = 1,
            CreatedBy = _userInfo.Id,
            CreatedDate = DateTime.UtcNow,
            ModifiedBy = _userInfo.Id,
            ModifiedDate = DateTime.UtcNow,
            SignById = entity.SignById,
            SignDate = entity.SignDate,
            Comment = entity.Comment,
            Extension = entity.Extension,
            IsChunk = true,
            IsAllChunkUploaded = false
        };
        // Check if document has direct user or role permissions
        bool hasDocumentPermissions =
            (entity.DocumentUserPermissions != null && entity.DocumentUserPermissions.Any()) ||
            (entity.DocumentRolePermissions != null && entity.DocumentRolePermissions.Any());

        // Check if category has user or role permissions
        bool hasCategoryPermissions = false;
        if (entity.CategoryId != Guid.Empty)
        {
            hasCategoryPermissions = await _documentRepository
                .HasCategorySharedPermissionsAsync(entity.CategoryId);
        }

        // Set IsShared flag
        if (hasDocumentPermissions || hasCategoryPermissions)
        {
            entity.IsShared = true;
        }

        _documentRepository.Add(entity);

        documentVersionRepository.Add(version);

        if (request.IsAssignToMe)
        {
            var documentUserPermission = new DocumentUserPermission
            {
                Id = Guid.NewGuid(),
                DocumentId = entity.Id,
                UserId = _userInfo.Id,
                IsTimeBound = false,
                IsAllowDownload = true,
                CreatedBy = _userInfo.Id,
                CreatedDate = DateTime.UtcNow

            };
            _documentUserPermissionRepository.Add(documentUserPermission);
            var documentAudit = new DocumentAuditTrail()
            {
                DocumentId = entity.Id,
                CreatedBy = _userInfo.Id,
                CreatedDate = DateTime.UtcNow,
                OperationName = DocumentOperation.Added_Permission,
                AssignToUserId = _userInfo.Id
            };
            _documentAuditTrailRepository.Add(documentAudit);
        }
        _documentIndexRepository.Add(new DocumentIndex { Id = Guid.NewGuid(), DocumentVersionId = version.Id });
        if (await _uow.SaveAsync() <= 0)
        {
            return ServiceResponse<DocumentDto>.ReturnFailed(500, "Error While Added Document");
        }

        var entityDto = _mapper.Map<DocumentDto>(entity);
        entityDto.DocumentVersionId = version.Id;
        return ServiceResponse<DocumentDto>.ReturnResultWith200(entityDto);
    }
}

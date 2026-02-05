using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Handlers.LuceneHandler;
using DocumentManagement.MediatR.Handlers.StorageStategies;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DocumentManagement.MediatR.Handlers;

public class AddDocumentToMeCommandHandler(
    IDocumentRepository _documentRepository,
    IUnitOfWork<DocumentContext> _uow,
    IMapper _mapper,
    UserInfoToken _userInfo,
    IDocumentUserPermissionRepository _documentUserPermissionRepository,
    IDocumentAuditTrailRepository _documentAuditTrailRepository,
    PathHelper _pathHelper,
    IWebHostEnvironment _webHostEnvironment,
    IStorageSettingRepository _storageSettingRepository,
    StorageServiceFactory _storeageServiceFactory,
    IDocumentIndexRepository _documentIndexRepository,
    ILogger<AddDocumentToMeCommandHandler> _logger,
    IHubContext<UserHub, IHubClient> hubContext,
    IConnectionMappingRepository connectionMappingRepository,
    IDocumentVersionRepository documentVersionRepository,
    ICategoryRepository categoryRepository

    ) : IRequestHandler<AddDocumentToMeCommand, ServiceResponse<DocumentDto>>
{
    public async Task<ServiceResponse<DocumentDto>> Handle(AddDocumentToMeCommand request, CancellationToken cancellationToken)
    {
        if (request.Files.Count == 0)
        {
            return ServiceResponse<DocumentDto>.ReturnFailed(409, "Please select the file.");
        }
        if (!FileSignatureHelper.IsFileSignatureValid(request.Files[0]))
        {
            return ServiceResponse<DocumentDto>.ReturnFailed(409, "Invalid file signature.");
        }
        var entityExist = await _documentRepository.FindBy(c => c.Name == request.Name && c.CategoryId == request.CategoryId).FirstOrDefaultAsync();
        if (entityExist != null)
        {
            return ServiceResponse<DocumentDto>.ReturnFailed(409, "Document already exist.");
        }

        long fileSizeInBytes = request.Files[0].Length;
        // Convert file size to kilobytes or megabytes if necessary
        double fileSizeInKB = fileSizeInBytes / 1024.0;

        var storeageSetting = await _storageSettingRepository.GetStorageSettingByIdOrLocal(request.StorageSettingId);

        var storageService = _storeageServiceFactory.GetStorageService(storeageSetting.StorageType);

        var fileNameKeyValut = await storageService.UploadFileAsync(request.Files[0], storeageSetting, request.Extension);

        if (string.IsNullOrEmpty(fileNameKeyValut.FileName))
        {
            return ServiceResponse<DocumentDto>.Return422("Settings are not properly setup.");
        }
        var keyValut = KeyGenerator.GenerateKeyAndIV();

        //var url = UploadFile(request.Files[0]);

        var entity = _mapper.Map<Document>(request);
        entity.Id = Guid.NewGuid();
        entity.CreatedBy = _userInfo.Id;
        entity.CreatedDate = DateTime.UtcNow;
        entity.Url = fileNameKeyValut.FileName;
        entity.Key = fileNameKeyValut.Key;
        entity.IV = fileNameKeyValut.IV;
        entity.StorageType = storeageSetting.StorageType;
        entity.StorageSettingId = storeageSetting.Id;
        entity.DocumentNumber = await _documentRepository.GenerateDocumentNumberAsync();
        entity.Extension = request.Extension;
        entity.IsAddedPageIndxing = true;
        entity.IsChunk = false;
        entity.IsAllChunkUploaded = true;
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
            Url = fileNameKeyValut.FileName,
            Key = fileNameKeyValut.Key,
            IV = fileNameKeyValut.IV,
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
            IsChunk = false,
            IsAllChunkUploaded = true
        };
        //await documentVersionRepository.UpdateCurrentVersion(entityExist.Id);

        documentVersionRepository.Add(version);

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
        var maxSizeQuick = _pathHelper.MaxFileSizeIndexingQuick;
        if (fileSizeInKB > maxSizeQuick)
        {
            entity.IsAddedPageIndxing = false;
        }

        bool hasCategoryPermissions = false;
        if (entity.CategoryId != Guid.Empty)
        {
            hasCategoryPermissions = await _documentRepository
                .HasCategorySharedPermissionsAsync(entity.CategoryId);
        }
        if (hasCategoryPermissions)
        {
            entity.IsShared = true;
        }

        _documentRepository.Add(entity);
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


        if (fileSizeInKB > maxSizeQuick)
        {
            entity.IsAddedPageIndxing = false;
            _documentIndexRepository.Add(new DocumentIndex { Id = Guid.NewGuid(), DocumentVersionId = version.Id });
        }

        if (await _uow.SaveAsync() <= 0)
        {
            return ServiceResponse<DocumentDto>.ReturnFailed(500, "Error While Added Document");
        }
        try
        {
            var onlineUsers = connectionMappingRepository.GetAllUsersExceptThis(new SignlarUser { Id = _userInfo.Id.ToString() });
            if (onlineUsers.Count() > 0)
            {
                var userInfo = connectionMappingRepository.GetUserInfoById(_userInfo.Id);
                if (userInfo != null)
                {
                    await hubContext.Clients.AllExcept(new List<string> { userInfo.ConnectionId }).SendNotificationFolderChange(entity.CategoryId);
                }
                else
                {
                    await hubContext.Clients.All.SendNotificationFolderChange(entity.CategoryId);
                }
            }

            var user = connectionMappingRepository.GetUserInfoById(_userInfo.Id);
            if (user != null)
            {
                await hubContext.Clients.AllExcept(new List<string> { user.ConnectionId }).RefreshDocuments(entity.CategoryId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SignalR Error");

        }
        var entityDto = _mapper.Map<DocumentDto>(entity);
        if (fileSizeInKB <= maxSizeQuick)
        {
            try
            {
                string extension = Path.GetExtension(request.Files[0].FileName);
                var extractor = ContentExtractorFactory.GetExtractor(extension, _webHostEnvironment.WebRootPath);
                var imagessupport = _pathHelper.IMAGESSUPPORT;
                if (extractor != null)
                {
                    string tessFilePath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, _pathHelper.TESSDATA);
                    var content = new DocumentProcessor(extractor).ProcessDocumentByIFile(request.Files[0], tessFilePath, _pathHelper.TESSSUPPORTLANGUAGES);
                    content = UnWantKeywordRemovalHelper.CleanExtractedText(content);
                    if (!string.IsNullOrEmpty(content))
                    {
                        string searchIndexPath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, _pathHelper.SearchIndexPath);
                        var indexWriterManager = new IndexWriterManager(searchIndexPath);
                        indexWriterManager.AddDocument(version.Id.ToString(), content);
                        indexWriterManager.Commit();
                        indexWriterManager.Dispose();
                    }
                }
                else if (Array.Exists(imagessupport, element => element.ToLower() == extension.ToLower()))
                {
                    string tessFilePath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, _pathHelper.TESSDATA);
                    var tessDataContextExtractor = new TessDataContextExtractor();
                    var tessLang = _pathHelper.TESSSUPPORTLANGUAGES;
                    var content = await tessDataContextExtractor.ExtractContentByFile(tessFilePath, request.Files[0], tessLang, _logger);
                    content = UnWantKeywordRemovalHelper.CleanExtractedText(content);
                    if (!string.IsNullOrEmpty(content))
                    {
                        string searchIndexPath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, _pathHelper.SearchIndexPath);
                        var indexWriterManager = new IndexWriterManager(searchIndexPath);
                        indexWriterManager.AddDocument(version.Id.ToString(), content);
                        indexWriterManager.Commit();
                        indexWriterManager.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while indexing document");
                _documentIndexRepository.Add(new DocumentIndex { Id = Guid.NewGuid(), DocumentVersionId = version.Id });
                return ServiceResponse<DocumentDto>.ReturnResultWith200(entityDto);
            }
        }
        return ServiceResponse<DocumentDto>.ReturnResultWith200(entityDto);
    }

}
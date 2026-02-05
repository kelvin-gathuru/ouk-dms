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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;

public class ApproveDocumentCommandHandler(
    IDocumentAuditTrailRepository _documentAuditTrailRepository,
    IMediator _mediator,
    IFileRequestDocumentRepository _fileRequestDocumentRepository,
    IDocumentRepository _documentRepository,
    IDocumentVersionRepository _documentVersionRepository,
    IUnitOfWork<DocumentContext> _uow,
    IMapper _mapper,
    UserInfoToken _userInfo,
    PathHelper _pathHelper,
    IWebHostEnvironment _webHostEnvironment,
    StorageServiceFactory _storeageServiceFactory,
    IStorageSettingRepository _storageSettingRepository,
    ILogger<AddDocumentCommandHandler> _logger,
    IDocumentIndexRepository _documentIndexRepository,
    IHubContext<UserHub, IHubClient> hubContext,
    IConnectionMappingRepository connectionMappingRepository,
    IDocumentUserPermissionRepository _documentUserPermissionRepository) : IRequestHandler<ApproveDocumentCommand, ServiceResponse<FileRequestDocumentDto>>
{
    private IFormFile ConvertByteArrayToIFormFile(byte[] fileBytes, string fileName, string contentType)
    {
        var stream = new MemoryStream(fileBytes);
        var formFile = new FormFile(stream, 0, fileBytes.Length, null, fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType
        };
        return formFile;
    }
    public async Task<ServiceResponse<FileRequestDocumentDto>> Handle([FromForm] ApproveDocumentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var command = new DownloadFileRequestDocumentCommand
            {
                Id = request.FileRequestDocumentId
            };

            var response = await _mediator.Send(command);
            var downloadDocument = response.Data;
            var files = ConvertByteArrayToIFormFile(downloadDocument.Data, downloadDocument.ContentType, downloadDocument.FileName);
            long fileSizeInBytes = files.Length;
            //Convert file size to kilobytes or megabytes if necessary
            double fileSizeInKB = fileSizeInBytes / 1024.0;

            var entityExist = await _documentRepository.FindBy(c => c.Name == request.Name && c.CategoryId == request.CategoryId).FirstOrDefaultAsync();
            if (entityExist != null)
            {
                return ServiceResponse<FileRequestDocumentDto>.ReturnFailed(409, "Document already exist.");
            }
            var storeageSetting = await _storageSettingRepository.GetStorageSettingByIdOrLocal(request.StorageSettingId);

            var storageService = _storeageServiceFactory.GetStorageService(storeageSetting.StorageType);

            var fileNameKeyValut = await storageService.UploadFileAsync(files, storeageSetting, request.Extension);

            if (string.IsNullOrEmpty(fileNameKeyValut.FileName))
            {
                return ServiceResponse<FileRequestDocumentDto>.Return422("Settings are not properly setup.");
            }
            var keyValut = KeyGenerator.GenerateKeyAndIV();
            //var url = UploadFile(request.Files[0]);
            var entity = new Document();
            entity.Id = Guid.NewGuid();
            entity.DocumentNumber = await _documentRepository.GenerateDocumentNumberAsync();
            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.Url = request.Url;
            entity.CategoryId = request.CategoryId;
            entity.DocumentStatusId = request.DocumentStatusId;
            entity.CreatedBy = _userInfo.Id;
            entity.CreatedDate = DateTime.UtcNow;
            entity.Url = fileNameKeyValut.FileName;
            entity.Key = fileNameKeyValut.Key;
            entity.IV = fileNameKeyValut.IV;
            entity.StorageType = storeageSetting.StorageType;
            entity.StorageSettingId = storeageSetting.Id;
            entity.ClientId = request.ClientId;
            entity.Extension = fileNameKeyValut.FileName.Split('.').Last();
            entity.IsChunk = false;
            entity.IsAllChunkUploaded = true;
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
            var maxSizeQuick = _pathHelper.MaxFileSizeIndexingQuick;
            entity.IsAddedPageIndxing = true;
            if (fileSizeInKB > maxSizeQuick)
            {
                entity.IsAddedPageIndxing = false;
            }

            // Check if category has user or role permissions
            bool hasCategoryPermissions = false;
            if (entity.CategoryId != Guid.Empty)
            {
                hasCategoryPermissions = await _documentRepository
                    .HasCategorySharedPermissionsAsync(entity.CategoryId);
            }
            // Set IsShared flag
            if (hasCategoryPermissions)
            {
                entity.IsShared = true;
            }

            _documentRepository.Add(entity);

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
                IsChunk = false,
                IsAllChunkUploaded = true
            };
            _documentVersionRepository.Add(version);

            if (fileSizeInKB > maxSizeQuick)
            {
                _documentIndexRepository.Add(new DocumentIndex { Id = Guid.NewGuid(), DocumentVersionId = version.Id });
            }
            var fileRequestDocument = await _fileRequestDocumentRepository.FindBy(c => c.Id == request.FileRequestDocumentId).FirstOrDefaultAsync();
            if (fileRequestDocument != null)
            {
                fileRequestDocument.ApprovalOrRjectedById = _userInfo.Id;
                fileRequestDocument.ApprovedRejectedDate = DateTime.UtcNow;
                fileRequestDocument.FileRequestDocumentStatus = FileRequestDocumentStatus.APPROVED;
                _fileRequestDocumentRepository.Update(fileRequestDocument);
            }
            var documentAuditTrail = _documentAuditTrailRepository.FindBy(c => c.DocumentId == entity.Id);
            if (documentAuditTrail == null)
            {
                var documentAudit = new DocumentAuditTrail()
                {
                    Id = entity.Id,
                    OperationName = DocumentOperation.Created
                };
                _documentAuditTrailRepository.Add(documentAudit);
            }

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
            var documentAuditPermission = new DocumentAuditTrail()
            {
                DocumentId = entity.Id,
                CreatedBy = _userInfo.Id,
                CreatedDate = DateTime.UtcNow,
                OperationName = DocumentOperation.Added_Permission,
                AssignToUserId = _userInfo.Id
            };
            _documentAuditTrailRepository.Add(documentAuditPermission);

            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<FileRequestDocumentDto>.ReturnFailed(500, "Error While Added Document");
            }
            var entityDto = _mapper.Map<FileRequestDocumentDto>(fileRequestDocument);

            try
            {
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


            if (fileSizeInKB <= maxSizeQuick)
            {
                try
                {
                    string extension = Path.GetExtension(files.FileName);
                    var extractor = ContentExtractorFactory.GetExtractor(extension, _webHostEnvironment.WebRootPath);
                    var imagessupport = _pathHelper.IMAGESSUPPORT;
                    if (extractor != null)
                    {
                        string tessFilePath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, _pathHelper.TESSDATA);
                        var content = new DocumentProcessor(extractor).ProcessDocumentByIFile(files, tessFilePath, _pathHelper.TESSSUPPORTLANGUAGES);
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
                        var content = await tessDataContextExtractor.ExtractContentByFile(tessFilePath, files, tessLang, _logger);
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
                    return ServiceResponse<FileRequestDocumentDto>.ReturnResultWith200(entityDto);
                }
            }

            return ServiceResponse<FileRequestDocumentDto>.ReturnResultWith200(entityDto);
        }
        catch (Exception)
        {
            return ServiceResponse<FileRequestDocumentDto>.Return500();
        }

    }

}

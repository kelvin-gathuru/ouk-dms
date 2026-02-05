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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;
public class AddDocumentWindowSharedCommandHandler(
    IDocumentRepository _documentRepository,
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
    IDocumentVersionRepository documentVersionRepository) : IRequestHandler<AddDocumentWindowSharedCommand, ServiceResponse<DocumentDto>>
{
    public async Task<ServiceResponse<DocumentDto>> Handle(AddDocumentWindowSharedCommand request, CancellationToken cancellationToken)
    {
        if (request.Files == null)
        {
            return ServiceResponse<DocumentDto>.ReturnFailed(409, "Please select the file.");
        }

        if (!FileSignatureHelper.IsFileSignatureValid(request.Files))
        {
            return ServiceResponse<DocumentDto>.ReturnFailed(409, "Invalid file signature.");
        }
        long fileSizeInBytes = request.Files.Length;
        // Convert file size to kilobytes or megabytes if necessary
        double fileSizeInKB = fileSizeInBytes / 1024.0;

        var entityExist = await _documentRepository.FindBy(c => c.Name == request.Name && c.CategoryId == request.CategoryId).FirstOrDefaultAsync();
        if (entityExist != null)
        {
            request.Name = await _documentRepository.GetDocumentName(request.Name, request.CategoryId);
        }
        var storeageSetting = await _storageSettingRepository.GetStorageSettingByIdOrLocal(request.StorageSettingId);

        var storageService = _storeageServiceFactory.GetStorageService(storeageSetting.StorageType);

        var fileNameKeyValut = await storageService.UploadFileAsync(request.Files, storeageSetting, request.Extension);

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
        var maxSizeQuick = _pathHelper.MaxFileSizeIndexingQuick;
        if (fileSizeInKB > maxSizeQuick)
        {
            entity.IsAddedPageIndxing = false;

        }
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

        _documentRepository.Add(entity);
        documentVersionRepository.Add(version);

        if (fileSizeInKB > maxSizeQuick)
        {
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

        if (fileSizeInKB <= maxSizeQuick)
        {
            try
            {
                string extension = Path.GetExtension(request.Files.FileName);
                var extractor = ContentExtractorFactory.GetExtractor(extension, _webHostEnvironment.WebRootPath);
                var imagessupport = _pathHelper.IMAGESSUPPORT;
                if (extractor != null)
                {
                    string tessFilePath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, _pathHelper.TESSDATA);
                    var content = new DocumentProcessor(extractor).ProcessDocumentByIFile(request.Files, tessFilePath, _pathHelper.TESSSUPPORTLANGUAGES);
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
                    var content = await tessDataContextExtractor.ExtractContentByFile(tessFilePath, request.Files, tessLang, _logger);
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

            }
        }
        //if (entity.Extension.ToLower() == ".docx" || entity.Extension.ToLower() == ".docx" || entity.Extension.ToLower() == "doc" || entity.Extension.ToLower() == "docx")
        //{
        //    var convertDocToPDFCommand = new ConvertDocToPDFCommand
        //    {
        //        DocumentId = version.Id
        //    };
        //    await mediator.Send(convertDocToPDFCommand);
        //}
        var entityDto = _mapper.Map<DocumentDto>(entity);
        return ServiceResponse<DocumentDto>.ReturnResultWith200(entityDto);

    }

}


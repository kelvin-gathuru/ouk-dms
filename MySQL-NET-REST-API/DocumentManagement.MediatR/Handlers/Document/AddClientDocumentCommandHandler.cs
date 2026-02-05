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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DocumentManagement.MediatR.Handlers;

public class AddClientDocumentCommandHandler(
    IDocumentRepository _documentRepository,
    IUnitOfWork<DocumentContext> _uow,
    IMapper _mapper,
    PathHelper _pathHelper,
    IWebHostEnvironment _webHostEnvironment,
    StorageServiceFactory _storeageServiceFactory,
    IStorageSettingRepository _storageSettingRepository,
    ILogger<AddClientDocumentCommandHandler> _logger,
    IDocumentIndexRepository _documentIndexRepository,
    IHubContext<UserHub, IHubClient> hubContext,
    IConnectionMappingRepository connectionMappingRepository,
    IDocumentVersionRepository documentVersionRepository,
    EmailHelper _emailHelper,
    IConfiguration _configuration,
    IClientRepository _clientRepository) : IRequestHandler<AddClientDocumentCommand, ServiceResponse<DocumentDto>>
{
    public async Task<ServiceResponse<DocumentDto>> Handle(AddClientDocumentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Starting AddClientDocument for file: {FileName}", request.Files?.FileName);
            
            if (request.Files == null)
            {
                return ServiceResponse<DocumentDto>.ReturnFailed(409, "Please select the file.");
            }

            if (!FileSignatureHelper.IsFileSignatureValid(request.Files))
            {
                return ServiceResponse<DocumentDto>.ReturnFailed(409, "Invalid file signature.");
            }
            
            long fileSizeInBytes = request.Files.Length;
            double fileSizeInKB = fileSizeInBytes / 1024.0;

            var entityExist = await _documentRepository.FindBy(c => c.Name == request.Name && c.CategoryId == request.CategoryId).FirstOrDefaultAsync();
            if (entityExist != null)
            {
                return ServiceResponse<DocumentDto>.ReturnFailed(409, "Document already exist.");
            }
            
            _logger.LogInformation("Getting storage setting for ID: {StorageSettingId}", request.StorageSettingId);
            var storeageSetting = await _storageSettingRepository.GetStorageSettingByIdOrLocal(request.StorageSettingId);
            
            if (storeageSetting == null)
            {
                _logger.LogError("Storage setting not found for ID: {StorageSettingId}", request.StorageSettingId);
                return ServiceResponse<DocumentDto>.ReturnFailed(500, "Storage settings not configured.");
            }

            var storageService = _storeageServiceFactory.GetStorageService(storeageSetting.StorageType);

            var fileNameKeyValut = await storageService.UploadFileAsync(request.Files, storeageSetting, request.Extension);

            if (string.IsNullOrEmpty(fileNameKeyValut.FileName))
            {
                return ServiceResponse<DocumentDto>.Return422("Settings are not properly setup.");
            }
            
            var entity = _mapper.Map<Document>(request);
            entity.Id = Guid.NewGuid();
            
            // Set CreatedBy and ModifiedBy to Super Admin ID (4b352b37-332a-40c6-ab05-e38fcf109719)
            var superAdminId = Guid.Parse("4b352b37-332a-40c6-ab05-e38fcf109719");
            entity.CreatedBy = superAdminId;
            entity.ModifiedBy = superAdminId;
            entity.CreatedDate = DateTime.UtcNow;
            entity.ModifiedDate = DateTime.UtcNow;
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
            entity.ClientId = request.ClientId; // Set ClientId
            
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
                Id = Guid.NewGuid(),
                DocumentId = entity.Id,
                Url = entity.Url,
                Key = fileNameKeyValut.Key,
                IV = fileNameKeyValut.IV,
                IsCurrentVersion = true,
                VersionNumber = 1, // Assuming this is the first version, so it's 1
                CreatedBy = superAdminId,
                CreatedDate = DateTime.UtcNow,
                ModifiedBy = superAdminId,
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
                // ignore
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
                // ignore
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
                // ignore
            }

            // Check if document has direct user or role permissions
            bool hasDocumentPermissions =
                (entity.DocumentUserPermissions != null && entity.DocumentUserPermissions.Any()) ||
                (entity.DocumentRolePermissions != null && entity.DocumentRolePermissions.Any());

            // For client uploads, don't check category permissions - set IsShared to false
            // This ensures the document is visible to all users without permission filtering
            entity.IsShared = false;

            _documentRepository.Add(entity);
            documentVersionRepository.Add(version);

            if (fileSizeInKB > maxSizeQuick)
            {
                _documentIndexRepository.Add(new DocumentIndex { Id = Guid.NewGuid(), DocumentVersionId = version.Id });
            }
            
            _logger.LogInformation("About to save document. Entity ID: {EntityId}, Version ID: {VersionId}, CategoryId: {CategoryId}, ClientId: {ClientId}", 
                entity.Id, version.Id, entity.CategoryId, entity.ClientId);
            
            try
            {
                var saveResult = await _uow.SaveAsync();
                _logger.LogInformation("SaveAsync completed with result: {SaveResult}", saveResult);
            }
            catch (Exception saveEx)
            {
                _logger.LogError(saveEx, "Database error while saving document: {Message}. Inner: {InnerMessage}", 
                    saveEx.Message, saveEx.InnerException?.Message);
                return ServiceResponse<DocumentDto>.ReturnFailed(500, $"Database error: {saveEx.InnerException?.Message ?? saveEx.Message}");
            }
            
            try
            {
                // SignalR notifications - skip if no user context available
                var onlineUsers = connectionMappingRepository.GetAllUsersExceptThis(new SignlarUser { Id = (request.ClientId ?? Guid.Empty).ToString() });
                if (onlineUsers.Count() > 0)
                {
                    await hubContext.Clients.All.SendNotificationFolderChange(entity.CategoryId);
                    await hubContext.Clients.All.DeleteFolder(entity.Id);
                    await hubContext.Clients.All.RefreshDocuments(entity.CategoryId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SignalR Error");
            }

            // Send Email Notification to Client
            if (request.ClientId.HasValue)
            {
                try
                {
                    var client = await _clientRepository.FindBy(c => c.Id == request.ClientId.Value).FirstOrDefaultAsync();
                    if (client != null && !string.IsNullOrEmpty(client.Email))
                    {
                        var smtpSettings = _configuration.GetSection("SmtpSettings");
                        var sendEmailSpecification = new SendEmailSpecification
                        {
                            FromAddress = smtpSettings["FromEmail"],
                            FromName = smtpSettings["FromName"],
                            ToAddress = client.Email,
                            ToName = client.ContactPerson,
                            Subject = "Petition Submission Confirmation",
                            Body = $@"
                                <h3>Petition Received</h3>
                                <p>Dear {client.ContactPerson},</p>
                                <p>This is to confirm that your petition <strong>{entity.Name}</strong> has been successfully submitted to the Parliament System.</p>
                                <p>Reference Number: <strong>{entity.DocumentNumber}</strong></p>
                                <p>Thank you for your submission.</p>",
                            Host = smtpSettings["Host"],
                            Port = int.Parse(smtpSettings["Port"]),
                            UserName = smtpSettings["UserName"],
                            Password = smtpSettings["Password"],
                            EncryptionType = smtpSettings["EncryptionType"]
                        };

                        await _emailHelper.SendEmail(sendEmailSpecification);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error sending petition confirmation email");
                    // Don't fail the request if email fails, just log it
                }
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
            
            var entityDto = _mapper.Map<DocumentDto>(entity);
            _logger.LogInformation("Successfully created document with ID: {DocumentId}", entity.Id);
            return ServiceResponse<DocumentDto>.ReturnResultWith200(entityDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in AddClientDocumentCommandHandler: {Message}", ex.Message);
            return ServiceResponse<DocumentDto>.ReturnFailed(500, $"Error uploading document: {ex.Message}");
        }
    }
}

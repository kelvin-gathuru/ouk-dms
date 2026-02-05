using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Handlers.StorageStategies;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DocumentManagement.MediatR.Handlers;

public class MarkDocumentChunksUploadedStatusCommandHandler(
    IDocumentRepository documentRepository,
    IDocumentVersionRepository documentVersionRepository,
    IDocumentChunkRepository documentChunkRepository,
    IUnitOfWork<DocumentContext> uow,
    IHubContext<UserHub, IHubClient> hubContext,
    IStorageSettingRepository _storageSettingRepository,
    StorageServiceFactory _storageServiceFactory,
    IDocumentAuditTrailRepository _documentAuditTrailRepository,
    ILogger<MarkDocumentChunksUploadedStatusCommandHandler> _logger,
    ICategoryRepository categoryRepository,
    IConnectionMappingRepository connectionMappingRepository,
    UserInfoToken userInfoToken) : IRequestHandler<MarkDocumentChunksUploadedStatusCommand, ServiceResponse<DocumentChunkStatus>>
{
    public async Task<ServiceResponse<DocumentChunkStatus>> Handle(MarkDocumentChunksUploadedStatusCommand request, CancellationToken cancellationToken)
    {
        var documentVersion = documentVersionRepository.All.Where(c => (c.DocumentId == request.DocumentId && c.IsCurrentVersion) || c.Id == request.DocumentId).FirstOrDefault();

        if (documentVersion == null)
        {
            return ServiceResponse<DocumentChunkStatus>.ReturnFailed(404, "Document Version not found.");
        }
        var document = await documentRepository.FindBy(c => c.Id == documentVersion.DocumentId).FirstOrDefaultAsync();
        if (document == null)
        {
            return ServiceResponse<DocumentChunkStatus>.ReturnFailed(404, "Document not found.");
        }
        var documentChunks = documentChunkRepository.All.Where(c => c.DocumentVersionId == documentVersion.Id).OrderByDescending(c => c.ChunkIndex).ToList();
        if (documentChunks.Count == 0)
        {
            return ServiceResponse<DocumentChunkStatus>.ReturnFailed(404, "Document Chunks not found.");
        }
        if (documentChunks[0].TotalChunk == documentChunks.Count())
        {
            document.IsAllChunkUploaded = request.status;
            if (documentVersion != null)
            {
                documentVersion.IsAllChunkUploaded = request.status;
                documentVersionRepository.Update(documentVersion);
            }
            documentRepository.Update(document);
            if (await uow.SaveAsync() <= 0)
            {
                return ServiceResponse<DocumentChunkStatus>.ReturnFailed(500, "Error While Added Document");
            }
            try
            {
                var user = connectionMappingRepository.GetUserInfoById(userInfoToken.Id);
                if (user != null)
                {
                    await hubContext.Clients.AllExcept(new List<string> { user.ConnectionId }).RefreshDocuments(document.CategoryId);
                }

                var category = categoryRepository.All.Where(c => c.Id == document.CategoryId).FirstOrDefault();
                var onlineUsers = connectionMappingRepository.GetAllUsersExceptThis(new SignlarUser { Id = userInfoToken.Id.ToString() });
                if (onlineUsers.Count() > 0)
                {

                    if (user != null)
                    {
                        await hubContext.Clients.AllExcept(new List<string> { user.ConnectionId }).SendNotificationFolderChange(category.ParentId);
                    }
                    else
                    {
                        await hubContext.Clients.All.SendNotificationFolderChange(category.ParentId);
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SignalR Error");
            }

            //if (documentVersion.Extension.ToLower() == ".docx" || documentVersion.Extension.ToLower() == ".docx" || documentVersion.Extension.ToLower() == "doc" || documentVersion.Extension.ToLower() == "docx")
            //{
            //    var convertDocToPDFCommand = new ConvertDocToPDFCommand
            //    {
            //        DocumentId = documentVersion.Id
            //    };
            //    await mediator.Send(convertDocToPDFCommand);
            //}

            return ServiceResponse<DocumentChunkStatus>.ReturnResultWith200(new DocumentChunkStatus { DocumentId = document.Id, Status = request.status });
        }
        else
        {
            if (documentChunks.Count() > 0)
            {
                foreach (var documentChunk in documentChunks)
                {
                    var storeageSetting = await _storageSettingRepository.GetStorageSettingByIdOrLocal(document.StorageSettingId);
                    var storageService = _storageServiceFactory.GetStorageService(storeageSetting.StorageType);
                    await storageService.DeleteFileAsync(documentChunk.Url, storeageSetting.JsonValue);

                }
                documentChunkRepository.RemoveRange(documentChunks);
            }

            var documentAutitTrails = await _documentAuditTrailRepository.All.Where(c => c.DocumentId == document.Id).ToListAsync();
            if (documentAutitTrails.Count > 0)
            {
                _documentAuditTrailRepository.RemoveRange(documentAutitTrails);
            }
            documentRepository.Remove(document);
            documentVersionRepository.Remove(documentVersion);
            if (await uow.SaveAsync() <= 0)
            {
                return ServiceResponse<DocumentChunkStatus>.ReturnFailed(500, "Error While Added Document");
            }
            try
            {
                var user = connectionMappingRepository.GetUserInfoById(userInfoToken.Id);
                if (user != null)
                {
                    await hubContext.Clients.AllExcept(new List<string> { user.ConnectionId }).RefreshDocuments(document.CategoryId);
                }
                else
                {
                    await hubContext.Clients.All.RefreshDocuments(document.CategoryId);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SignalR Error");
            }
            return ServiceResponse<DocumentChunkStatus>.ReturnFailed(500, "Upload failed: Some parts of the document were not uploaded successfully. The document has been deleted. Please try uploading it again.");
        }

    }
}

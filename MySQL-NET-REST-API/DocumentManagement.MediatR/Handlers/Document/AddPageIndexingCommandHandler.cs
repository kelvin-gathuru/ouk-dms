using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Handlers.StorageStategies;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;

public class AddPageIndexingCommandHandler(
    IDocumentRepository _documentRepository,
    Helper.PathHelper pathHelper,
         ILogger<RemovePageIndexingCommandHandler> _logger,
         IDocumentIndexRepository documentIndexRepository,
         IDocumentVersionRepository documentVersionRepository,
        IUnitOfWork<DocumentContext> uow,
        IStorageSettingRepository _storageSettingRepository,
        StorageServiceFactory _storeageServiceFactory,
        IWebHostEnvironment _webHostEnvironment,
        IDocumentChunkRepository documentChunkRepository) : IRequestHandler<AddPageIndexingCommand, ServiceResponse<bool>>
{
    public async Task<ServiceResponse<bool>> Handle(AddPageIndexingCommand request, CancellationToken cancellationToken)
    {
        var entityExist = await _documentRepository.All.Where(c => c.Id == request.DocumentId).FirstOrDefaultAsync();
        if (entityExist == null)
        {
            return ServiceResponse<bool>.ReturnFailed(404, "Document is not found");
        }
        var documentVersion = documentVersionRepository.All.Where(c => c.DocumentId == entityExist.Id && c.IsCurrentVersion).FirstOrDefault();
        if (documentVersion == null)
        {
            return ServiceResponse<bool>.ReturnFailed(404, "Document version is not found");
        }
        try
        {
            var storeageSetting = await _storageSettingRepository.GetStorageSettingByIdOrLocal(entityExist.StorageSettingId);
            if (storeageSetting == null)
            {
                return ServiceResponse<bool>.ReturnResultWith200(true);
            }
            var storageService = _storeageServiceFactory.GetStorageService(storeageSetting.StorageType);
            byte[] responseBytes;
            if (documentVersion.IsChunk)
            {
                responseBytes = await CombineChunkBytes(documentVersion, entityExist, storeageSetting, storageService);
            }
            else
            {
                var fileData = await storageService.DownloadFileAsync(documentVersion.Url, storeageSetting.JsonValue, documentVersion.Key, documentVersion.IV);
                responseBytes = fileData.FileBytes;
            }

            var imagessupport = pathHelper.IMAGESSUPPORT;
            try
            {
                string extension = Path.GetExtension(entityExist.Url);
                string tessdataPath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, pathHelper.TESSDATA);
                var extractor = ContentExtractorFactory.GetExtractor(extension, _webHostEnvironment.WebRootPath);
                if (extractor != null)
                {
                    documentIndexRepository.Add(new Data.Entities.DocumentIndex
                    {
                        DocumentVersionId = documentVersion.Id,
                        CreatedDate = DateTime.UtcNow
                    });
                }
                else if (Array.Exists(imagessupport, element => element.ToLower() == extension.ToLower()))
                {
                    documentIndexRepository.Add(new Data.Entities.DocumentIndex
                    {
                        DocumentVersionId = documentVersion.Id,
                        CreatedDate = DateTime.UtcNow
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while indexing document");
            }
            entityExist.IsAddedPageIndxing = true;
            _documentRepository.Update(entityExist);
            if (await uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.ReturnFailed(500, "Error while removing document from index");
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while removing document from index");
            return ServiceResponse<bool>.ReturnFailed(404, "Error while removing document from index");
        }
    }

    private async Task<byte[]> CombineChunkBytes(DocumentVersion documentVersion, Document document, StorageSetting storeageSetting, IStorageService storageService)
    {
        var documentChunks = await documentChunkRepository.All.Where(c => c.DocumentVersionId == documentVersion.Id).OrderBy(c => c.ChunkIndex).ToListAsync();
        var lstBytes = new List<byte[]>();
        foreach (var chunk in documentChunks)
        {
            var fileResult = await storageService.DownloadFileAsync(chunk.Url, storeageSetting.JsonValue, document.Key, document.IV);
            lstBytes.Add(fileResult.FileBytes);
        }
        using (var finalStream = new MemoryStream())
        {
            foreach (var chunk in lstBytes)
            {
                finalStream.Write(chunk, 0, chunk.Length);
            }
            return finalStream.ToArray();
        }
    }
}

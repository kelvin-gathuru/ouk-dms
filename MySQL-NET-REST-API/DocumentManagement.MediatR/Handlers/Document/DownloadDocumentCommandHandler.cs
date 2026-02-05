using DocumentManagement.Data;
using DocumentManagement.Data.Entities;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Handlers.StorageStategies;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class DownloadDocumentCommandHandler(
        IDocumentRepository _documentRepository,
        IDocumentVersionRepository _versionRepository,
         StorageServiceFactory _storeageServiceFactory,
         IStorageSettingRepository _storageSettingRepository,
         IDocumentChunkRepository documentChunkRepository)
        : IRequestHandler<DownloadDocumentCommand, ServiceResponse<DocumentDownload>>
    {


        public async Task<ServiceResponse<DocumentDownload>> Handle(DownloadDocumentCommand request, CancellationToken cancellationToken)
        {

            var documentVersion = await _versionRepository.All.Where(c => (c.DocumentId == request.Id && c.IsCurrentVersion) || c.Id == request.Id).FirstOrDefaultAsync();
            if (documentVersion == null)
            {

                return ServiceResponse<DocumentDownload>.ReturnFailed(404, "Document version is not found");
            }
            var document = await _documentRepository.All.FirstOrDefaultAsync(c => c.Id == documentVersion.DocumentId);
            if (document == null)
            {
                return ServiceResponse<DocumentDownload>.ReturnFailed(404, "Document  is not found");
            }
            if (documentVersion.IsChunk)
            {
                var bytes = await CombineChunkBytes(documentVersion, document);

                DocumentDownload documentDownload = new DocumentDownload
                {
                    Data = bytes,
                    ContentType = FileHelper.GetMimeTypeByExtension(documentVersion.Extension),
                    FileName = document.Url,
                    DocumentNumber = document.DocumentNumber
                };
                return ServiceResponse<DocumentDownload>.ReturnResultWith200(documentDownload);
            }
            else
            {

                var storeageSetting = await _storageSettingRepository.GetStorageSettingByIdOrLocal(document.StorageSettingId);

                if (storeageSetting == null)
                {
                    return ServiceResponse<DocumentDownload>.ReturnFailed(404, "Storage setting not found");
                }

                var storageService = _storeageServiceFactory.GetStorageService(storeageSetting.StorageType);

                var fileResult = await storageService.DownloadFileAsync(documentVersion.Url, storeageSetting.JsonValue, documentVersion.Key, documentVersion.IV);

                if (string.IsNullOrWhiteSpace(fileResult.ErrorMessage))
                {
                    DocumentDownload documentDownload = new DocumentDownload
                    {
                        Data = fileResult.FileBytes,
                        ContentType = FileHelper.GetMimeTypeByExtension(documentVersion.Extension),
                        FileName = document.Url,
                        DocumentNumber = document.DocumentNumber
                    };
                    return ServiceResponse<DocumentDownload>.ReturnResultWith200(documentDownload);
                }

                return ServiceResponse<DocumentDownload>.ReturnFailed(400, fileResult.ErrorMessage);
            }
        }
        private async Task<byte[]> CombineChunkBytes(DocumentVersion documentVersion, Document document)
        {
            var storeageSetting = await _storageSettingRepository.GetStorageSettingByIdOrLocal(document.StorageSettingId);

            if (storeageSetting == null)
            {
                return null;
            }

            var storageService = _storeageServiceFactory.GetStorageService(storeageSetting.StorageType);
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
}

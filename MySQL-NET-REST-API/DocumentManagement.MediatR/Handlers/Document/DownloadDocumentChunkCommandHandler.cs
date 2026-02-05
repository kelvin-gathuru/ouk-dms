using DocumentManagement.Data;
using DocumentManagement.Data.Entities;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Handlers.StorageStategies;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class DownloadDocumentChunkCommandHandler(
        IDocumentVersionRepository documentVersionRepository,
        IDocumentRepository documentRepository,
        IDocumentChunkRepository documentChunkRepository,
        IStorageSettingRepository _storageSettingRepository,
        IMediator mediator,
         StorageServiceFactory _storeageServiceFactory) : IRequestHandler<DownloadDocumentChunkCommand, ServiceResponse<DocumentDownload>>
    {
        public async Task<ServiceResponse<DocumentDownload>> Handle(DownloadDocumentChunkCommand request, CancellationToken cancellationToken)
        {
            DocumentVersion documentVersion;
            if (request.DocumentVersionId != Guid.Empty && request.DocumentVersionId != null)
            {
                documentVersion = await documentVersionRepository.All.Where(c => c.Id == request.DocumentVersionId).FirstOrDefaultAsync();
            }
            else
            {
                documentVersion = documentVersionRepository.All.Where(c => (c.DocumentId == request.Id && c.IsCurrentVersion) || c.Id == request.Id).FirstOrDefault();
            }


            if (documentVersion == null)
            {
                return ServiceResponse<DocumentDownload>.ReturnFailed(404, "Document version Is not found");
            }
            var document = documentRepository.All.Where(c => c.Id == documentVersion.DocumentId).FirstOrDefault();
            if (documentVersion.IsChunk)
            {
                var bytes = await CombineChunkBytes(documentVersion, document);

                DocumentDownload documentDownload = new DocumentDownload
                {
                    Data = bytes,
                    ContentType = FileHelper.GetMimeTypeByExtension(documentVersion.Extension),
                    FileName = document.Url
                };
                return ServiceResponse<DocumentDownload>.ReturnResultWith200(documentDownload);
            }
            else
            {
                var commnad = new DownloadDocumentCommand
                {
                    Id = documentVersion.Id,
                    IsVersion = false
                };
                var downloadDocument = await mediator.Send(commnad);
                return downloadDocument;
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

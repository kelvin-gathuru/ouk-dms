using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Handlers.StorageStategies;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetFileChunkCommandHandler(
        IDocumentChunkRepository documentChunkRepository, 
        IDocumentRepository documentRepository,
        IDocumentVersionRepository documentVersionRepository,
         StorageServiceFactory _storeageServiceFactory,
       IStorageSettingRepository storageSettingRepository) : IRequestHandler<GetFileChunkCommand, ServiceResponse<DocumentChunkDownload>>
    {
        public async Task<ServiceResponse<DocumentChunkDownload>> Handle(GetFileChunkCommand request, CancellationToken cancellationToken)
        {
            var documentChunk = await documentChunkRepository.All.Where(c => c.DocumentVersionId == request.DocumentVersionId && c.ChunkIndex == request.ChunkIndex).FirstOrDefaultAsync();
            if (documentChunk == null)
            {
                return ServiceResponse<DocumentChunkDownload>.ReturnFailed(404, "Document Chunk is not found");
            }
            var documentVersion = await documentVersionRepository.All.Where(c => c.Id == request.DocumentVersionId).FirstOrDefaultAsync();
            if (documentVersion == null)
            {
                return ServiceResponse<DocumentChunkDownload>.ReturnFailed(404, "Document Version is not found");
            }

            var document = documentRepository.Find(documentVersion.DocumentId);
            if (document == null)
            {
                return ServiceResponse<DocumentChunkDownload>.ReturnFailed(404, "Document is not found");
            }
        

            var storeageSetting = await storageSettingRepository.GetStorageSettingByIdOrLocal(document.StorageSettingId);

            if (storeageSetting == null)
            {
                return ServiceResponse<DocumentChunkDownload>.ReturnFailed(404, "Storage setting not found");
            }
            var storageService = _storeageServiceFactory.GetStorageService(storeageSetting.StorageType);
            var fileResult = await storageService.DownloadFileAsync(documentChunk.Url, storeageSetting.JsonValue,  document.Key,  document.IV);
            if (string.IsNullOrWhiteSpace(fileResult.ErrorMessage))
            {
                var extension = documentChunk.Extension;
                if (!extension.StartsWith("."))
                   extension = "." + extension;
                DocumentChunkDownload documentDownload = new DocumentChunkDownload
                {
                    Data = Convert.ToBase64String( fileResult.FileBytes),
                    ContentType = FileHelper.GetMimeTypeByExtension(extension),
                    FileName = documentChunk.Url,
                    ChunkIndex = documentChunk.ChunkIndex
                };
                return ServiceResponse<DocumentChunkDownload>.ReturnResultWith200(documentDownload);
            }
            return ServiceResponse<DocumentChunkDownload>.ReturnFailed(500, fileResult.ErrorMessage); ;
        }
    }
}

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DocumentManagement.Data;
using DocumentManagement.Data.Entities;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Handlers.StorageStategies;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.MediatR.Handlers
{
    public class DownloadSharedDocumentCommandHandler(
        IMediator mediator,
        IDocumentRepository documentRepository,
        IDocumentVersionRepository documentVersionRepository,
          IDocumentChunkRepository documentChunkRepository,
       IStorageSettingRepository _storageSettingRepository,
         StorageServiceFactory _storeageServiceFactory)
        : IRequestHandler<DownloadSharedDocumentCommand, ServiceResponse<DocumentDownload>>
    {
        public async Task<ServiceResponse<DocumentDownload>> Handle(DownloadSharedDocumentCommand request, CancellationToken cancellationToken)
        {

            var command = new GetLinkInfoByCodeQuery { Code = request.Code };
            var result = await mediator.Send(command);

            if (!result.Success || result.Data == null)
            {
                return ServiceResponse<DocumentDownload>.ReturnFailed(404, result.Errors);
            }

            var link = result.Data;
            if (link.IsLinkExpired)
            {
                return ServiceResponse<DocumentDownload>.ReturnFailed(404, "Link is expired");
            }

            if (link.HasPassword)
            {
                if (link.Password != request.Password)
                {
                    return ServiceResponse<DocumentDownload>.ReturnFailed(404, "Invalid password");
                }
            }
            var documentVersion = documentVersionRepository.All.Where(c => (c.DocumentId == link.DocumentId && c.IsCurrentVersion) || c.Id == link.DocumentId).FirstOrDefault();

            var document = documentRepository.All.Where(c => c.Id == documentVersion.DocumentId).FirstOrDefault();

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
                var commnad = new DownloadDocumentCommand
                {
                    Id = link.DocumentId,
                    IsVersion = false,
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


using AutoMapper;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Handlers.StorageStategies;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;

public class ConvertDocToPDFCommandHandler(
    IDocumentRepository _documentRepository,
    IDocumentVersionRepository documentVersionRepository,
    IMapper _mapper,
    UserInfoToken _userInfoToken,
    PathHelper _pathHelper,
    IWebHostEnvironment _webHostEnvironment,
    IStorageSettingRepository _storageSettingRepository,
     StorageServiceFactory _storeageServiceFactory,
   IDocumentChunkRepository documentChunkRepository) : IRequestHandler<ConvertDocToPDFCommand, bool>
{
    public async Task<bool> Handle(ConvertDocToPDFCommand request, CancellationToken cancellationToken)
    {
        var documentVersion = documentVersionRepository.All.Where(c => (c.DocumentId == request.DocumentId && c.IsCurrentVersion) || c.Id == request.DocumentId).FirstOrDefault();
        if (documentVersion == null)
        {
            return false;
        }
        var document = _documentRepository.All.Where(c => c.Id == documentVersion.DocumentId).FirstOrDefault();
        if (document == null)
        {
            return false;
        }
        byte[] docBytes = null;
        var storeageSetting = await _storageSettingRepository.GetStorageSettingByIdOrLocal(document.StorageSettingId);
        if (storeageSetting == null)
        {
            return false;
        }

        if (documentVersion.IsChunk)
        {
            docBytes = await CombineChunkBytes(documentVersion, document, storeageSetting);
        }
        else
        {
            docBytes = await DownloadPlainFileBytes(documentVersion, document, storeageSetting);
        }
        if (docBytes != null)
        {
            var libreOfficeService = new LibreOfficeService();
            var pdfBytes = await libreOfficeService.ConvertDocToPdfAsync(docBytes, documentVersion.Url, _pathHelper.libreOfficePath);
            if (pdfBytes != null)
            {
                var storageService = _storeageServiceFactory.GetStorageService(storeageSetting.StorageType);
                var fileName = Path.GetFileNameWithoutExtension(documentVersion.Url) + ".pdf";

                var fileNameKeyValut = await storageService.UploadBytesAsync(pdfBytes, storeageSetting, ".pdf");
            }
        }
        return true;
    }

    private async Task<byte[]> CombineChunkBytes(DocumentVersion documentVersion, Document document, StorageSetting storeageSetting)
    {
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
    private async Task<byte[]> DownloadPlainFileBytes(DocumentVersion documentVersion, Document document, StorageSetting storeageSetting)
    {
        var storageService = _storeageServiceFactory.GetStorageService(storeageSetting.StorageType);
        var fileResult = await storageService.DownloadFileAsync(documentVersion.Url, storeageSetting.JsonValue, document.Key, document.IV);
        return fileResult.FileBytes;

    }
}

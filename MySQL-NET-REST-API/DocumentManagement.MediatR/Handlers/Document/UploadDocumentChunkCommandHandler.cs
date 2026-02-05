using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Handlers.StorageStategies;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;

public class UploadDocumentChunkCommandHandler(
    IDocumentRepository documentRepository,
  IStorageSettingRepository _storageSettingRepository,
   StorageServiceFactory _storeageServiceFactory,
   IDocumentChunkRepository documentChunkRepository,
    IUnitOfWork<DocumentContext> _uow,
   IDocumentVersionRepository documentVersionRepository,
    IMapper _mapper) : IRequestHandler<UploadDocumentChunkCommand, ServiceResponse<DocumentChunkDto>>
{
    public async Task<ServiceResponse<DocumentChunkDto>> Handle(UploadDocumentChunkCommand request, CancellationToken cancellationToken)
    {
        if (request.File == null)
        {
            return ServiceResponse<DocumentChunkDto>.ReturnFailed(409, "Please select the file.");
        }
        if (request.ChunkIndex == 0 && !FileSignatureHelper.IsFileSignatureValid(request.File, request.Extension))
        {
            return ServiceResponse<DocumentChunkDto>.ReturnFailed(409, "Invalid file signature.");
        }
        var documentVersion = await documentVersionRepository.All.Where(c => c.Id == request.DocumentVersionId).FirstOrDefaultAsync();
        if (documentVersion == null)
        {
            return ServiceResponse<DocumentChunkDto>.ReturnFailed(404, "Document Version not found.");
        }
        var document = documentRepository.Find(documentVersion.DocumentId);
        if (document == null)
        {
            return ServiceResponse<DocumentChunkDto>.ReturnFailed(404, "Document not found.");
        }

        var storeageSetting = await _storageSettingRepository.GetStorageSettingByIdOrLocal(document.StorageSettingId);

        var storageService = _storeageServiceFactory.GetStorageService(storeageSetting.StorageType);

        var fileNameKeyValut = await storageService.UploadFileChunkAsync(request.File, storeageSetting, documentVersion.Extension, documentVersion.Key, documentVersion.IV);

        if (string.IsNullOrEmpty(fileNameKeyValut.FileName))
        {
            return ServiceResponse<DocumentChunkDto>.Return422("Settings are not properly setup.");
        }
        var entity = new DocumentChunk
        {
            Id = Guid.NewGuid(),
            ChunkIndex = request.ChunkIndex,
            DocumentVersionId = documentVersion.Id,
            Extension = request.Extension,
            Size = request.Size,
            Url = fileNameKeyValut.FileName,
            TotalChunk = request.TotalChunks
        };
        documentChunkRepository.Add(entity);
        if (await _uow.SaveAsync() <= 0)
        {
            return ServiceResponse<DocumentChunkDto>.ReturnFailed(500, "Error While Added Document");
        }
        var entityDto = _mapper.Map<DocumentChunkDto>(entity);
        entityDto.DocumentId = document.Id;
        return ServiceResponse<DocumentChunkDto>.ReturnResultWith200(entityDto);
    }
}

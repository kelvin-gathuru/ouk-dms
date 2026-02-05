using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;

public class RestoreDocumentVersionCommandHandler(
    IDocumentVersionRepository _documentVersionRepository,
        IDocumentRepository _documentRepository,
        IUnitOfWork<DocumentContext> _uow,
        IDocumentIndexRepository _documentIndexRepository,
        UserInfoToken _userInfoToken,
        IDocumentChunkRepository _documentChunkRepository
    ) : IRequestHandler<RestoreDocumentVersionCommand, ServiceResponse<bool>>
{

    public async Task<ServiceResponse<bool>> Handle(RestoreDocumentVersionCommand request, CancellationToken cancellationToken)
    {
        var document = await _documentRepository.FindAsync(request.DocumentId);
        if (document == null)
        {
            return ServiceResponse<bool>.Return404();
        }

        var currentActiveDocument = _documentVersionRepository.All.FirstOrDefault(c => c.DocumentId == request.DocumentId && c.IsCurrentVersion);
        currentActiveDocument.IsCurrentVersion = false;
        _documentVersionRepository.Update(currentActiveDocument);

        var originalPath = document.Url;
        var version = _documentVersionRepository
            .All.FirstOrDefault(c => c.Id == request.Id && c.DocumentId == request.DocumentId);
        if (version == null)
        {
            return ServiceResponse<bool>.Return404();
        }

        var versionId = Guid.NewGuid();
        _documentVersionRepository.Add(new DocumentVersion
        {
            Id = versionId,
            DocumentId = document.Id,
            VersionNumber = await _documentVersionRepository.GetDocumentVersionCount(document.Id) + 1,
            Url = version.Url,
            Key = version.Key,
            IV = version.IV,
            SignById = version.SignById,
            SignDate = version.SignDate,
            IsChunk = version.IsChunk,
            IsAllChunkUploaded = version.IsAllChunkUploaded,
            Comment = version.Comment,
            Extension = version.Extension,
            IsCurrentVersion = true,
            CreatedBy = _userInfoToken.Id,
            CreatedDate = DateTime.UtcNow,
            ModifiedBy = _userInfoToken.Id,
            ModifiedDate = DateTime.UtcNow
        });
        document.Url = version.Url;
        document.Key = version.Key;
        document.IV = version.IV;
        document.CreatedBy = _userInfoToken.Id;
        document.CreatedDate = version.CreatedDate;
        document.ModifiedDate = version.ModifiedDate;
        document.ModifiedBy = version.ModifiedBy;
        document.Extension = version.Extension;
        document.IsChunk = version.IsChunk;
        document.IsAllChunkUploaded = version.IsAllChunkUploaded;
        document.IsAddedPageIndxing = false;
        if (Guid.Empty != version.SignById)
        {
            document.IsSignatureExists = true;
        }
        document.SignById = version.SignById;
        document.SignDate = version.SignDate;

        if (version.IsChunk)
        {
            var documentNewChunks = new List<DocumentChunk>();
            var documentChunks = _documentChunkRepository.All.Where(c => c.DocumentVersionId == request.Id).OrderBy(c => c.ChunkIndex).ToList();
            foreach (var chunk in documentChunks)
            {
                var documentChunk = new DocumentChunk
                {
                    Id = Guid.NewGuid(),
                    DocumentVersionId = versionId,
                    ChunkIndex = chunk.ChunkIndex,
                    Url = chunk.Url,
                    Size = chunk.Size,
                    Extension = chunk.Extension
                };
                documentNewChunks.Add(documentChunk);
            }
            if (documentNewChunks.Count > 0)
                _documentChunkRepository.AddRange(documentNewChunks);
        }

        _documentIndexRepository.Add(new Data.Entities.DocumentIndex
        {
            DocumentVersionId = versionId,
            CreatedDate = DateTime.UtcNow
        });
        _documentRepository.Update(document);
        if (await _uow.SaveAsync() <= 0)
        {
            return ServiceResponse<bool>.Return500();
        }

        return ServiceResponse<bool>.ReturnResultWith200(true); ;
    }
}

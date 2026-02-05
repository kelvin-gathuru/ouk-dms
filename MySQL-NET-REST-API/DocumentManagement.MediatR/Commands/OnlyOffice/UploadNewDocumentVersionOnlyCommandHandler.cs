using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Handlers;
using DocumentManagement.MediatR.Handlers.StorageStategies;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Commands;
public class UploadNewDocumentVersionOnlyCommandHandler(
    IDocumentRepository _documentRepository,
    IDocumentVersionRepository _documentVersionRepository,
    IUnitOfWork<DocumentContext> _uow,
    IMapper _mapper,
    ILogger<UploadNewDocumentVersionCommandHandler> _logger,
    UserInfoToken _userInfoToken,
    StorageServiceFactory _storeageServiceFactory,
    IStorageSettingRepository _storageSettingRepository,
    IDocumentIndexRepository _documentIndexRepository,
    IDocumentAuditTrailRepository _documentAuditTrailRepository) : IRequestHandler<UploadNewDocumentVersionOnlyCommand, ServiceResponse<DocumentVersionDto>>
{
    public async Task<ServiceResponse<DocumentVersionDto>> Handle(UploadNewDocumentVersionOnlyCommand request, CancellationToken cancellationToken)
    {
        if (request.File == null)
        {
            return ServiceResponse<DocumentVersionDto>.ReturnFailed(409, "Please select the file.");
        }

        var doc = await _documentRepository.FindAsync(request.DocumentId);
        if (doc == null)
        {
            _logger.LogError("Document Not Found");
            return ServiceResponse<DocumentVersionDto>.Return500();
        }
        var currentDocumentVersion = await _documentVersionRepository.All.Where(c => c.DocumentId == request.DocumentId && c.IsCurrentVersion).AsNoTracking().FirstOrDefaultAsync();
        if (currentDocumentVersion != null)
        {
            _documentVersionRepository.UpdateDetachedAndAttached(currentDocumentVersion);
        }
        var storeageSetting = await _storageSettingRepository.GetStorageSettingByIdOrLocal(doc.StorageSettingId);

        var storageService = _storeageServiceFactory.GetStorageService(storeageSetting.StorageType);

        var fileNameKeyValut = await storageService.UploadBytesAsync(request.File, storeageSetting, doc.Extension);

        if (string.IsNullOrEmpty(fileNameKeyValut.FileName))
        {
            return ServiceResponse<DocumentVersionDto>.Return422("Settings are not properly setup.");
        }

        var version = new DocumentVersion
        {
            DocumentId = doc.Id,
            Url = fileNameKeyValut.FileName,
            Key = fileNameKeyValut.Key,
            IV = fileNameKeyValut.IV,
            IsCurrentVersion = true,
            VersionNumber = await _documentVersionRepository.GetDocumentVersionCount(doc.Id) + 1,
            CreatedBy = _userInfoToken.Id,
            CreatedDate = DateTime.UtcNow,
            ModifiedBy = _userInfoToken.Id,
            ModifiedDate = DateTime.UtcNow,
            Comment = doc.Comment,
            Extension = doc.Extension,
            IsChunk = false,
            IsAllChunkUploaded = true,
            SignById = doc.SignById,
            SignDate = doc.SignDate

        };
        doc.Url = fileNameKeyValut.FileName;
        doc.Key = fileNameKeyValut.Key;
        doc.IV = fileNameKeyValut.IV;
        doc.CreatedDate = DateTime.UtcNow;
        doc.CreatedBy = _userInfoToken.Id;
        doc.IsSignatureExists = doc.IsSignatureExists;
        doc.IsChunk = false;
        doc.IsAllChunkUploaded = true;
        doc.Extension = doc.Extension;
        DocumentAuditTrail documentAudit;
        if (doc.IsSignatureExists)
        {
            doc.SignById = _userInfoToken.Id;
            doc.SignDate = DateTime.UtcNow;
            documentAudit = new DocumentAuditTrail()
            {
                DocumentId = doc.Id,
                CreatedBy = _userInfoToken.Id,
                CreatedDate = DateTime.UtcNow,
                OperationName = DocumentOperation.Added_Signature,
                AssignToUserId = _userInfoToken.Id
            };
        }
        else
        {
            documentAudit = new DocumentAuditTrail()
            {
                DocumentId = doc.Id,
                CreatedBy = _userInfoToken.Id,
                CreatedDate = DateTime.UtcNow,
                OperationName = DocumentOperation.Added_Version,
                AssignToUserId = _userInfoToken.Id
            };
        }
        _documentRepository.Update(doc);

        currentDocumentVersion.IsCurrentVersion = false;
        _uow.Context.Attach(currentDocumentVersion).State = EntityState.Modified;
        _documentVersionRepository.Add(version);

        _documentIndexRepository.Add(new DocumentIndex { Id = Guid.NewGuid(), DocumentVersionId = version.Id });

        _documentAuditTrailRepository.Add(documentAudit);

        if (await _uow.SaveAsync() <= 0)
        {
            _logger.LogError("Error while adding industry");
            return ServiceResponse<DocumentVersionDto>.Return500();
        }
        var documentCommentDto = _mapper.Map<DocumentVersionDto>(version);
        documentCommentDto.CategoryId = doc.CategoryId;
        return ServiceResponse<DocumentVersionDto>.ReturnResultWith200(documentCommentDto);
    }

}



using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DocumentManagement.MediatR.Handlers;

public class UploadNewDocumentChunkVersionCommandHandler : IRequestHandler<UploadNewDocumentChunkVersionCommand, ServiceResponse<DocumentVersionDto>>
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IDocumentVersionRepository _documentVersionRepository;
    private readonly IUnitOfWork<DocumentContext> _uow;
    private readonly IMapper _mapper;
    private readonly ILogger<UploadNewDocumentVersionCommandHandler> _logger;
    private readonly UserInfoToken _userInfoToken;
    private readonly IDocumentIndexRepository _documentIndexRepository;
    private readonly IDocumentAuditTrailRepository _documentAuditTrailRepository;

    public UploadNewDocumentChunkVersionCommandHandler(
        IDocumentRepository documentRepository,
        IDocumentVersionRepository documentVersionRepository,
        IUnitOfWork<DocumentContext> uow,
        IMapper mapper,
        ILogger<UploadNewDocumentVersionCommandHandler> logger,
        UserInfoToken userInfoToken,
        IDocumentIndexRepository documentIndexRepository,
        IDocumentAuditTrailRepository documentAuditTrailRepository
        )
    {
        _documentRepository = documentRepository;
        _documentVersionRepository = documentVersionRepository;
        _uow = uow;
        _mapper = mapper;
        _logger = logger;
        _userInfoToken = userInfoToken;
        _documentIndexRepository = documentIndexRepository;
        _documentAuditTrailRepository = documentAuditTrailRepository;
    }
    public async Task<ServiceResponse<DocumentVersionDto>> Handle(UploadNewDocumentChunkVersionCommand request, CancellationToken cancellationToken)
    {

        var doc = await _documentRepository.FindAsync(request.DocumentId);
        if (doc == null)
        {
            _logger.LogError("Document Not Found");
            return ServiceResponse<DocumentVersionDto>.Return500();
        }
        var currentDocumentVersion = await _documentVersionRepository.All.Where(c => c.DocumentId == request.DocumentId && c.IsCurrentVersion).FirstOrDefaultAsync();
        currentDocumentVersion.IsCurrentVersion = false;
        var url = Guid.NewGuid() + "." + request.Extension;

        var version = new DocumentVersion
        {
            DocumentId = doc.Id,
            Url = url,
            Key = doc.Key,
            IV = doc.IV,
            IsCurrentVersion = true,
            VersionNumber = await _documentVersionRepository.GetDocumentVersionCount(doc.Id) + 1,
            CreatedBy = _userInfoToken.Id,
            CreatedDate = DateTime.UtcNow,
            ModifiedBy = _userInfoToken.Id,
            ModifiedDate = DateTime.UtcNow,
            Comment = request.Comment,
            Extension = request.Extension,
            IsChunk = true,
            IsAllChunkUploaded = false,
            SignById = request.isSignatureExists ? _userInfoToken.Id : null,
            SignDate = request.isSignatureExists ? DateTime.UtcNow : null,
        };

        doc.ModifiedBy = _userInfoToken.Id;
        doc.ModifiedDate = DateTime.UtcNow;
        doc.IsSignatureExists = request.isSignatureExists;
        doc.IsChunk = true;
        doc.IsAllChunkUploaded = false;
        doc.SignById = request.isSignatureExists ? _userInfoToken.Id : null;
        doc.SignDate = request.isSignatureExists ? DateTime.UtcNow : null;
        doc.Url = url;
        doc.Extension = request.Extension;
        doc.Comment = request.Comment;

        var documentAudit = new DocumentAuditTrail()
        {
            DocumentId = doc.Id,
            CreatedBy = _userInfoToken.Id,
            CreatedDate = DateTime.UtcNow,
            OperationName = DocumentOperation.Added_Version,
            AssignToUserId = _userInfoToken.Id
        };

        _documentRepository.Update(doc);
        _documentVersionRepository.Update(currentDocumentVersion);
        _documentVersionRepository.Add(version);
        _documentIndexRepository.Add(new DocumentIndex { Id = Guid.NewGuid(), DocumentVersionId = version.Id });

        _documentAuditTrailRepository.Add(documentAudit);

        if (await _uow.SaveAsync() <= 0)
        {
            _logger.LogError("Error while adding industry");
            return ServiceResponse<DocumentVersionDto>.Return500();
        }
        var documentCommentDto = _mapper.Map<DocumentVersionDto>(version);
        return ServiceResponse<DocumentVersionDto>.ReturnResultWith200(documentCommentDto);
    }

}

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
using DocumentManagement.MediatR.Handlers.StorageStategies;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DocumentManagement.MediatR.Handlers;

public class UploadNewDocumentVersionCommandHandler : IRequestHandler<UploadNewDocumentVersionCommand, ServiceResponse<DocumentVersionDto>>
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IDocumentVersionRepository _documentVersionRepository;
    private readonly IUnitOfWork<DocumentContext> _uow;
    private readonly IMapper _mapper;
    private readonly ILogger<UploadNewDocumentVersionCommandHandler> _logger;
    private readonly UserInfoToken _userInfoToken;
    private readonly Helper.PathHelper _pathHelper;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly StorageServiceFactory _storeageServiceFactory;
    private readonly IStorageSettingRepository _storageSettingRepository;
    private readonly IMediator _mediator;
    private readonly IDocumentIndexRepository _documentIndexRepository;
    private readonly IDocumentAuditTrailRepository _documentAuditTrailRepository;

    public UploadNewDocumentVersionCommandHandler(
        IDocumentRepository documentRepository,
        IDocumentVersionRepository documentVersionRepository,
        IUnitOfWork<DocumentContext> uow,
        IMapper mapper,
        ILogger<UploadNewDocumentVersionCommandHandler> logger,
        UserInfoToken userInfoToken,
        Helper.PathHelper pathHelper,
        IWebHostEnvironment webHostEnvironment,
        StorageServiceFactory storeageServiceFactory,
        IStorageSettingRepository storageSettingRepository,
        IMediator mediator,
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
        _pathHelper = pathHelper;
        _webHostEnvironment = webHostEnvironment;
        _storeageServiceFactory = storeageServiceFactory;
        _storageSettingRepository = storageSettingRepository;
        _mediator = mediator;
        _documentIndexRepository = documentIndexRepository;
        _documentAuditTrailRepository = documentAuditTrailRepository;
    }
    public async Task<ServiceResponse<DocumentVersionDto>> Handle(UploadNewDocumentVersionCommand request, CancellationToken cancellationToken)
    {
        if (request.File == null)
        {
            return ServiceResponse<DocumentVersionDto>.ReturnFailed(409, "Please select the file.");
        }
        if (!FileSignatureHelper.IsFileSignatureValid(request.File))
        {
            return ServiceResponse<DocumentVersionDto>.ReturnFailed(409, "Invalid file signature.");
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

        var fileNameKeyValut = await storageService.UploadFileAsync(request.File, storeageSetting, request.Extension);

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
            Extension = request.Extension,
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
        doc.IsSignatureExists = request.IsSignatureExists;
        doc.IsChunk = false;
        doc.IsAllChunkUploaded = true;
        doc.Extension = request.Extension;
        DocumentAuditTrail documentAudit;
        if (request.IsSignatureExists)
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

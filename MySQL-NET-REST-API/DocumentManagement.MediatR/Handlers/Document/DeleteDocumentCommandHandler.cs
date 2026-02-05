using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Handlers.LuceneHandler;
using DocumentManagement.MediatR.Handlers.StorageStategies;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DocumentManagement.MediatR.Handlers;

public class DeleteDocumentCommandHandler(
    IStorageSettingRepository _storageSettingRepository,
    StorageServiceFactory _storageServiceFactory,
    IDocumentRepository _documentRepository,
    IUnitOfWork<DocumentContext> _uow,
    PathHelper _pathHelper,
    IWebHostEnvironment _webHostEnvironment,
    IDocumentAuditTrailRepository _documentAuditTrailRepository,
    UserInfoToken userInfoToken,
    IDocumentVersionRepository _documentVersionRepository,
    ILogger<DeleteDocumentCommandHandler> _logger) : IRequestHandler<DeleteDocumentCommand, DocumentDto>
{
    public async Task<DocumentDto> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
    {
        var entityExist = await _documentRepository.All.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.Id == request.Id);
        if (entityExist == null)
        {
            var errorDto = new DocumentDto
            {
                StatusCode = 404,
                Messages = new List<string> { "Not Found" }
            };
            return errorDto;
        }
        entityExist.IsArchive = true;
        entityExist.IsDeleted = true;
        entityExist.DeletedBy = userInfoToken.Id;
        entityExist.DeletedDate = DateTime.UtcNow;
        if (request.IsRetention)
        {
            entityExist.RetentionDate = null;
        }
        _documentRepository.Update(entityExist);
        var documentAuditCreated = new DocumentAuditTrail()
        {
            DocumentId = entityExist.Id,
            CreatedBy = entityExist.Id,
            CreatedDate = DateTime.UtcNow,
            OperationName = DocumentOperation.Deleted
        };
        _documentAuditTrailRepository.Add(documentAuditCreated);
        //_documentRepository.Delete(request.Id);
        if (await _uow.SaveAsync() <= -1)
        {
            var errorDto = new DocumentDto
            {
                StatusCode = 500,
                Messages = new List<string> { "An unexpected fault happened. Try again later." }
            };
            return errorDto;
        }
        var storeageSetting = await _storageSettingRepository.GetStorageSettingByIdOrLocal(entityExist.StorageSettingId);
        var storageService = _storageServiceFactory.GetStorageService(storeageSetting.StorageType);
        await storageService.DeleteFileAsync(entityExist.Url, storeageSetting.JsonValue);
        try
        {
            var documentVersions = await _documentVersionRepository.All.Where(c => c.DocumentId == entityExist.Id).ToListAsync();
            foreach (var documentVersion in documentVersions)
            {
                string searchIndexPath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, _pathHelper.SearchIndexPath);
                var indexService = new IndexDeleteManager(searchIndexPath);
                indexService.DeleteDocumentById(documentVersion.Id.ToString());
                indexService.Dispose();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting document from index");
        }

        return new DocumentDto
        {
            StatusCode = 200,
            Messages = new List<string> { "Document deleted successfully." }
        };
    }
}

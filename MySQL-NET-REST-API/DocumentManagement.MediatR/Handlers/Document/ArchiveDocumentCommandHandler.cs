
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Handlers.LuceneHandler;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;

public class ArchiveDocumentCommandHandler(
    IDocumentRepository _documentRepository,
    IWorkflowInstanceRepository _workflowInstanceRepository,
    IUnitOfWork<DocumentContext> _uow,
    PathHelper _pathHelper,
    IWebHostEnvironment _webHostEnvironment,
     IHubContext<UserHub, IHubClient> hubContext,
    IConnectionMappingRepository connectionMappingRepository,
    UserInfoToken _userInfoToken,
    ILogger<DeleteDocumentCommandHandler> _logger,
    IDocumentVersionRepository documentVersionRepository) : IRequestHandler<ArchiveDocumentCommand, DocumentDto>
{
    public async Task<DocumentDto> Handle(ArchiveDocumentCommand request, CancellationToken cancellationToken)
    {
        var entityExist = await _documentRepository.FindAsync(request.DocumentId);
        if (entityExist == null)
        {
            var errorDto = new DocumentDto
            {
                StatusCode = 404,
                Messages = new List<string> { "Not Found" }
            };
            return errorDto;
        }
        await _workflowInstanceRepository.CancelWorkflowInstancesAsync(entityExist.Id);
        entityExist.IsArchive = true;
        entityExist.ArchiveById = _userInfoToken.Id;
        if (request.IsRetention)
        {
            entityExist.RetentionDate = null;
        }
        _documentRepository.Update(entityExist);
        var documentVersions = await documentVersionRepository.All.Where(c => c.DocumentId == entityExist.Id).ToListAsync();
        if (await _uow.SaveAsync() <= -1)
        {
            var errorDto = new DocumentDto
            {
                StatusCode = 500,
                Messages = new List<string> { "An unexpected fault happened. Try again later." }
            };
            return errorDto;
        }
        var onlineUsers = connectionMappingRepository.GetAllUsersExceptThis(new SignlarUser { Id = _userInfoToken.Id.ToString() });
        if (onlineUsers.Count() > 0)
        {
            await hubContext.Clients.All.ArchieveRestoreFolder(entityExist.CategoryId);
        }
        try
        {
            if (documentVersions != null && documentVersions.Count() > 0)
            {
                foreach (var documentVerion in documentVersions)
                {
                    string searchIndexPath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, _pathHelper.SearchIndexPath);
                    var indexService = new IndexDeleteManager(searchIndexPath);
                    indexService.DeleteDocumentById(documentVerion.Id.ToString());
                    indexService.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting document from index");
        }
        try
        {
            var user = connectionMappingRepository.GetUserInfoById(_userInfoToken.Id);
            if (user != null)
            {
                await hubContext.Clients.AllExcept(new List<string> { user.ConnectionId }).RefreshDocuments(entityExist.CategoryId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting document from index");
        }

        return new DocumentDto
        {
            StatusCode = 200,
            Messages = new List<string> { "Document Archived successfully." }
        };
    }
}

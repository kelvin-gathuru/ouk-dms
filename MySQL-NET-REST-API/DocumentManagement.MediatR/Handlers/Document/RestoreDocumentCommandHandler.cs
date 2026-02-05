using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DocumentManagement.MediatR.Handlers;

public class RestoreDocumentCommandHandler(IDocumentRepository _documentRepository,
    IDocumentVersionRepository _documentVersionRepository,
    IUnitOfWork<DocumentContext> _uow,
    IDocumentIndexRepository _documentIndexRepository,
    IDocumentAuditTrailRepository _documentAuditTrailRepository,
    IHubContext<UserHub, IHubClient> hubContext,
    ILogger<DeleteDocumentCommandHandler> _logger,
     IConnectionMappingRepository connectionMappingRepository,
    UserInfoToken userInfoToken,
     ICategoryRepository categoryRepository
   ) : IRequestHandler<RestoreDocumentCommand, DocumentDto>
{
    public async Task<DocumentDto> Handle(RestoreDocumentCommand request, CancellationToken cancellationToken)
    {
        var entityExist = await _documentRepository.FindAsync(request.DocumentId);
        try
        {

            if (entityExist == null)
            {
                var errorDto = new DocumentDto
                {
                    StatusCode = 404,
                    Messages = new List<string> { "Not Found" }
                };
                return errorDto;
            }
            entityExist.IsArchive = false;
            entityExist.ArchiveById = null;
            var documentVersion = await _documentVersionRepository.All.Where(c => c.DocumentId == entityExist.Id && c.IsCurrentVersion).FirstOrDefaultAsync();
            _documentRepository.Update(entityExist);
            var documentAuditCreated = new DocumentAuditTrail()
            {
                DocumentId = entityExist.Id,
                CreatedBy = entityExist.Id,
                CreatedDate = DateTime.UtcNow,
                OperationName = DocumentOperation.Restored
            };
            _documentAuditTrailRepository.Add(documentAuditCreated);
            _documentIndexRepository.Add(new DocumentIndex
            {
                DocumentVersionId = documentVersion.Id,
                CreatedDate = DateTime.UtcNow
            });
            if (await _uow.SaveAsync() <= -1)
            {
                var errorDto = new DocumentDto
                {
                    StatusCode = 500,
                    Messages = new List<string> { "An unexpected fault happened. Try again later." }
                };
                return errorDto;
            }
            var onlineUsers = connectionMappingRepository.GetAllUsersExceptThis(new SignlarUser { Id = userInfoToken.Id.ToString() });
            if (onlineUsers.Count() > 0)
            {
                var user = connectionMappingRepository.GetUserInfoById(userInfoToken.Id);
                if (user != null)
                {
                    await hubContext.Clients.AllExcept(new List<string> { user.ConnectionId }).ArchieveRestoreFolder(entityExist.CategoryId);
                }
                else
                {
                    await hubContext.Clients.All.ArchieveRestoreFolder(entityExist.CategoryId);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting document from index");
        }
        try
        {

            var user = connectionMappingRepository.GetUserInfoById(userInfoToken.Id);
            if (user != null)
            {
                await hubContext.Clients.AllExcept(new List<string> { user.ConnectionId }).RefreshDocuments(entityExist.CategoryId);
            }
            var category = categoryRepository.All.Where(c => c.Id == entityExist.CategoryId).FirstOrDefault();
            var onlineUsers = connectionMappingRepository.GetAllUsersExceptThis(new SignlarUser { Id = userInfoToken.Id.ToString() });
            if (onlineUsers.Count() > 0)
            {
                if (user != null)
                {
                    await hubContext.Clients.AllExcept(new List<string> { user.ConnectionId }).SendNotificationFolderChange(category.ParentId);
                }
                else
                {
                    await hubContext.Clients.All.SendNotificationFolderChange(category.ParentId);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SignalR Error");

        }

        return new DocumentDto
        {
            StatusCode = 200,
            Messages = new List<string> { "Document Restore successfully." }
        };
    }
}

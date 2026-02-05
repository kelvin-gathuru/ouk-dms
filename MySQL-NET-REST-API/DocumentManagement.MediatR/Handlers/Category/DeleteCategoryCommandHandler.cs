using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
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
using Microsoft.Extensions.Logging;

namespace DocumentManagement.MediatR.Handlers;

public class DeleteCategoryCommandHandler(
    ICategoryRepository _categoryRepository,
    IDocumentRepository _documentRepository,
    IUnitOfWork<DocumentContext> _uow,
    IHubContext<UserHub, IHubClient> hubContext,
    UserInfoToken userInfoToken,
    ILogger<DeleteCategoryCommandHandler> _logger,
    IDocumentAuditTrailRepository documentAuditTrailRepository,
    IConnectionMappingRepository connectionMappingRepository) : IRequestHandler<DeleteCategoryCommand, CategoryDto>
{
    public async Task<CategoryDto> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var entityExist = await _categoryRepository.FindAsync(request.Id);

        if (entityExist == null)
        {
            var errorDto = new CategoryDto
            {
                StatusCode = 404,
                Messages = new List<string> { "Not Found" }
            };
            return errorDto;
        }

        var isParentDoc = _categoryRepository.All.Any(c => !c.IsDeleted && c.ParentId == request.Id);

        if (isParentDoc)
        {
            return new CategoryDto
            {
                StatusCode = 422,
                Messages = new List<string> { "A folder is already associated with a child folder. You can not deleted parent folder." }
            };
        }

        var isExistingDoc = _documentRepository.All.Where(c => !c.IsDeleted && c.CategoryId == request.Id).ToList();

        //if (isExistingDoc)
        //{
        //    return new CategoryDto
        //    {
        //        StatusCode = 404,
        //        Messages = new List<string> { "Category can not be deleted. Document is assign to this category." }
        //    };
        //}
        if (isExistingDoc.Count > 0)
        {
            foreach (var item in isExistingDoc)
            {
                _documentRepository.Delete(item);
            }
        }
        _categoryRepository.Delete(request.Id);
        var documentAudit = new DocumentAuditTrail()
        {
            CategoryId = entityExist.Id,
            CreatedBy = userInfoToken.Id,
            CreatedDate = DateTime.UtcNow,
            OperationName = DocumentOperation.Deleted_Folder
        };
        documentAuditTrailRepository.Add(documentAudit);

        if (await _uow.SaveAsync() <= -1)
        {
            var errorDto = new CategoryDto
            {
                StatusCode = 500,
                Messages = new List<string> { "An unexpected fault happened. Try again later." }
            };
            return errorDto;
        }
        try
        {
            var onlineUsers = connectionMappingRepository.GetAllUsers();
            if (onlineUsers.Count() > 0)
            {
                var userInfo = connectionMappingRepository.GetUserInfoById(userInfoToken.Id);
                if (userInfo != null)
                {
                    await hubContext.Clients.AllExcept(new List<string> { userInfo.ConnectionId }).SendNotificationFolderChange(entityExist.ParentId);
                }
                else
                {
                    await hubContext.Clients.All.SendNotificationFolderChange(entityExist.ParentId);
                }
                await hubContext.Clients.All.AddEditFolder(entityExist.ParentId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in sending notification to all users.");
        }

        return new CategoryDto
        {
            StatusCode = 200,
            Messages = new List<string> { "Folder deleted successfully." }
        };
    }
}
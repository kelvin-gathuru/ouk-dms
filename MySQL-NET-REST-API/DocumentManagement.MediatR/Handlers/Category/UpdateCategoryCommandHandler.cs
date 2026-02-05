using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
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

public class UpdateCategoryCommandHandler(
    ICategoryRepository _categoryRepository,
    IUnitOfWork<DocumentContext> _uow,
    IMapper _mapper,
     ILogger<UpdateCategoryCommandHandler> _logger,
    UserInfoToken userInfoToken,
       IHubContext<UserHub, IHubClient> hubContext,
    IConnectionMappingRepository connectionMappingRepository,
    IDocumentAuditTrailRepository documentAuditTrailRepository) : IRequestHandler<UpdateCategoryCommand, CategoryDto>
{
    public async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var entityExist = await _categoryRepository
            .FindBy(c => c.Name == request.Name && c.Id != request.Id && c.ParentId == request.ParentId).FirstOrDefaultAsync();
        if (entityExist != null)
        {
            var errorDto = new CategoryDto
            {
                StatusCode = 409,
                Messages = new List<string> { "Category Name already exist for another category." }
            };
            return errorDto;
        }
        var categoriesChilds = _categoryRepository.GetAllChildrenAsync(request.Id);
        if (request.Id == request.ParentId || categoriesChilds.Any(c => c.Id == request.ParentId))
        {
            var errorDto = new CategoryDto
            {
                StatusCode = 409,
                Messages = new List<string> { "Parent folder cannot be the folder itself or any of its subfolders." }
            };
            return errorDto;
        }

        var entity = _categoryRepository.All.FirstOrDefault(c => c.Id == request.Id);
        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.ParentId = request.ParentId;
        _categoryRepository.Update(entity);

        var documentAuditPermission = new DocumentAuditTrail()
        {
            CategoryId = entity.Id,
            CreatedBy = userInfoToken.Id,
            CreatedDate = DateTime.UtcNow,
            OperationName = DocumentOperation.Added_Folder_Permission
        };
        documentAuditTrailRepository.Add(documentAuditPermission);

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
            var onlineUsers = connectionMappingRepository.GetAllUsersExceptThis(new SignlarUser { Id = userInfoToken.Id.ToString() });
            if (onlineUsers.Count() > 0)
            {
                var userSignInfo = connectionMappingRepository.GetUserInfoById(userInfoToken.Id);
                if (userSignInfo != null)
                {
                    await hubContext.Clients.AllExcept(new List<string> { userSignInfo.ConnectionId }).SendNotificationFolderChange(entity.ParentId);
                }
                else
                {
                    await hubContext.Clients.All.SendNotificationFolderChange(entity.ParentId);
                }
                await hubContext.Clients.All.AddEditFolder(entity.ParentId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SignalR Error");
        }

        var entityDto = _mapper.Map<CategoryDto>(entity);
        return entityDto;
    }
}

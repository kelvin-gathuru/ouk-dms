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
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace DocumentManagement.MediatR.Handlers;
public class RestoreCategoryCommandHandler(
        ICategoryRepository _categoryRepository,
        IUnitOfWork<DocumentContext> _uow,
        IMapper mapper,
        UserInfoToken userInfoToken,
         IHubContext<UserHub, IHubClient> hubContext,
         IDocumentAuditTrailRepository documentAuditTrailRepository,
    IConnectionMappingRepository connectionMappingRepository,
     ILogger<RestoreCategoryCommandHandler> logger) : IRequestHandler<RestoreCategoryCommand, ServiceResponse<CategoryDto>>
{
    public async Task<ServiceResponse<CategoryDto>> Handle(RestoreCategoryCommand request, CancellationToken cancellationToken)
    {
        var entityExist = await _categoryRepository.FindAsync(request.CategoryId);
        if (entityExist == null)
        {
            return ServiceResponse<CategoryDto>.ReturnFailed(409, "Category already exist.");
        }
        if (entityExist.ArchiveParentId != null)
        {
            return ServiceResponse<CategoryDto>.ReturnFailed(409, "Parent folder is archive. You can't restore child folder.");
        }
        entityExist.IsArchive = false;
        entityExist.ArchiveById = null;
        entityExist.ArchiveParentId = null;
        _categoryRepository.Update(entityExist);
        var documentAuditCreated = new DocumentAuditTrail()
        {
            CategoryId = entityExist.Id,
            CreatedBy = entityExist.Id,
            CreatedDate = DateTime.UtcNow,
            OperationName = DocumentOperation.Restored_Folder
        };
        documentAuditTrailRepository.Add(documentAuditCreated);

        var childs = _categoryRepository.GetAllChildCategoryIdsUsingRawSql(request.CategoryId);
        if (childs.Count > 0)
        {
            var lstChildCategory = new List<Data.Entities.Category>();
            foreach (var child in childs)
            {
                var childEntity = await _categoryRepository.FindAsync(child);
                if (childEntity != null)
                {
                    childEntity.IsArchive = false;
                    childEntity.ArchiveParentId = null;
                    lstChildCategory.Add(childEntity);
                }
            }
            _categoryRepository.UpdateRange(lstChildCategory);
        }
        if (await _uow.SaveAsync() <= -1)
        {

            return ServiceResponse<CategoryDto>.ReturnFailed(500, "An unexpected fault happened. Try again later.");

        }
        try
        {
            var onlineUsers = connectionMappingRepository.GetAllUsersExceptThis(new SignlarUser { Id = userInfoToken.Id.ToString() });
            if (onlineUsers.Count() > 0)
            {
                await hubContext.Clients.All.ArchieveRestoreFolder(entityExist.ParentId);
            }

            var user = connectionMappingRepository.GetUserInfoById(userInfoToken.Id);
            if (user != null)
            {
                await hubContext.Clients.AllExcept(new List<string> { user.ConnectionId }).RefreshDocuments(entityExist.Id);
                await hubContext.Clients.AllExcept(new List<string> { user.ConnectionId }).RestoreFolder(entityExist.Id);
            }
            else
            {
                await hubContext.Clients.All.RefreshDocuments(entityExist.Id);
                await hubContext.Clients.All.RestoreFolder(entityExist.Id);
            }
            await hubContext.Clients.All.AddEditFolder(entityExist.ParentId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "SignalR Error");
        }

        return ServiceResponse<CategoryDto>.ReturnResultWith200(mapper.Map<CategoryDto>(entityExist));
    }
}

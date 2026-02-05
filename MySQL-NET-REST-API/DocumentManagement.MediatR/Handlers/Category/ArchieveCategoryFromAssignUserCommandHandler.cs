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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DocumentManagement.MediatR.Handlers;
public class ArchieveCategoryFromAssignUserCommandHandler(
        ICategoryRepository categoryRepository,
        ICategoryUserPermissionRepository categoryUserPermissionRepository,
        ICategoryRolePermissionRepository categoryRolePermissionRepository,
        IWorkflowInstanceRepository _workflowInstanceRepository,
        IUnitOfWork<DocumentContext> uow,
        IMapper mapper,
        IHubContext<UserHub, IHubClient> hubContext,
        IConnectionMappingRepository connectionMappingRepository,
        UserInfoToken userInfoToken,
        IDocumentAuditTrailRepository documentAuditTrailRepository,
        ILogger<DeleteDocumentCommandHandler> logger) : IRequestHandler<ArchieveCategoryFromAssignUserCommand, ServiceResponse<CategoryDto>>
{
    public async Task<ServiceResponse<CategoryDto>> Handle(ArchieveCategoryFromAssignUserCommand request, CancellationToken cancellationToken)
    {
        var entityExist = await categoryRepository.FindAsync(request.CategoryId);
        if (entityExist == null)
        {
            return ServiceResponse<CategoryDto>.ReturnFailed(409, "Folder does not exist.");
        }

        if (entityExist.CreatedBy != userInfoToken.Id)
        {
            return ServiceResponse<CategoryDto>.ReturnFailed(409, "Folder is created another User.You are not authorized to archive this folder.");
        }
        var categoryRolesPermissions = await categoryRolePermissionRepository.All.Where(c => c.CategoryId == request.CategoryId && c.CreatedBy != userInfoToken.Id).ToListAsync();

        if (categoryRolesPermissions.Any())
        {
            return ServiceResponse<CategoryDto>.ReturnFailed(409, "This folder is shared with a role and shared by another user. You cannot archive it.");
        }

        var categoryUserPermissions = await categoryUserPermissionRepository.All.Where(c => c.CategoryId == request.CategoryId && c.CreatedBy != userInfoToken.Id).ToListAsync();
        if (categoryUserPermissions.Any())
        {
            return ServiceResponse<CategoryDto>.ReturnFailed(409, "This folder is shared with a user and shared by another user. You cannot archive it.");
        }
        var categories = new List<Guid>();
        categories.Add(entityExist.Id);
        entityExist.IsArchive = true;
        entityExist.ArchiveById = userInfoToken.Id;
        entityExist.ArchiveParentId = null;
        categoryRepository.Update(entityExist);

        var documentAudit = new DocumentAuditTrail()
        {
            CategoryId = entityExist.Id,
            CreatedBy = userInfoToken.Id,
            CreatedDate = DateTime.UtcNow,
            OperationName = DocumentOperation.Archived_Folder
        };
        documentAuditTrailRepository.Add(documentAudit);
        var childs = categoryRepository.GetAllChildCategoryIdsUsingRawSql(request.CategoryId);
        if (childs.Count > 0)
        {
            var lstChildCategory = new List<Data.Entities.Category>();
            foreach (var child in childs)
            {
                var childEntity = await categoryRepository.FindAsync(child);
                if (childEntity != null)
                {
                    childEntity.IsArchive = true;
                    childEntity.ArchiveParentId = entityExist.Id;
                    lstChildCategory.Add(childEntity);
                    categories.Add(childEntity.Id);
                }

            }
            categoryRepository.UpdateRange(lstChildCategory);
        }
        await _workflowInstanceRepository.CancelWorkflowInstancesByCategoryAsync(categories);
        if (await uow.SaveAsync() <= -1)
        {
            return ServiceResponse<CategoryDto>.ReturnFailed(409, "An unexpected fault happened. Try again later.");
        }
        try
        {
            var onlineUsers = connectionMappingRepository.GetAllUsersExceptThis(new SignlarUser { Id = userInfoToken.Id.ToString() });
            if (onlineUsers.Count() > 0)
            {
                var user = connectionMappingRepository.GetUserInfoById(userInfoToken.Id);
                if (user != null)
                {
                    await hubContext.Clients.AllExcept(new List<string> { user.ConnectionId }).ArchieveRestoreFolder(entityExist.ParentId);
                }
                else
                {
                    await hubContext.Clients.All.ArchieveRestoreFolder(entityExist.ParentId);
                }
                await hubContext.Clients.All.AddEditFolder(entityExist.ParentId);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "SignalR Error");
        }
        var entityDto = mapper.Map<CategoryDto>(entityExist);
        return ServiceResponse<CategoryDto>.ReturnResultWith200(entityDto);
    }
}

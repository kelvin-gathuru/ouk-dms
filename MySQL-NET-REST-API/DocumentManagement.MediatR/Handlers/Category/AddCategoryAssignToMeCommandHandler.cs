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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;

public class AddCategoryAssignToMeCommandHandler(
    ICategoryRepository _categoryRepository,
    IUnitOfWork<DocumentContext> _uow,
    IMapper _mapper,
    UserInfoToken userInfo,
    ILogger<AddCategoryCommandHandler> _logger,
    ICategoryUserPermissionRepository categoryUserPermissionRepository,
    ICategoryRolePermissionRepository categoryRolePermissionRepository,
    IHubContext<UserHub, IHubClient> hubContext,
     IDocumentAuditTrailRepository documentAuditTrailRepository,
    IConnectionMappingRepository connectionMappingRepository) : IRequestHandler<AddCategoryAssignToMeCommand, CategoryDto>
{
    public async Task<CategoryDto> Handle(AddCategoryAssignToMeCommand request, CancellationToken cancellationToken)
    {
        var entityExist = await _categoryRepository.FindBy(c => c.Name == request.Name && c.ParentId == request.ParentId).FirstOrDefaultAsync();
        if (entityExist != null)
        {
            _logger.LogError("Category name already exist.");
            var errorDto = new CategoryDto
            {
                StatusCode = 409,
                Messages = new List<string> { "Category name already exist." }
            };
            return errorDto;
        }
        var entity = _mapper.Map<Data.Entities.Category>(request);
        entity.Id = Guid.NewGuid();
        _categoryRepository.Add(entity);
        var documentAuditCreated = new DocumentAuditTrail()
        {
            CategoryId = entity.Id,
            CreatedBy = userInfo.Id,
            CreatedDate = DateTime.UtcNow,
            OperationName = DocumentOperation.Added_Folder
        };
        documentAuditTrailRepository.Add(documentAuditCreated);

        var documentAudit = new DocumentAuditTrail()
        {
            CategoryId = entity.Id,
            CreatedBy = userInfo.Id,
            CreatedDate = DateTime.UtcNow,
            OperationName = DocumentOperation.Added_Folder_Permission,
            AssignToUserId = userInfo.Id
        };
        documentAuditTrailRepository.Add(documentAudit);

        if (request.ParentId != null)
        {

            var parentUserPermissions = await categoryUserPermissionRepository.All.Where(c => c.CategoryId == request.ParentId).ToListAsync();

            var parentRolePermissions = await categoryRolePermissionRepository.All.Where(c => c.CategoryId == request.ParentId).ToListAsync();

            if (parentUserPermissions.Count > 0)
            {
                var lstUserPermission = new List<CategoryUserPermission>();
                foreach (var parentUserPermission in parentUserPermissions)
                {
                    var categoryUserPermission = new CategoryUserPermission
                    {
                        Id = Guid.NewGuid(),
                        CategoryId = entity.Id,
                        UserId = parentUserPermission.UserId,
                        ParentId = parentUserPermission.ParentId != null ? parentUserPermission.ParentId : parentUserPermission.Id,
                        StartDate = null,
                        EndDate = null,
                        IsTimeBound = false,
                        IsAllowDownload = false
                    };

                    lstUserPermission.Add(categoryUserPermission);

                    var documentAuditUserPermission = new DocumentAuditTrail()
                    {
                        CategoryId = entity.Id,
                        CreatedBy = userInfo.Id,
                        CreatedDate = DateTime.UtcNow,
                        OperationName = DocumentOperation.Added_Folder_Permission,
                        AssignToUserId = parentUserPermission.UserId
                    };
                    documentAuditTrailRepository.Add(documentAuditUserPermission);

                }
                categoryUserPermissionRepository.AddRange(lstUserPermission);
            }
            if (parentRolePermissions.Count > 0)
            {
                var lstRolePermission = new List<CategoryRolePermission>();
                foreach (var parentRolePermission in parentRolePermissions)
                {
                    var categoryRolePermission = new CategoryRolePermission
                    {
                        Id = Guid.NewGuid(),
                        CategoryId = entity.Id,
                        RoleId = parentRolePermission.RoleId,
                        ParentId = parentRolePermission.ParentId != null ? parentRolePermission.ParentId : parentRolePermission.Id,
                        StartDate = null,
                        EndDate = null,
                        IsTimeBound = false,
                        IsAllowDownload = false
                    };
                    lstRolePermission.Add(categoryRolePermission);
                    var documentAuditUserPermission = new DocumentAuditTrail()
                    {
                        CategoryId = entity.Id,
                        CreatedBy = userInfo.Id,
                        CreatedDate = DateTime.UtcNow,
                        OperationName = DocumentOperation.Added_Folder_Permission,
                        AssignToRoleId = parentRolePermission.RoleId
                    };
                    documentAuditTrailRepository.Add(documentAuditUserPermission);
                }
                categoryRolePermissionRepository.AddRange(lstRolePermission);
            }

        }
        else
        {
            var categoryUserPermission = new CategoryUserPermission
            {
                Id = Guid.NewGuid(),
                CategoryId = entity.Id,
                UserId = userInfo.Id,
                ParentId = null,
                StartDate = null,
                EndDate = null,
                IsTimeBound = false,
                IsAllowDownload = false
            };
            categoryUserPermissionRepository.Add(categoryUserPermission);

            var documentAuditUserPermission = new DocumentAuditTrail()
            {
                CategoryId = entity.Id,
                CreatedBy = userInfo.Id,
                CreatedDate = DateTime.UtcNow,
                OperationName = DocumentOperation.Added_Folder_Permission,
                AssignToUserId = userInfo.Id
            };
            documentAuditTrailRepository.Add(documentAuditUserPermission);
        }

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
            var onlineUsers = connectionMappingRepository.GetAllUsersExceptThis(new SignlarUser { Id = userInfo.Id.ToString() });
            if (onlineUsers.Count() > 0)
            {

                var userSignInfo = connectionMappingRepository.GetUserInfoById(userInfo.Id);
                if (userSignInfo != null)
                {
                    await hubContext.Clients.AllExcept(new List<string> { userSignInfo.ConnectionId }).SharedFolder(entity.ParentId);
                }
                else
                {
                    await hubContext.Clients.All.SharedFolder(entity.ParentId);
                }
                await hubContext.Clients.All.AddEditFolder(entity.ParentId);
                await hubContext.Clients.AllExcept(new List<string> { userSignInfo.ConnectionId }).AddedNewFolder(entity.Id);
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


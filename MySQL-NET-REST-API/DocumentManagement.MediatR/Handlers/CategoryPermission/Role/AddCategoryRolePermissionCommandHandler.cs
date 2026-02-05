using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace DocumentManagement.MediatR.Handlers;

public class AddCategoryRolePermissionCommandHandler(
      ICategoryRolePermissionRepository categoryRolePermissionRepository,
        IUnitOfWork<DocumentContext> uow,
        IDocumentRepository documentRepository,
        IUserNotificationRepository userNotificationRepository,
        IMapper mapper,
        IDocumentAuditTrailRepository documentAuditTrailRepository,
        UserInfoToken userInfo,
        ICategoryRepository categoryRepository,
        IUserRepository userRepository,
        ISendEmailRepository sendEmailRepository,
        IHubContext<UserHub, IHubClient> hubContext,
        IConnectionMappingRepository connectionMappingRepository,
           ILogger<AddCategoryRolePermissionCommandHandler> _logger
    ) : IRequestHandler<AddCategoryRolePermissionCommand, CategoryRolePermissionDto>
{
    public async Task<CategoryRolePermissionDto> Handle(AddCategoryRolePermissionCommand request, CancellationToken cancellationToken)
    {
        var permissions = mapper.Map<List<CategoryRolePermission>>(request.CategoryRolePermissions);
        var roleIds = permissions.Select(p => p.RoleId).Distinct().ToList();
        var categoryIds = permissions.Select(p => p.CategoryId).Distinct().ToList();

        var existingPermissions = categoryRolePermissionRepository.All
            .Where(p => categoryIds.Contains(p.CategoryId) && roleIds.Contains(p.RoleId))
            .ToList();



        foreach (var permission in permissions)
        {
            var existingPermission = existingPermissions
                .FirstOrDefault(p => p.CategoryId == permission.CategoryId && p.RoleId == permission.RoleId);

            if (existingPermission != null)
            {
                categoryRolePermissionRepository.Remove(existingPermission);
            }
            permission.Id = Guid.NewGuid();
            permission.ParentId = null;

            if (permission.IsTimeBound)
            {
                permission.EndDate = permission.EndDate.Value.AddDays(1).AddSeconds(-1);
            }
        }
        categoryRolePermissionRepository.AddRange(permissions);

        var childPermissions = new List<CategoryRolePermission>();
        foreach (var permission in permissions)
        {
            var childCategoryIds = categoryRepository.GetAllChildCategoryIdsUsingRawSql(permission.CategoryId);
            foreach (var childId in childCategoryIds)
            {
                var existingChildPermission = existingPermissions
                    .FirstOrDefault(p => p.CategoryId == childId && p.RoleId == permission.RoleId);

                if (existingChildPermission != null)
                {
                    categoryRolePermissionRepository.Remove(existingChildPermission);
                }

                var childPermission = new CategoryRolePermission
                {
                    Id = Guid.NewGuid(),
                    CategoryId = childId,
                    RoleId = permission.RoleId,
                    IsTimeBound = permission.IsTimeBound,
                    StartDate = permission.StartDate,
                    EndDate = permission.EndDate,
                    ParentId = permission.Id
                };

                childPermissions.Add(childPermission);
            }
            childCategoryIds.Add(categoryIds.FirstOrDefault());
            await documentRepository.UpdateIsSharedByCategoryIdAsync(childCategoryIds, true);
        }

        if (childPermissions.Any())
        {
            categoryRolePermissionRepository.AddRange(childPermissions);
        }

        var categoryId = request.CategoryRolePermissions.FirstOrDefault()?.CategoryId ?? Guid.Empty;
        var categoryInfo = await categoryRepository.FindAsync(categoryId);
        var currentUserInfo = await userRepository.FindAsync(userInfo.Id);
        var sharedByUserName = $"{currentUserInfo.FirstName} {currentUserInfo.LastName}";

        var lstDocumentAuditTrail = roleIds.Select(roleId => new DocumentAuditTrail
        {
            CategoryId = categoryId,
            CreatedBy = userInfo.Id,
            CreatedDate = DateTime.UtcNow,
            OperationName = DocumentOperation.Added_Folder_Permission,
            AssignToRoleId = roleId
        }).ToList();

        if (lstDocumentAuditTrail.Any())
        {
            documentAuditTrailRepository.AddRange(lstDocumentAuditTrail);
        }

        var users = await userNotificationRepository.CreateRolesCategoryNotifiction(roleIds, categoryInfo.Id, sharedByUserName, categoryInfo.Name);
        var userIds = users.Select(u => u.Id).ToList();

        if (request.CategoryRolePermissions.Any(p => p.IsAllowEmailNotification) && userIds.Any())
        {
            foreach (var user in users)
            {
                sendEmailRepository.AddSharedFolderEmails(new SendEmail
                {
                    Email = user.Email,
                    FromEmail = currentUserInfo.Email,
                    FromName = sharedByUserName,
                    ToName = $"{user.FirstName} {user.LastName}",
                    CreatedBy = userInfo.Id,
                    CreatedDate = DateTime.UtcNow
                }, categoryInfo.Name);
            }
        }

        if (await uow.SaveAsync() <= -1)
        {
            return new CategoryRolePermissionDto
            {
                StatusCode = 500,
                Messages = new List<string> { "An unexpected fault happened. Try again later." }
            };
        }

        try
        {
            await userNotificationRepository.SendNotification(userIds);

            var onlineUsers = connectionMappingRepository.GetAllUsersExceptThis(new SignlarUser { Id = userInfo.Id.ToString() });

            if (onlineUsers.Any())
            {
                var signalrUserInfo = connectionMappingRepository.GetUserInfoById(currentUserInfo.Id);
                if (signalrUserInfo != null)
                {
                    await hubContext.Clients.AllExcept(new List<string> { signalrUserInfo.ConnectionId }).SharedFolder(categoryInfo.ParentId);
                }
                else
                {
                    await hubContext.Clients.All.SharedFolder(categoryInfo.ParentId);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in sending notification to all users.");
        }

        return new CategoryRolePermissionDto();
    }

}

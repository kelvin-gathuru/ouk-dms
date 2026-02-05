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

public class AddCategoryUserPermissionCommandHandler(
        ICategoryUserPermissionRepository categoryUserPermissionRepository,
        IDocumentRepository documentRepository,
        IUnitOfWork<DocumentContext> uow,
        IUserNotificationRepository userNotificationRepository,
        IMapper mapper,
        IDocumentAuditTrailRepository documentAuditTrailRepository,
        UserInfoToken userInfo,
        ICategoryRepository categoryRepository,
        IUserRepository userRepository,
        ISendEmailRepository sendEmailRepository,
        IHubContext<UserHub, IHubClient> hubContext,
        IConnectionMappingRepository connectionMappingRepository,
          ILogger<AddCategoryUserPermissionCommandHandler> _logger
    ) : IRequestHandler<AddCategoryUserPermissionCommand, CategoryUserPermissionDto>
{

    public async Task<CategoryUserPermissionDto> Handle(AddCategoryUserPermissionCommand request, CancellationToken cancellationToken)
    {
        var permissions = mapper.Map<List<CategoryUserPermission>>(request.CategoryUserPermissions);
        var lstSendEmail = new List<SendEmail>();

        var categoryIds = permissions.Select(p => p.CategoryId).Distinct().ToList();
        var userIds = permissions.Select(p => p.UserId).Distinct().ToList();

        var existingPermissions = categoryUserPermissionRepository.All
            .Where(p => categoryIds.Contains(p.CategoryId) && userIds.Contains(p.UserId))
            .ToList();

        foreach (var permission in permissions)
        {
            var existingPermission = existingPermissions
                .FirstOrDefault(p => p.CategoryId == permission.CategoryId && p.UserId == permission.UserId);

            if (existingPermission != null)
            {
                categoryUserPermissionRepository.Remove(existingPermission);
            }

            permission.Id = Guid.NewGuid();
            permission.ParentId = null;

            if (permission.IsTimeBound)
            {
                permission.EndDate = permission.EndDate.Value.AddDays(1).AddSeconds(-1);
            }
        }

        categoryUserPermissionRepository.AddRange(permissions);

        var childPermissions = new List<CategoryUserPermission>();

        foreach (var permission in permissions)
        {
            var childCategoryIds = categoryRepository.GetAllChildCategoryIdsUsingRawSql(permission.CategoryId);
            foreach (var childId in childCategoryIds)
            {
                var existingChildPermission = existingPermissions
                    .FirstOrDefault(p => p.CategoryId == childId && p.UserId == permission.UserId);

                if (existingChildPermission != null)
                {
                    categoryUserPermissionRepository.Remove(existingChildPermission);
                }

                var childPermission = new CategoryUserPermission
                {
                    Id = Guid.NewGuid(),
                    CategoryId = childId,
                    UserId = permission.UserId,
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
            categoryUserPermissionRepository.AddRange(childPermissions);
        }

        var categoryId = request.CategoryUserPermissions.First().CategoryId;
        var category = categoryRepository.Find(categoryId.Value);

        var lstDocumentAuditTrail = userIds.Select(userId => new DocumentAuditTrail
        {
            CategoryId = categoryId,
            CreatedBy = userInfo.Id,
            CreatedDate = DateTime.UtcNow,
            OperationName = DocumentOperation.Added_Folder_Permission,
            AssignToUserId = userId
        }).ToList();

        var currentUserInfo = await userRepository.FindAsync(userInfo.Id);

        if (request.CategoryUserPermissions.Any(d => d.IsAllowEmailNotification == true))
        {
            var users = await userRepository.GetUsersByIds(userIds);
            foreach (var user in users)
            {
                sendEmailRepository.AddSharedFolderEmails(new SendEmail
                {
                    Email = user.Email,
                    FromEmail = currentUserInfo.Email,
                    FromName = $"{currentUserInfo.FirstName} {currentUserInfo.LastName}",
                    ToName = $"{user.FirstName} {user.LastName}",
                    CreatedBy = userInfo.Id,
                    CreatedDate = DateTime.UtcNow,
                }, category.Name);
            }
        }

        if (lstDocumentAuditTrail.Any())
        {
            documentAuditTrailRepository.AddRange(lstDocumentAuditTrail);
        }

        var sharedByUserName = $"{currentUserInfo.FirstName} {currentUserInfo.LastName}";
        userNotificationRepository.CreateUsersCategoryNotifiction(userIds, category.Id, sharedByUserName, category.Name);

        if (await uow.SaveAsync() <= -1)
        {
            return new CategoryUserPermissionDto
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
                var userSignalrInfo = connectionMappingRepository.GetUserInfoById(currentUserInfo.Id);
                if (userSignalrInfo != null)
                {
                    await hubContext.Clients.AllExcept(new List<string> { userSignalrInfo.ConnectionId }).SendNotificationFolderChange(category.ParentId);
                }
                else
                {
                    await hubContext.Clients.All.SendNotificationFolderChange(category.ParentId);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in sending notification to all users.");
        }

        return new CategoryUserPermissionDto();
    }

}

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

public class CategoryPermissionUserRoleCommandHandler(
        ICategoryRolePermissionRepository categoryRolePermissionRepository,
        IUnitOfWork<DocumentContext> uow,
        IUserNotificationRepository userNotificationRepository,
        IMapper mapper,
        IDocumentAuditTrailRepository documentAuditTrailRepository,
        UserInfoToken userInfo,
        ICategoryUserPermissionRepository categoryUserPermissionRepository,
        IUserRepository userRepository,
        ICategoryRepository categoryRepository,
        ISendEmailRepository sendEmailRepository,
        IHubContext<UserHub, IHubClient> hubContext,
         ILogger<CategoryPermissionUserRoleCommandHandler> _logger,
        IConnectionMappingRepository connectionMappingRepository) : IRequestHandler<CategoryPermissionUserRoleCommand, bool>
{
    public async Task<bool> Handle(CategoryPermissionUserRoleCommand request, CancellationToken cancellationToken)
    {
        List<DocumentAuditTrail> lstDocumentAuditTrail = new List<DocumentAuditTrail>();

        List<Guid> userIds = new List<Guid>();
        var currentUserInfo = userRepository.Find(userInfo.Id);
        var sharedByUserName = currentUserInfo.FirstName + currentUserInfo.LastName;
        if (request.Roles != null && request.Roles.Count() > 0)
        {
            List<CategoryRolePermission> lstCategoryRolePermission = new List<CategoryRolePermission>();

            foreach (var category in request.Categories)
            {
                foreach (var role in request.Roles)
                {

                    lstCategoryRolePermission.Add(new CategoryRolePermission
                    {
                        CategoryId = Guid.Parse(category),
                        RoleId = Guid.Parse(role),
                        StartDate = request.StartDate,
                        EndDate = request.IsTimeBound ? request.EndDate.Value.AddDays(1).AddSeconds(-1) : request.EndDate,
                        IsTimeBound = request.IsTimeBound,
                        IsAllowDownload = request.IsAllowDownload,
                        CreatedBy = userInfo.Id,
                        CreatedDate = DateTime.UtcNow
                    });

                    lstDocumentAuditTrail.Add(new DocumentAuditTrail()
                    {
                        CategoryId = Guid.Parse(category),
                        CreatedBy = userInfo.Id,
                        CreatedDate = DateTime.UtcNow,
                        OperationName = DocumentOperation.Added_Folder_Permission,
                        AssignToRoleId = Guid.Parse(role)
                    });
                }
                List<Guid> roles = request.Roles.Select(c => Guid.Parse(c)).ToList();
                var categoryInfo = await categoryRepository.FindAsync(Guid.Parse(category));
                var users = await userNotificationRepository.CreateRolesCategoryNotifiction(roles, categoryInfo.Id, sharedByUserName, categoryInfo.Name);
                userIds.AddRange(users.Select(d => d.Id));
                if (request.IsAllowEmailNotification && users.Count() > 0)
                {


                    foreach (var user in users)
                    {
                        sendEmailRepository.AddSharedFolderEmails(new SendEmail
                        {
                            Email = user.Email,
                            FromEmail = currentUserInfo.Email,
                            FromName = currentUserInfo.FirstName + ' ' + currentUserInfo.LastName,
                            ToName = user.FirstName + ' ' + user.LastName,
                            CreatedBy = userInfo.Id,
                            CreatedDate = DateTime.UtcNow,
                        }, categoryInfo.Name);
                    }

                }
            }
            categoryRolePermissionRepository.AddRange(lstCategoryRolePermission);
        }

        if (request.Users != null && request.Users.Count() > 0)
        {
            List<CategoryUserPermission> lstCategoryUserPermission = new List<CategoryUserPermission>();

            foreach (var category in request.Categories)
            {
                foreach (var user in request.Users)
                {

                    lstCategoryUserPermission.Add(new CategoryUserPermission
                    {
                        CategoryId = Guid.Parse(category),
                        UserId = Guid.Parse(user),
                        StartDate = request.StartDate,
                        EndDate = request.IsTimeBound ? request.EndDate.Value.AddDays(1).AddSeconds(-1) : request.EndDate,
                        IsTimeBound = request.IsTimeBound,
                        IsAllowDownload = request.IsAllowDownload,
                        CreatedBy = userInfo.Id,
                        CreatedDate = DateTime.UtcNow
                    });

                    lstDocumentAuditTrail.Add(new DocumentAuditTrail()
                    {
                        CategoryId = Guid.Parse(category),
                        CreatedBy = userInfo.Id,
                        CreatedDate = DateTime.UtcNow,
                        OperationName = DocumentOperation.Added_Folder_Permission,
                        AssignToUserId = Guid.Parse(user)
                    });

                }
                var filterUsers = request.Users.Where(c => !userIds.Contains(Guid.Parse(c))).ToList();
                var categoryInfo = await categoryRepository.FindAsync(Guid.Parse(category));
                var users = await userRepository.GetUsersByIds(filterUsers.Select(c => Guid.Parse(c)).ToList());
                if (request.IsAllowEmailNotification && filterUsers.Count() > 0)
                {

                    foreach (var user in users)
                    {
                        sendEmailRepository.AddSharedFolderEmails(new SendEmail
                        {
                            Email = user.Email,
                            FromEmail = currentUserInfo.Email,
                            FromName = currentUserInfo.FirstName + ' ' + currentUserInfo.LastName,
                            ToName = user.FirstName + ' ' + user.LastName,
                            CreatedBy = userInfo.Id,
                            CreatedDate = DateTime.UtcNow,
                        }, categoryInfo.Name);
                    }
                }
                if (users.Count() > 0)
                {
                    await userNotificationRepository.NotifiedUsersPermission(users.Select(c => c.Id).ToList());
                }
                var tempUserIds = request.Users.Select(c => Guid.Parse(c)).ToList();
                userNotificationRepository.CreateUsersCategoryNotifiction(tempUserIds, categoryInfo.Id, sharedByUserName, categoryInfo.Name);
                userIds.AddRange(tempUserIds);
            }
            categoryUserPermissionRepository.AddRange(lstCategoryUserPermission);
        }


        if (lstDocumentAuditTrail.Count() > 0)
        {
            documentAuditTrailRepository.AddRange(lstDocumentAuditTrail);
        }

        if (await uow.SaveAsync() <= -1)
        {
            var errorDto = new CategoryRolePermissionDto
            {
                StatusCode = 500,
                Messages = new List<string> { "An unexpected fault happened. Try again later." }
            };
            return false;
        }
        try
        {
            var onlineUsers = connectionMappingRepository.GetAllUsersExceptThis(new SignlarUser { Id = userInfo.Id.ToString() });
            if (onlineUsers.Count() > 0)
            {
                foreach (var category in request.Categories)
                {

                    var userInfo = connectionMappingRepository.GetUserInfoById(currentUserInfo.Id);
                    if (userInfo != null)
                    {
                        await hubContext.Clients.AllExcept(new List<string> { userInfo.ConnectionId }).SendNotificationFolderChange(Guid.Parse(category));
                    }
                    else
                    {
                        await hubContext.Clients.All.SendNotificationFolderChange(Guid.Parse(category));
                    }
                }
            }

            await userNotificationRepository.SendNotification(userIds);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in sending notification to all users.");
        }

        return true;
    }
}

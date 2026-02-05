using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Resources;
using DocumentManagement.Domain;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.Repository;

public class UserNotificationRepository : GenericRepository<UserNotification, DocumentContext>,
   IUserNotificationRepository
{

    private readonly IPropertyMappingService _propertyMappingService;
    private readonly UserInfoToken _userInfoToken;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IConnectionMappingRepository _userInfoInMemory;
    private readonly IHubContext<UserHub, IHubClient> _hubContext;
    public UserNotificationRepository(
        IDocumentRepository documentRepository,
        IUserRoleRepository userRoleRepository,
        IPropertyMappingService propertyMappingService,
        UserInfoToken userInfoToken,
        IUnitOfWork<DocumentContext> uow,
        IConnectionMappingRepository userInfoInMemory,
         IHubContext<UserHub, IHubClient> hubContext) : base(uow)
    {
        _userRoleRepository = userRoleRepository;
        _propertyMappingService = propertyMappingService;
        _userInfoToken = userInfoToken;
        _userInfoInMemory = userInfoInMemory;
        _hubContext = hubContext;
    }

    public void CreateUsersDocumentNotifiction(List<Guid> userIds, Guid documentId)
    {
        userIds.ForEach(userId =>
        {
            Add(new UserNotification
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                DocumentId = documentId,
                IsRead = false,
                NotificationsType = NotificationsType.SHARE_USER
            });
        });
    }

    public void CreateUsersCategoryNotifiction(List<Guid> userIds, Guid categoryId, string sharedByUserName, string categoryName)
    {
        userIds.ForEach(userId =>
        {
            Add(new UserNotification
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CategoryId = categoryId,
                IsRead = false,
                Message = $"{sharedByUserName} has shared the folders '{categoryName}' with you.",
                NotificationsType = NotificationsType.SHARE_FOLDER
            });
        });
    }

    public void CreateUserNotificationFileRequestDocument(Guid userId, Guid fileRequestDocumentId, string fileRequestSubject, string fileRequestName)
    {
        Add(new UserNotification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            FileRequestDocumentId = fileRequestDocumentId,
            IsRead = false,
            Message = $"Your file request document for the subject \"{fileRequestSubject}\" with the name \"{fileRequestName}\" has been successfully uploaded.",
            NotificationsType = NotificationsType.FILE_REQUEST
        });
    }

    public void AddUserNotificationByReminderScheduler(ReminderScheduler reminderScheduler)
    {
        Add(new UserNotification
        {
            Id = Guid.NewGuid(),
            UserId = reminderScheduler.UserId,
            DocumentId = reminderScheduler.DocumentId,
            Message = reminderScheduler.Subject,
            IsRead = false,
            NotificationsType = NotificationsType.REMINDER
        });
    }


    public async Task<List<User>> CreateRolesDocumentNotifiction(List<Guid> roleIds, Guid documentId)
    {
        var users = await _userRoleRepository.All.Include(c => c.User).Where(cs => roleIds.Contains(cs.RoleId)).Select(c => c.User).Distinct().ToListAsync();
        users.ForEach(user =>
        {
            Add(new UserNotification
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                DocumentId = documentId,
                IsRead = false,
                NotificationsType = NotificationsType.SHARE_USER
            });

        });
        return users;
    }
    public async Task<List<User>> CreateRolesCategoryNotifiction(List<Guid> roleIds, Guid categoryId, string sharedByUserName, string categoryName)
    {
        var users = await _userRoleRepository.All.Include(c => c.User).Where(cs => roleIds.Contains(cs.RoleId)).Select(c => c.User).Distinct().ToListAsync();
        users.ForEach(user =>
        {
            Add(new UserNotification
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                CategoryId = categoryId,
                IsRead = false,
                Message = $"{sharedByUserName} has shared the folders '{categoryName}' with you.",
                NotificationsType = NotificationsType.SHARE_FOLDER
            });

        });
        return users;
    }

    public void createWorkflowInstanceNotifications(List<NotificationWorkflowDto> NotificationWorkflowDtos)
    {
        NotificationWorkflowDtos.ForEach(notificationWorkflow =>
        {
            Add(new UserNotification
            {
                Id = Guid.NewGuid(),
                UserId = notificationWorkflow.UserId,
                DocumentId = notificationWorkflow.DocumentId,
                WorkflowInstanceId = notificationWorkflow.WorkflowInstanceId,
                IsRead = false,
                Message = notificationWorkflow.Message,
                NotificationsType = NotificationsType.WORKFLOW
            });

        });
    }

    public async Task<NotificationList> GetUserNotifications(NotificationResource documentResource)
    {
        var collectionBeforePaging = AllIncluding(d => d.Document)
            .Include(c => c.WorkflowInstance)
                .ThenInclude(c => c.Workflow)
            .Where(c => (!c.DocumentId.HasValue || !c.Document.IsDeleted) && c.UserId == _userInfoToken.Id);
        collectionBeforePaging =
           collectionBeforePaging.ApplySort(documentResource.OrderBy,
           _propertyMappingService.GetPropertyMapping<UserNotificationDto, UserNotification>());

        if (!string.IsNullOrWhiteSpace(documentResource.Name))
        {
            collectionBeforePaging = collectionBeforePaging
                .Where(c => EF.Functions.Like(c.Document.Name, $"%{documentResource.Name}%")
                || EF.Functions.Like(c.Message, $"%{documentResource.Name}%") || EF.Functions.Like(c.WorkflowInstance.Workflow.Name, $"%{documentResource.Name}%"));
        }

        var documentAuditTrailList = new NotificationList();
        return await documentAuditTrailList.Create(
            collectionBeforePaging,
            documentResource.Skip,
            documentResource.PageSize
            );
    }

    public async Task MarkAsRead(Guid notificationId)
    {
        var notification = Find(notificationId);
        if (notification != null)
        {
            notification.IsRead = true;
            Update(notification);
            await _uow.SaveAsync();
        }
    }

    public async Task SendNotification(List<Guid> userIds)
    {
        foreach (var userId in userIds)
        {
            await SendNotification(userId);
        }
    }

    public async Task SendNotification(Guid userId)
    {
        var userInfoReciever = _userInfoInMemory.GetUserInfoById(userId);
        if (userInfoReciever != null)
            await _hubContext.Clients.Client(userInfoReciever.ConnectionId).SendNotification(userId);
    }

    public async Task NotifiedUsersPermission(List<Guid> userIds)
    {
        foreach (var userId in userIds)
        {
            var userInfoReciever = _userInfoInMemory.GetUserInfoById(userId);
            if (userInfoReciever != null)
            {
                await _hubContext.Clients.Client(userInfoReciever.ConnectionId).NotifyUserPermissionChange();
            }
        }
    }

    public async Task MarkAllAsRead()
    {
        var userId = _userInfoToken.Id;
        var notifications = All.Where(c => c.UserId == userId).ToList();
        notifications.ForEach(notification => notification.IsRead = true);
        UpdateRange(notifications);
        await _uow.SaveAsync();
    }

    public async Task MarkAsReadByDocumentId(Guid documentId)
    {
        var userId = _userInfoToken.Id;
        var notifications = All.Where(c => c.DocumentId == documentId && c.UserId == userId).ToList();
        notifications.ForEach(c => c.IsRead = true);
        UpdateRange(notifications);
        await _uow.SaveAsync();
    }
}

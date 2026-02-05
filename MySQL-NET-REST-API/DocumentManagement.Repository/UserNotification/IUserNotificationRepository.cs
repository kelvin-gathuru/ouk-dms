using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DocumentManagement.Repository;

public interface IUserNotificationRepository : IGenericRepository<UserNotification>
{
    void CreateUsersDocumentNotifiction(List<Guid> userIds, Guid documentId);
    Task<List<User>> CreateRolesDocumentNotifiction(List<Guid> roleIds, Guid documentId);
    Task<NotificationList> GetUserNotifications(NotificationResource documentResource);
    Task MarkAsRead(Guid notificationId);
    Task MarkAsReadByDocumentId(Guid documentId);
    Task MarkAllAsRead();
    void AddUserNotificationByReminderScheduler(ReminderScheduler reminderScheduler);
    Task SendNotification(Guid userId);
    Task SendNotification(List<Guid> userIds);
    void createWorkflowInstanceNotifications(List<NotificationWorkflowDto> NotificationWorkflowDtos);
    void CreateUserNotificationFileRequestDocument(Guid userId, Guid fileRequestDocumentId, string fileRequestSubject, string fileRequestName);

    void CreateUsersCategoryNotifiction(List<Guid> userIds, Guid categoryId, string sharedByUserName, string categoryName);
    Task<List<User>> CreateRolesCategoryNotifiction(List<Guid> roleIds, Guid categoryId, string sharedByUserName, string categoryName);
    Task NotifiedUsersPermission(List<Guid> userIds);
}

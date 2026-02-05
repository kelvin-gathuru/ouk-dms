using DocumentManagement.Data.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DocumentManagement.Repository;

public interface IHubClient
{
    Task UserLeft(string id);

    Task NewOnlineUser(SignlarUser userInfo);

    Task Joined(SignlarUser userInfo);

    Task OnlineUsers(IEnumerable<SignlarUser> userInfo);

    Task Logout(SignlarUser userInfo);

    Task ForceLogout(SignlarUser userInfo);

    Task SendDM(string message, SignlarUser userInfo);

    Task SendNotification(Guid userId);

    Task RefreshDocuments(Guid? categoryId);

    Task RefreshWorkflows();

    Task RefreshWorkflowSettings();

    Task NotifyUserPermissionChange();
    Task SendNotificationFolderChange(Guid? categoryId);
    Task ArchieveRestoreFolder(Guid? categoryId);
    Task SendAiPromptResponse(Guid msgId, string msg);
    Task SharedFolder(Guid? categoryId);
    Task SharedDocument(Guid? categoryId);
    Task AddEditFolder(Guid? categoryId);
    Task AddedNewFolder(Guid? categoryId);
    Task DeleteFolder(Guid? categoryId);
    Task RestoreFolder(Guid? categoryId);
    Task UpdateUserPermission(Guid? userId);

}

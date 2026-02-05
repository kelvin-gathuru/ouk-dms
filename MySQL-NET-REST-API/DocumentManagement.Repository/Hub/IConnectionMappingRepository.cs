using DocumentManagement.Data.Dto;
using System;
using System.Collections.Generic;

namespace DocumentManagement.Repository;

public interface IConnectionMappingRepository
{
    bool AddUpdate(SignlarUser tempUserInfo, string connectionId);
    void Remove(SignlarUser tempUserInfo);
    IEnumerable<SignlarUser> GetAllUsersExceptThis(SignlarUser tempUserInfo);
    SignlarUser GetUserInfo(SignlarUser tempUserInfo);
    SignlarUser GetUserInfoByName(string id);
    SignlarUser GetUserInfoById(Guid userId);
    SignlarUser GetUserInfoByConnectionId(string connectionId);

    void SetSchedulerServiceStatus(bool status);
    bool GetSchedulerServiceStatus();
    void SetEmailSchedulerStatus(bool status);
    bool GetEmailSchedulerStatus();
    void SetDocumentIndexingStatus(bool status);
    bool GetDocumentIndexingStatus();
    void SetSendEmailReminderForWorkflowTransitionStatus(bool status);
    bool GetSendEmailReminderForWorkflowTransitionStatus();
    IEnumerable<SignlarUser> GetAllUsers();


}

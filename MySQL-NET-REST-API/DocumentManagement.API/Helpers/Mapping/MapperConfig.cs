using AutoMapper;

namespace DocumentManagement.API.Helpers.Mapping;

public static class MapperConfig
{
    public static IMapper GetMapperConfigs()
    {
        var mappingConfig = new MapperConfiguration(mc =>
        {

            mc.AddProfile(new ScreenProfile());
            mc.AddProfile(new RoleProfile());
            mc.AddProfile(new UserProfile());
            mc.AddProfile(new CategoryProfile());
            mc.AddProfile(new DocumentProfile());
            mc.AddProfile(new DocumentPermission());
            mc.AddProfile(new DocumentAuditTrailProfile());
            mc.AddProfile(new UserNotificationProfile());
            mc.AddProfile(new ReminderProfile());
            mc.AddProfile(new EmailSMTPSettingProfile());
            mc.AddProfile(new StorageSettingProfile());
            mc.AddProfile(new DocumentStatusProfile());
            mc.AddProfile(new CompanyProfileProfile());
            mc.AddProfile(new PageHelperProfile());
            mc.AddProfile(new NLogProfile());
            mc.AddProfile(new WorkflowTransitionProfile());
            mc.AddProfile(new WorkflowInstanceProfile());
            mc.AddProfile(new WorkflowProfile());
            mc.AddProfile(new WorkflowStepProfile());
            mc.AddProfile(new WorkflowStepInstanceProfile());
            //mc.AddProfile(new WorkflowStepRoleProfile());
            //mc.AddProfile(new WorkflowStepUserProfile());
            mc.AddProfile(new AllowFileExtensionProfile());
            mc.AddProfile(new DocumentSignatureProfile());
            mc.AddProfile(new FileRequestProfile());
            mc.AddProfile(new FileRequestDocumentProfile());
            mc.AddProfile(new ClientProfile());
            mc.AddProfile(new MatTableSettingProfile());
            mc.AddProfile(new CategoryPermission());
            mc.AddProfile(new DocumentMetaTagProfile());
            mc.AddProfile(new UserOpenaiMsgProfile());
            mc.AddProfile(new PageActionProfile());
        });
        return mappingConfig.CreateMapper();
    }
}

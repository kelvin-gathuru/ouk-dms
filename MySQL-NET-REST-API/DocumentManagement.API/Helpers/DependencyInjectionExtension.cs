using Amazon.S3;
using DocumentManagement.API.Helpers;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Handlers.StorageStategies;
using DocumentManagement.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentManagement.Api.Helpers;

public static class DependencyInjectionExtension
{
    public static void AddDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        services.AddScoped<IPropertyMappingService, PropertyMappingService>();
        services.AddScoped<IScreenRepository, ScreenRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        services.AddScoped<IUserClaimRepository, UserClaimRepository>();
        services.AddScoped<IRoleClaimRepository, RoleClaimRepository>();
        services.AddScoped<IDocumentRepository, DocumentRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IDocumentShareableLinkRepository, DocumentShareableLinkRepository>();
        services.AddScoped<IDocumentRolePermissionRepository, DocumentRolePermissionRepository>();
        services.AddScoped<IDocumentUserPermissionRepository, DocumentUserPermissionRepository>();
        services.AddScoped<IDocumentAuditTrailRepository, DocumentAuditTrailRepository>();
        services.AddScoped<IUserNotificationRepository, UserNotificationRepository>();
        services.AddScoped<ILoginAuditRepository, LoginAuditRepository>();

        // Reminder
        services.AddScoped<IReminderNotificationRepository, ReminderNotificationRepository>();
        services.AddScoped<IReminderRepository, ReminderRepository>();
        services.AddScoped<IReminderUserRepository, ReminderUserRepository>();
        services.AddScoped<IReminderSchedulerRepository, ReminderSchedulerRepository>();
        services.AddScoped<IDailyReminderRepository, DailyReminderRepository>();
        services.AddScoped<IQuarterlyReminderRepository, QuarterlyReminderRepository>();
        services.AddScoped<IHalfYearlyReminderRepository, HalfYearlyReminderRepository>();
        services.AddScoped<IDocumentTokenRepository, DocumentTokenRepository>();
        services.AddScoped<IEmailSMTPSettingRepository, EmailSMTPSettingRepository>();
        services.AddScoped<ISendEmailRepository, SendEmailRepository>();
        services.AddSingleton<IConnectionMappingRepository, ConnectionMappingRepository>();

        services.AddScoped<IDocumentCommentRepository, DocumentCommentRepository>();

        services.AddScoped<IDocumentVersionRepository, DocumentVersionRepository>();
        services.AddScoped<IDocumentMetaDataRepository, DocumentMetaDataRepository>();

        services.AddScoped<IStorageSettingRepository, StorageSettingRepository>();
        services.AddScoped<IDocumentStatusRepository, DocumentStatusRepository>();
        services.AddScoped<ICompanyProfileRepository, CompanyProfileRepository>();

        //Store stategies dependancies

        services.AddScoped<LocalStorageService>();
        services.AddScoped<AwsS3StorageService>();
        services.AddScoped<CloudflareR2StorageService>();
        services.AddScoped<StorageServiceFactory>();

        services.AddScoped<INLogRepository, NLogRepository>();
        services.AddScoped<IPageHelperRepository, PageHelperRepository>();
        services.AddScoped<IDocumentIndexRepository, DocumentIndexRepository>();

        services.AddScoped<IWorkflowRepository, WorkflowRepository>();
        services.AddScoped<IWorkflowInstanceRepository, WorkflowInstanceRepository>();
        services.AddScoped<IWorkflowStepRepository, WorkflowStepRepository>();
        services.AddScoped<IWorkflowStepInstanceRepository, WorkflowStepInstanceRepository>();
        services.AddScoped<IWorkflowTransitionRepository, WorkflowTransitionRepository>();
        //services.AddScoped<IWorkflowStepRoleRepository, WorkflowStepRoleRepository>();
        services.AddScoped<IWorkflowTransitionInstanceRepository, WorkflowTransitionInstanceRepository>();
        //services.AddScoped<IWorkflowStepUserRepository, WorkflowStepUserRepository>();

        services.AddScoped<IAllowFileExtensionRepository, AllowFileExtensionRepository>();
        services.AddScoped<IWorkflowInstanceEmailSenderRepository, WorkflowInstanceEmailSenderRepository>();
        services.AddScoped<IDocumentSignatureRepository, DocumentSignatureRepository>();
        services.AddScoped<IFileRequestsRepository, FileRequestsRepository>();
        services.AddScoped<IFileRequestDocumentRepository, FileRequestDocumentRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IMatTableSettingReposistory, MatTableSettingReposistory>();
        services.AddScoped<IDocumentChunkRepository, DocumentChunkRepository>();
        services.AddScoped<ICategoryRolePermissionRepository, CategoryRolePermissionRepository>();
        services.AddScoped<ICategoryUserPermissionRepository, CategoryUserPermissionRepository>();
        services.AddScoped<IDocumentMetaTagRepository, DocumentMetaTagRepository>();
        services.AddScoped<IUserOpenaiMsgRepository, UserOpenaiMsgRepository>();
        services.AddScoped<IAIPromptTemplateRepository, AIPromptTemplateRepository>();
        services.AddScoped<IWorkflowTransitionRoleRepository, WorkflowTransitionRoleRepository>();
        services.AddScoped<IWorkflowTransitionUserRepository, WorkflowTransitionUserRepository>();
        services.AddScoped<IRecentDocumentRepository, RecentDocumentRepository>();
        services.AddScoped<IArchiveRetentionRepository, ArchiveRetentionRepository>();
        services.AddScoped<IPageActionRepository, PageActionRepository>();

        // Add AWS S3 configuration
        services.AddAWSService<IAmazonS3>();
        services.AddScoped<EmailHelper>();
        services.AddScoped<OnlyOfficeTokenService>();
    }
}

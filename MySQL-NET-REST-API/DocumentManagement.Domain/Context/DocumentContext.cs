using System;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace DocumentManagement.Domain;

public class DocumentContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
{
    public DocumentContext(DbContextOptions options) : base(options)
    {
    }
    public override DbSet<User> Users { get; set; }
    public override DbSet<Role> Roles { get; set; }
    public override DbSet<UserClaim> UserClaims { get; set; }
    public override DbSet<UserRole> UserRoles { get; set; }
    public override DbSet<UserLogin> UserLogins { get; set; }
    public override DbSet<RoleClaim> RoleClaims { get; set; }
    public override DbSet<UserToken> UserTokens { get; set; }
    public DbSet<Screen> Screens { get; set; }
    //public DbSet<ScreenOperation> ScreenOperations { get; set; }
    //public DbSet<Operation> Operations { get; set; }
    public DbSet<NLog> NLog { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<Category> Categories { get; set; }
    public virtual DbSet<DocumentRolePermission> DocumentRolePermissions { get; set; }
    public virtual DbSet<DocumentUserPermission> DocumentUserPermissions { get; set; }
    public DbSet<DocumentAuditTrail> DocumentAuditTrails { get; set; }
    public DbSet<UserNotification> UserNotifications { get; set; }
    public DbSet<LoginAudit> LoginAudits { get; set; }
    public DbSet<DocumentToken> DocumentTokens { get; set; }
    public DbSet<Reminder> Reminders { get; set; }
    public DbSet<ReminderNotification> ReminderNotifications { get; set; }
    public DbSet<ReminderUser> ReminderUsers { get; set; }
    public DbSet<ReminderScheduler> ReminderSchedulers { get; set; }
    public DbSet<HalfYearlyReminder> HalfYearlyReminders { get; set; }
    public DbSet<QuarterlyReminder> QuarterlyReminders { get; set; }
    public DbSet<DailyReminder> DailyReminders { get; set; }
    public DbSet<EmailSMTPSetting> EmailSMTPSettings { get; set; }
    public DbSet<SendEmail> SendEmails { get; set; }

    public DbSet<DocumentComment> DocumentComments { get; set; }
    public DbSet<DocumentVersion> DocumentVersions { get; set; }
    public DbSet<DocumentMetaData> DocumentMetaDatas { get; set; }
    public DbSet<DocumentShareableLink> DocumentShareableLinks { get; set; }
    public DbSet<StorageSetting> StorageSettings { get; set; }
    public DbSet<DocumentStatus> DocumentStatuses { get; set; }
    public DbSet<CompanyProfile> CompanyProfiles { get; set; }
    public DbSet<PageHelper> PageHelpers { get; set; }
    public DbSet<DocumentIndex> DocumentIndexes { get; set; }
    public DbSet<Workflow> Workflows { get; set; }
    public DbSet<WorkflowStep> WorkflowSteps { get; set; }
    public DbSet<WorkflowInstance> WorkflowInstances { get; set; }
    public DbSet<WorkflowStepInstance> WorkflowStepInstances { get; set; }
    public DbSet<WorkflowTransition> WorkflowTransitions { get; set; }
    //public DbSet<WorkflowStepRole> WorkFlowStepRoles { get; set; }
    //public DbSet<WorkflowStepUser> WorkflowStepUsers { get; set; }
    public DbSet<WorkflowTransitionInstance> WorkflowTransitionInstances { get; set; }
    public DbSet<PendingTransition> PendingTransitions { get; private set; }
    public DbSet<AllowFileExtension> AllowFileExtensions { get; set; }
    public DbSet<WorkflowInstanceEmailSender> WorkflowInstanceEmailSenders { get; set; }
    public DbSet<DocumentSignature> DocumentSignatures { get; set; }
    public DbSet<CustomCategory> CustomCategories { get; private set; }
    public DbSet<FileRequest> FileRequests { get; set; }
    public DbSet<FileRequestDocument> FileRequestDocuments { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<MatTableSetting> MatTableSettings { get; set; }
    public DbSet<DocumentChunk> DocumentChunks { get; set; }
    public DbSet<CategoryUserPermission> CategoryUserPermissions { get; set; }
    public DbSet<CategoryRolePermission> CategoryRolePermissions { get; set; }
    public DbSet<DocumentMetaTag> DocumentMetaTags { get; set; }
    public DbSet<UserOpenaiMsg> UserOpenaiMsgs { get; set; }
    public DbSet<AIPromptTemplate> AIPromptTemplates { get; set; }
    public DbSet<WorkflowTransitionRole> WorkflowTransitionRoles { get; set; }
    public DbSet<WorkflowTransitionUser> WorkflowTransitionUsers { get; set; }
    public DbSet<ArchiveRetention> ArchiveRetentions { get; set; }
    public DbSet<PageAction> PageActions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {

        base.OnModelCreating(builder);

        builder.Entity<Document>(entity =>
        {
            if (Database.IsSqlServer()) // For SQL Server
            {
                entity.Property(e => e.IV).HasColumnType("VARBINARY(MAX)").IsRequired(false);
                entity.Property(e => e.Key).HasColumnType("VARBINARY(MAX)").IsRequired(false);


            }
            else //For MySQL
            {
                entity.Property(e => e.IV).HasColumnType("LONGBLOB").IsRequired(false);  // or BLOB
                entity.Property(e => e.Key).HasColumnType("LONGBLOB").IsRequired(false);
            }

        });
        builder.Entity<Document>()
            .HasIndex(b => new { b.Name, b.CategoryId, b.IsArchive, b.IsDeleted })
            .IsUnique();

        builder.Entity<DocumentVersion>(entity =>
        {
            if (Database.IsSqlServer()) // For SQL Server
            {
                entity.Property(e => e.IV).HasColumnType("VARBINARY(MAX)").IsRequired(false);
                entity.Property(e => e.Key).HasColumnType("VARBINARY(MAX)").IsRequired(false);
            }
            else //For MySQL
            {
                entity.Property(e => e.IV).HasColumnType("LONGBLOB").IsRequired(false);  // or BLOB
                entity.Property(e => e.Key).HasColumnType("LONGBLOB").IsRequired(false);
            }
        });
        builder.Entity<DocumentVersion>().HasIndex(b => new { b.DocumentId, b.Url, b.CreatedBy });

        builder.Entity<Category>(entity =>
        {
            entity.HasOne(x => x.Parent)
                .WithMany(x => x.Children)
                .HasForeignKey(x => x.ParentId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.CreatedByUser)
                .WithMany()
                .HasForeignKey(c => c.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });
        builder.Entity<Category>()
            .HasIndex(b => new { b.Name, b.ParentId, b.IsArchive, b.IsDeleted });


        builder.Entity<WorkflowInstance>(entity =>
        {
            entity.HasOne(x => x.Document)
                .WithMany(x => x.WorkflowInstances)
                .HasForeignKey(x => x.DocumentId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        });

        //builder.Entity<Document>(entity =>
        //{
        //    entity.HasMany(x => x.WorkflowInstances)
        //        .WithOne(x => x.Document)
        //        .HasForeignKey(x => x.DocumentId)
        //        .IsRequired(false)
        //        .OnDelete(DeleteBehavior.Restrict);
        //});


        builder.Entity<User>(b =>
        {
            // Each User can have many UserClaims
            b.HasMany(e => e.UserClaims)
                .WithOne(e => e.User)
                .HasForeignKey(uc => uc.UserId)
                .IsRequired();

            // Each User can have many UserLogins
            b.HasMany(e => e.UserLogins)
                .WithOne(e => e.User)
                .HasForeignKey(ul => ul.UserId)
                .IsRequired();

            // Each User can have many UserTokens
            b.HasMany(e => e.UserTokens)
                .WithOne(e => e.User)
                .HasForeignKey(ut => ut.UserId)
                .IsRequired();

            // Each User can have many entries in the UserRole join table
            b.HasMany(e => e.UserRoles)
                .WithOne(e => e.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        });

        builder.Entity<Role>(b =>
        {
            // Each Role can have many entries in the UserRole join table
            b.HasMany(e => e.UserRoles)
                .WithOne(e => e.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            // Each Role can have many associated RoleClaims
            b.HasMany(e => e.RoleClaims)
                .WithOne(e => e.Role)
                .HasForeignKey(rc => rc.RoleId)
                .IsRequired();
        });

        builder.Entity<DocumentRolePermission>(entity =>
        {
            entity.HasOne(d => d.Document)
                .WithMany(p => p.DocumentRolePermissions)
                .HasForeignKey(d => d.DocumentId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Role)
                .WithMany(p => p.DocumentRolePermissions)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        builder.Entity<DocumentUserPermission>(entity =>
        {

            entity.HasOne(d => d.Document)
                .WithMany(p => p.DocumentUserPermissions)
                .HasForeignKey(d => d.DocumentId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.User)
                .WithMany(p => p.DocumentUserPermissions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        builder.Entity<DocumentAuditTrail>(entity =>
        {
            entity.HasOne(d => d.Document)
              .WithMany(p => p.DocumentAuditTrails)
              .HasForeignKey(d => d.DocumentId)
              .OnDelete(DeleteBehavior.ClientSetNull);
        });
        builder.Entity<UserNotification>(entity =>
        {
            entity.HasOne(d => d.User)
                .WithMany(p => p.UserNotifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.Document)
                .WithMany(p => p.UserNotifications)
                .HasForeignKey(d => d.DocumentId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.WorkflowInstance)
                .WithMany()
                .HasForeignKey(d => d.WorkflowInstanceId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            entity.HasOne(d => d.FileRequestDocument)
                 .WithMany()
                 .HasForeignKey(d => d.FileRequestDocumentId)
                 .OnDelete(DeleteBehavior.ClientSetNull);
        });

        builder.Entity<ReminderUser>(b =>
        {
            b.HasKey(e => new { e.ReminderId, e.UserId });
            b.HasOne(e => e.User)
              .WithMany()
              .HasForeignKey(ur => ur.UserId)
              .OnDelete(DeleteBehavior.NoAction);
        });

        builder.Entity<Document>()
         .HasQueryFilter(p => !p.IsDeleted);
        //.HasIndex(b => b.Url);

        builder.Entity<DocumentComment>(b =>
        {
            b.HasOne(e => e.CreatedByUser)
                .WithMany()
                .HasForeignKey(ur => ur.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<DocumentVersion>(b =>
        {
            b.HasOne(e => e.CreatedByUser)
                .WithMany()
                .HasForeignKey(ur => ur.CreatedBy)
                .OnDelete(DeleteBehavior.NoAction);
        });

        builder.Entity<Workflow>(entity =>
        {
            entity.HasOne(w => w.User)
                .WithMany()
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<WorkflowStep>()
          .HasOne(wsi => wsi.Workflow)
          .WithMany(wi => wi.WorkflowSteps)
          .HasForeignKey(wsi => wsi.WorkflowId)
          .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<WorkflowTransition>()
        .HasMany(wsi => wsi.WorkflowTransitionRoles)
        .WithOne(wi => wi.WorkflowTransition)
        .OnDelete(DeleteBehavior.Cascade);



        builder.Entity<WorkflowTransition>()
        .HasMany(wsi => wsi.WorkflowTransitionUsers)
        .WithOne(wi => wi.WorkflowTransition)
        .OnDelete(DeleteBehavior.Cascade);


        builder.Entity<WorkflowInstance>()
            .HasOne(wsi => wsi.Workflow)
            .WithMany(wi => wi.WorkflowInstances)
            .HasForeignKey(wsi => wsi.WorkflowId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<WorkflowStepInstance>()
            .HasOne(wsi => wsi.WorkflowStep)
            .WithMany(wi => wi.WorkflowStepInstances)
            .HasForeignKey(wsi => wsi.StepId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<WorkflowStepInstance>()
            .HasOne(wsi => wsi.WorkflowInstance)
            .WithMany(wi => wi.WorkflowStepInstances)
            .HasForeignKey(wsi => wsi.WorkflowInstanceId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<WorkflowStepInstance>()
           .HasOne(wsi => wsi.User)
           .WithMany()
           .HasForeignKey(wsi => wsi.UserId)
           .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<WorkflowTransition>(entity =>
        {
            // Foreign key: FromStepId
            entity.HasOne(wt => wt.FromWorkflowStep)
                  .WithMany(ws => ws.FromWorkflowTransitions)
                  .HasForeignKey(wt => wt.FromStepId)
                  .OnDelete(DeleteBehavior.Restrict);


            // Foreign key: ToStepId
            entity.HasOne(wt => wt.ToWorkflowStep)
                  .WithMany(ws => ws.ToWorkflowTransitions)
                  .HasForeignKey(wt => wt.ToStepId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Foreign key: WorkflowId
            entity.HasOne(wt => wt.Workflow)
                  .WithMany(w => w.WorkflowTransitions)
                  .HasForeignKey(wt => wt.WorkflowId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<DocumentMetaData>()
        .HasOne(d => d.DocumentMetaTag)
        .WithMany()
        .HasForeignKey(d => d.DocumentMetaTagId)
        .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<DocumentSignature>(entity =>
        {
            entity.HasOne(ds => ds.Document)
                .WithMany()
                .HasForeignKey(ds => ds.DocumentId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ds => ds.SignatureUser)
                .WithMany()
                .HasForeignKey(ds => ds.SignatureUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        //builder.Entity<Document>(entity =>
        //{
        //    entity.Property(x => x.StorageType).HasDefaultValue(StorageType.LOCAL_STORAGE);
        //});

        builder.Entity<DocumentUserPermission>().HasQueryFilter(p => !p.IsDeleted);
        builder.Entity<DocumentRolePermission>().HasQueryFilter(p => !p.IsDeleted);
        builder.Entity<User>().ToTable("Users");
        builder.Entity<Role>().ToTable("Roles");
        builder.Entity<RoleClaim>().ToTable("RoleClaims");
        builder.Entity<UserClaim>().ToTable("UserClaims");
        builder.Entity<UserLogin>().ToTable("UserLogins");
        builder.Entity<UserRole>().ToTable("UserRoles");
        builder.Entity<UserToken>().ToTable("UserTokens");
        builder.Entity<DocumentUserPermission>().ToTable("DocumentUserPermissions");
        builder.Entity<DocumentRolePermission>().ToTable("DocumentRolePermissions");
        builder.Entity<UserNotification>().ToTable("UserNotifications");

        builder.Entity<CategoryRolePermission>(entity =>
        {
            entity.HasOne(d => d.Category)
                .WithMany(p => p.CategoryRolePermissions)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Role)
                .WithMany(p => p.CategoryRolePermissions)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        builder.Entity<CategoryUserPermission>(entity =>
        {
            entity.HasOne(d => d.Category)
                .WithMany(p => p.CategoryUserPermissions)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.User)
                .WithMany(p => p.CategoryUserPermissions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        builder.Entity<PageAction>(entity =>
        {
            entity.HasOne(d => d.Page)
                .WithMany()
                .HasForeignKey(d => d.PageId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Document>()
            .Property(e => e.IsArchive)
            .HasDefaultValue(false);


        builder.Entity<WorkflowTransitionRole>()
         .HasKey(o => new { o.WorkflowTransitionId, o.RoleId });
        builder.Entity<WorkflowTransitionUser>()
      .HasKey(o => new { o.WorkflowTransitionId, o.UserId });

        builder.DefalutMappingValue();
        builder.DefalutDeleteValueFilter();
    }
}

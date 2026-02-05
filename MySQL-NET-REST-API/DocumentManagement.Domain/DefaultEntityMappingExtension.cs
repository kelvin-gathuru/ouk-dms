using DocumentManagement.Data;
using DocumentManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.Domain;

public static class DefaultEntityMappingExtension
{
    public static void DefalutMappingValue(this ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<Operation>()
        //   .Property(b => b.ModifiedDate)
        //   .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<DocumentIndex>()
          .Property(b => b.CreatedDate)
          .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<Screen>()
       .Property(b => b.ModifiedDate)
       .HasDefaultValueSql("CURRENT_TIMESTAMP");

        //modelBuilder.Entity<ScreenOperation>()
        //.Property(b => b.ModifiedDate)
        //.HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<Document>()
      .Property(b => b.ModifiedDate)
      .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<Category>()
       .Property(b => b.ModifiedDate)
       .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<DocumentAuditTrail>()
         .Property(b => b.ModifiedDate)
         .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<NLog>()
            .Property(b => b.Logged)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<ArchiveRetention>()
          .Property(b => b.ModifiedDate)
          .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }

    public static void DefalutDeleteValueFilter(this ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<User>()
            .HasQueryFilter(p => !p.IsDeleted && !p.IsSystemUser);

        modelBuilder.Entity<Role>()
            .HasQueryFilter(p => !p.IsDeleted);

        //modelBuilder.Entity<Operation>()
        //    .HasQueryFilter(p => !p.IsDeleted);

        modelBuilder.Entity<Screen>()
            .HasQueryFilter(p => !p.IsDeleted);

        //modelBuilder.Entity<ScreenOperation>()
        //    .HasQueryFilter(p => !p.IsDeleted);

        modelBuilder.Entity<Document>()
            .HasQueryFilter(p => !p.IsDeleted);

        modelBuilder.Entity<Category>()
            .HasQueryFilter(p => !p.IsDeleted);

        modelBuilder.Entity<Reminder>()
            .HasQueryFilter(p => !p.IsDeleted);

        modelBuilder.Entity<EmailSMTPSetting>()
        .HasQueryFilter(p => !p.IsDeleted);

        modelBuilder.Entity<StorageSetting>()
       .HasQueryFilter(p => !p.IsDeleted);

        modelBuilder.Entity<CompanyProfile>()
        .HasQueryFilter(p => !p.IsDeleted);

        modelBuilder.Entity<Workflow>()
            .HasQueryFilter(p => !p.IsDeleted);

        modelBuilder.Entity<Client>()
            .HasQueryFilter(p => !p.IsDeleted);

        modelBuilder.Entity<FileRequest>()
            .HasQueryFilter(p => !p.IsDeleted);

        modelBuilder.Entity<DocumentMetaTag>()
            .HasQueryFilter(p => !p.IsDeleted);
    }
}
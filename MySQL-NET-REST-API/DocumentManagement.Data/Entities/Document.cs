using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManagement.Data.Entities;

public class Document : BaseEntity
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Url { get; set; }
    public bool IsAddedPageIndxing { get; set; } = false;
    [ForeignKey("CreatedBy")]
    public User User { get; set; }
    [ForeignKey("CategoryId")]
    public Category Category { get; set; }
    public StorageType StorageType { get; set; }
    public Guid? StorageSettingId { get; set; }
    [ForeignKey("StorageSettingId")]
    public StorageSetting StorageSetting { get; set; }
    public byte[] Key { get; set; } = null;
    public byte[] IV { get; set; } = null;
    public Guid? DocumentStatusId { get; set; }
    public bool IsSignatureExists { get; set; } = false;
    public Guid? ClientId { get; set; }
    [ForeignKey("ClientId")]
    public Client Client { get; set; }
    public Guid? SignById { get; set; }
    [ForeignKey("SignById")]
    public User SignBy { get; set; }
    public DateTime? SignDate { get; set; }
    [ForeignKey("DocumentStatusId")]
    public DocumentStatus DocumentStatus { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string DocumentNumber { get; set; }
    public ICollection<DocumentUserPermission> DocumentUserPermissions { get; set; }
    public ICollection<DocumentRolePermission> DocumentRolePermissions { get; set; }
    public ICollection<DocumentAuditTrail> DocumentAuditTrails { get; set; }
    public ICollection<UserNotification> UserNotifications { get; set; }
    public ICollection<DocumentComment> DocumentComments { get; set; }
    public List<DocumentMetaData> DocumentMetaDatas { get; set; } = new List<DocumentMetaData>();
    public ICollection<WorkflowInstance> WorkflowInstances { get; set; } = new List<WorkflowInstance>();
    public bool IsArchive { get; set; }
    public string Comment { get; set; }
    public string Extension { get; set; }
    public bool IsChunk { get; set; }
    public bool IsAllChunkUploaded { get; set; }
    public bool IsShared { get; set; }
    public Guid? ArchiveById { get; set; }
    [ForeignKey("ArchiveById")]
    public User ArchiveBy { get; set; }
    public int? RetentionPeriodInDays { get; set; }
    public RETENTION_ACTION_ENUM? OnExpiryAction { get; set; }
    public DateOnly? RetentionDate { get; set; }
}

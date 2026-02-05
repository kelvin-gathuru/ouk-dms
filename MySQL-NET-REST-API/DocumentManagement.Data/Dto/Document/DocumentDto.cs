using System;
using System.Collections.Generic;
using DocumentManagement.Data.Entities;

namespace DocumentManagement.Data.Dto;

public class DocumentDto : ErrorStatusCode
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string CreatedBy { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; }
    public Guid? DocumentStatusId { get; set; }
    public Guid? ClientId { get; set; }
    public Client Client { get; set; }
    public DocumentStatus DocumentStatus { get; set; }
    public Guid? StorageSettingId { get; set; }
    public string StorageSettingName { get; set; }
    public string ViewerType { get; set; }
    public DateTime? ExpiredDate { get; set; }
    public bool IsAllowDownload { get; set; }
    public bool IsAddedPageIndxing { get; set; }
    public List<DocumentMetaDataDto> DocumentMetaDatas { get; set; } = new List<DocumentMetaDataDto>();
    public StorageType StorageType { get; set; }
    public bool IsSignatureExists { get; set; }
    public string SignBy { get; set; }
    public DateTime? SignByDate { get; set; }
    public string DocumentNumber { get; set; }
    public List<WorkflowShortDetail> WorkflowsDetail { get; set; } = new List<WorkflowShortDetail>();
    public string Comment { get; set; }
    public bool IsChunk { get; set; } = false;
    public string Extension { get; set; }
    public Guid? DocumentVersionId { get; set; }
    public int VersionNumber { get; set; } = 0;
    public int CommentCount { get; set; } = 0;
    public bool IsShared { get; set; }
    public int? RetentionPeriodInDays { get; set; }
    public RETENTION_ACTION_ENUM? OnExpiryAction { get; set; }
    public Guid? ArchiveById { get; set; }
    public string ArchiveName { get; set; }
}

using System;
using System.Collections.Generic;
using DocumentManagement.Data.Entities;

namespace DocumentManagement.Data.Dto;

/// <summary>
/// A document in the DMS, as exposed to the intranet application.
/// </summary>
public class DocumentDto : ErrorStatusCode
{
    /// <summary>The document id (GUID).</summary>
    public Guid Id { get; set; }
    /// <summary>The document name/title.</summary>
    public string Name { get; set; }
    /// <summary>Relative path/URL of the stored file.</summary>
    public string Url { get; set; }
    /// <summary>Free-text description of the document.</summary>
    public string Description { get; set; }
    /// <summary>When the document was created (UTC).</summary>
    public DateTime CreatedDate { get; set; }
    /// <summary>When the document was last modified (UTC).</summary>
    public DateTime? ModifiedDate { get; set; }
    /// <summary>Display name of the user who created the document.</summary>
    public string CreatedBy { get; set; }
    /// <summary>The category id the document belongs to.</summary>
    public Guid CategoryId { get; set; }
    /// <summary>The category name.</summary>
    public string CategoryName { get; set; }
    /// <summary>The document status id.</summary>
    public Guid? DocumentStatusId { get; set; }
    /// <summary>The client id the document belongs to.</summary>
    public Guid? ClientId { get; set; }
    /// <summary>The client the document belongs to.</summary>
    public Client Client { get; set; }
    /// <summary>The document status.</summary>
    public DocumentStatus DocumentStatus { get; set; }
    /// <summary>The storage setting id.</summary>
    public Guid? StorageSettingId { get; set; }
    /// <summary>The storage setting name.</summary>
    public string StorageSettingName { get; set; }
    /// <summary>The viewer type used to render the document.</summary>
    public string ViewerType { get; set; }
    /// <summary>The date the document expires, if applicable.</summary>
    public DateTime? ExpiredDate { get; set; }
    /// <summary>Whether download is allowed for the requesting principal.</summary>
    public bool IsAllowDownload { get; set; }
    /// <summary>Whether the document content was added to the search index.</summary>
    public bool IsAddedPageIndxing { get; set; }
    /// <summary>The metadata tags attached to the document.</summary>
    public List<DocumentMetaDataDto> DocumentMetaDatas { get; set; } = new List<DocumentMetaDataDto>();
    /// <summary>How the file is stored (file system, database, S3, etc).</summary>
    public StorageType StorageType { get; set; }
    /// <summary>Whether the document has an electronic signature.</summary>
    public bool IsSignatureExists { get; set; }
    /// <summary>Display name of the signatory.</summary>
    public string SignBy { get; set; }
    /// <summary>When the document was signed, if signed.</summary>
    public DateTime? SignByDate { get; set; }
    /// <summary>The unique document number.</summary>
    public string DocumentNumber { get; set; }
    /// <summary>The workflow instances the document is part of.</summary>
    public List<WorkflowShortDetail> WorkflowsDetail { get; set; } = new List<WorkflowShortDetail>();
    /// <summary>Most recent comment text.</summary>
    public string Comment { get; set; }
    /// <summary>True when the file was uploaded in chunks.</summary>
    public bool IsChunk { get; set; } = false;
    /// <summary>File extension, e.g. "pdf".</summary>
    public string Extension { get; set; }
    /// <summary>The current document version id.</summary>
    public Guid? DocumentVersionId { get; set; }
    /// <summary>The current version number.</summary>
    public int VersionNumber { get; set; } = 0;
    /// <summary>Number of comments on the document.</summary>
    public int CommentCount { get; set; } = 0;
    /// <summary>Whether the document is shared.</summary>
    public bool IsShared { get; set; }
    /// <summary>Whether the document is exposed to the intranet application.</summary>
    public bool IsIntranetAccessible { get; set; }
    /// <summary>Retention period in days before the document expires.</summary>
    public int? RetentionPeriodInDays { get; set; }
    /// <summary>Action taken when the document reaches its expiry date.</summary>
    public RETENTION_ACTION_ENUM? OnExpiryAction { get; set; }
    /// <summary>The user id that archived the document, if archived.</summary>
    public Guid? ArchiveById { get; set; }
    /// <summary>Display name of the user that archived the document.</summary>
    public string ArchiveName { get; set; }
}

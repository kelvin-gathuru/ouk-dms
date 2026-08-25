using System;

namespace DocumentManagement.Data.Resources;

/// <summary>
/// Filter, sort and paging options for the intranet document list endpoint.
/// Every property is optional; when omitted the appropriate default is used.
/// </summary>
public class DocumentResource : ResourceParameter
{
    public DocumentResource() : base("Name")
    {
    }
    /// <summary>Filter by document name (partial match).</summary>
    public string Name { get; set; }
    /// <summary>Filter by document id.</summary>
    public string Id { get; set; }
    /// <summary>Filter by category id.</summary>
    public string CategoryId { get; set; }
    /// <summary>Filter by operation.</summary>
    public string Operation { get; set; }
    /// <summary>Filter by document status id.</summary>
    public string DocumentStatusId { get; set; }
    /// <summary>Filter by storage setting id.</summary>
    public string StorageSettingId { get; set; }
    /// <summary>Filter by client id.</summary>
    public string ClientId { get; set; }
    /// <summary>Filter by creation date.</summary>
    public DateTime? CreateDate { get; set; }
    /// <summary>Filter by creation date (string form, yyyy-MM-dd).</summary>
    public string CreateDateString { get; set; }
    /// <summary>Filter documents created on or after this date.</summary>
    public DateTime? StartDate { get; set; }
    /// <summary>Filter documents created on or before this date.</summary>
    public DateTime? EndDate { get; set; }
    /// <summary>Filter by the display name of the user who created the document.</summary>
    public string CreatedBy { get; set; }
    /// <summary>Filter by metadata tags.</summary>
    public string MetaTags { get; set; }
    /// <summary>Filter archived documents (always forced to false by the intranet API).</summary>
    public bool IsArchive { get; set; }
    /// <summary>Filter by intranet accessibility (always forced to true by the intranet API).</summary>
    public bool? IsIntranetAccessible { get; set; }
    /// <summary>Filter by document number.</summary>
    public string DocumentNumber { get; set; }
    /// <summary>Filter by metadata tag type id.</summary>
    public string MetaTagsTypeId { get; set; }
}

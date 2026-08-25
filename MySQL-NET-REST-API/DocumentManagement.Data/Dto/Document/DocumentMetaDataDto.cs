using System;

namespace DocumentManagement.Data.Dto;

/// <summary>
/// A metadata tag attached to a document.
/// </summary>
public class DocumentMetaDataDto
{
    /// <summary>The metadata id (GUID).</summary>
    public Guid? Id { get; set; }
    /// <summary>The document id (GUID) the metadata belongs to.</summary>
    public Guid? DocumentId { get; set; }
    /// <summary>The metadata tag definition id (GUID).</summary>
    public Guid? DocumentMetaTagId { get; set; }
    /// <summary>The metadata tag name.</summary>
    public string Metatag { get; set; }
    /// <summary>The metadata value/date, if the tag is date typed.</summary>
    public DateTime? MetaTagDate { get; set; }
    /// <summary>The type of the metadata tag.</summary>
    public MetaTagType MetaTagType { get; set; }
}

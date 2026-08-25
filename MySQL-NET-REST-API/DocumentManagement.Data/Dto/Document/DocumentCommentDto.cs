using System;

namespace DocumentManagement.Data.Dto;

/// <summary>
/// A comment attached to a document.
/// </summary>
public class DocumentCommentDto
{
    /// <summary>The comment id (GUID).</summary>
    public Guid Id { get; set; }
    /// <summary>The document id (GUID) the comment belongs to.</summary>
    public Guid DocumentId { get; set; }
    /// <summary>The comment text.</summary>
    public string Comment { get; set; }
    /// <summary>When the comment was created (UTC).</summary>
    public DateTime CreatedDate { get; set; }
    /// <summary>Display name of the user who created the comment.</summary>
    public string CreatedBy { get; set; }
}

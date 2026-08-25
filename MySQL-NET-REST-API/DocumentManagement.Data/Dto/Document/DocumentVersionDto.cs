using System;

namespace DocumentManagement.Data.Dto
{
    /// <summary>
    /// A single version of a document.
    /// </summary>
    public class DocumentVersionDto
    {
        /// <summary>The version id (GUID). Use this with the version download endpoint.</summary>
        public Guid Id { get; set; }
        /// <summary>The parent document id (GUID).</summary>
        public Guid DocumentId { get; set; }
        /// <summary>Relative path/URL of the stored file.</summary>
        public string Url { get; set; }
        /// <summary>Display name of the user who created this version.</summary>
        public string CreatedByUser { get; set; }
        /// <summary>True when this is the current version of the document.</summary>
        public bool IsCurrentVersion { get; set; }
        /// <summary>When this version was last modified (UTC).</summary>
        public DateTime ModifiedDate { get; set; }
        /// <summary>Display name of the signatory, if signed.</summary>
        public string SignBy { get; set; }
        /// <summary>When the version was signed, if signed.</summary>
        public DateTime? SignDate { get; set; }
        /// <summary>Version comment/revision note.</summary>
        public string Comment { get; set; }
        /// <summary>True when the file was uploaded in chunks.</summary>
        public bool IsChunk { get; set; }
        /// <summary>Sequential version number (1 = original upload).</summary>
        public int VersionNumber { get; set; }
        /// <summary>File extension, e.g. "pdf".</summary>
        public string Extension { get; set; }
        /// <summary>True when a signature exists on this version.</summary>
        public bool IsSignatureExists { get; set; }
        /// <summary>The category id the document belongs to.</summary>
        public Guid? CategoryId { get; set; }
    }
}

using System;

namespace DocumentManagement.Data.Dto
{
    /// <summary>
    /// A single audit trail entry for a document.
    /// </summary>
    public class DocumentAuditTrailDto : ErrorStatusCode
    {
        /// <summary>The audit trail entry id (GUID).</summary>
        public Guid Id { get; set; }
        /// <summary>The document id (GUID) the entry belongs to.</summary>
        public Guid? DocumentId { get; set; }
        /// <summary>The category name of the document.</summary>
        public string CategoryName { get; set; }
        /// <summary>The document name.</summary>
        public string DocumentName { get; set; }
        /// <summary>The operation performed (e.g. Create, Update, Download, View).</summary>
        public string OperationName { get; set; }
        /// <summary>Display name of the user who performed the operation.</summary>
        public string CreatedBy { get; set; }
        /// <summary>When the operation was performed (UTC).</summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>Permission user involved in the operation, if any.</summary>
        public string PermissionUser { get; set; }
        /// <summary>Permission role involved in the operation, if any.</summary>
        public string PermissionRole { get; set; }
        /// <summary>The document URL at the time of the operation.</summary>
        public string Url { get; set; }
        /// <summary>Comment recorded with the operation, if any.</summary>
        public string Comment { get; set; }
        /// <summary>Whether the operation deleted the document.</summary>
        public bool IsDocumentDeleted { get; set; }
        /// <summary>Whether the operation involved a chunked file.</summary>
        public bool IsChunk { get; set; }
        /// <summary>The document number at the time of the operation.</summary>
        public string DocumentNumber { get; set; }
        /// <summary>The category id of the document.</summary>
        public Guid? CategoryId { get; set; }

    }
}

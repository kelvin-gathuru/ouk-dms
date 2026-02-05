using System;

namespace DocumentManagement.Data.Dto
{
    public class DocumentAuditTrailDto : ErrorStatusCode
    {
        public Guid Id { get; set; }
        public Guid? DocumentId { get; set; }
        public string CategoryName { get; set; }
        public string DocumentName { get; set; }
        public string OperationName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string PermissionUser { get; set; }
        public string PermissionRole { get; set; }
        public string Url { get; set; }
        public string Comment { get; set; }
        public bool IsDocumentDeleted { get; set; }
        public bool IsChunk { get; set; }
        public string DocumentNumber { get; set; }
        public Guid? CategoryId { get; set; }

    }
}

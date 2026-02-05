using System;

namespace DocumentManagement.Data.Dto
{
    public class DocumentVersionDto
    {
        public Guid Id { get; set; }
        public Guid DocumentId { get; set; }
        public string Url { get; set; }
        public string CreatedByUser { get; set; }
        public bool IsCurrentVersion { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string SignBy { get; set; }
        public DateTime? SignDate { get; set; }
        public string Comment { get; set; }
        public bool IsChunk { get; set; }
        public int VersionNumber { get; set; }
        public string Extension { get; set; }
        public bool IsSignatureExists { get; set; }
        public Guid? CategoryId { get; set; }
    }
}

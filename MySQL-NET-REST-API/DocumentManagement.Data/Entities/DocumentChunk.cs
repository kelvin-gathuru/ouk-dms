using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace DocumentManagement.Data
{
    public class DocumentChunk
    {
        public Guid Id { get; set; }
        public int ChunkIndex { get; set; }
        public string Url { get; set; }
        public long  Size { get; set; }
        public string Extension { get; set; }
        public int TotalChunk { get; set; }
        public Guid DocumentVersionId { get; set; }
        [ForeignKey("DocumentVersionId")]
        public DocumentVersion DocumentVersion { get; set; }
    }
}

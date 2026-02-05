using DocumentManagement.Data.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManagement.Data
{
    public class DocumentVersion : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid DocumentId { get; set; }
        public string Url { get; set; }
        [ForeignKey("DocumentId")]
        public Document Document { get; set; }
        [ForeignKey("CreatedBy")]
        public User CreatedByUser { get; set; }
        [Column(TypeName = "LONGBLOB")]
        public byte[] Key { get; set; }
        [Column(TypeName = "LONGBLOB")]
        public byte[] IV { get; set; }
        public Guid? SignById { get; set; }
        [ForeignKey("SignById")]
        public User SignBy { get; set; }
        public DateTime? SignDate { get; set; }
        public string Comment { get; set; }
        public bool IsCurrentVersion { get; set; }
        public int VersionNumber { get; set; }
        public string Extension { get; set; }
        public bool IsChunk { get; set; }
        public bool IsAllChunkUploaded { get; set; }
        public ICollection<DocumentChunk> DocumentChunks { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Data.Dto
{
    public class DocumentChunkDto
    {
        public Guid Id { get; set; }
        public int ChunkIndex { get; set; }
        public long Size { get; set; }
        public int TotalChunks { get; set; }
        public string Extension { get; set; }
        public Guid DocumentVersionId { get; set; }
        public Guid DocumentId { get; set; }
    }
}

using System;

namespace DocumentManagement.MediatR.Handlers
{
    public class DocumentDownload
    {
        public byte[] Data { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public int? ChunkIndex { get; set; }
        public string  DocumentNumber { get; set; }

    }
}

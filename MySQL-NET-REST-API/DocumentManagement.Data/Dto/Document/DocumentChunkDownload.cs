
namespace DocumentManagement.Data.Dto
{
    public class DocumentChunkDownload
    {
        public string Data { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public int ChunkIndex { get; set; }
    }
}

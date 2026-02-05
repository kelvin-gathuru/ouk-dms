using System;

namespace DocumentManagement.Data.Dto.Document
{
    public class OfficeViewerRequest
    {
        public Guid Token { get; set; }
        public bool IsVersion { get; set; } = false;
        public bool IsPublic { get; set; } = false;
        public bool IsFileRequest { get; set; } = false;
        public string Password { get; set; } = string.Empty;
        public Guid? DocumentVersionId { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace DocumentManagement.Data.Dto
{
    public class DocumentSignatureDto
    {
        public Guid Id { get; set; }
        public Guid DocumentId { get; set; }
        public DocumentDto Document { get; set; }
        public Guid SignatureUserId { get; set; }
        public UserDto SignatureUser { get; set; }
        public string SignatureUrl { get; set; }
        public DateTime? SignatureDate { get; set; }
    }
}

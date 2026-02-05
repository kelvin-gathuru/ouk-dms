using DocumentManagement.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManagement.Data
{
    public class DocumentSignature
    {
        public Guid Id { get; set; }
        public Guid DocumentId { get; set; }
        [ForeignKey("DocumentId")]
        public Document Document { get; set; }
        public Guid SignatureUserId { get; set; }
        [ForeignKey("SignatureUserId")]
        public User SignatureUser { get; set; }
        public string SignatureUrl { get; set; }
        public DateTime? SignatureDate { get; set; }
    }
}

using DocumentManagement.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManagement.Data
{
    public class SendEmail : BaseEntity
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string ToName { get; set; }
        public Guid? DocumentId { get; set; }
        [ForeignKey("DocumentId")]
        public Document Document { get; set; }
        public bool IsSend { get; set; }
        public string Email { get; set; }
    }
}

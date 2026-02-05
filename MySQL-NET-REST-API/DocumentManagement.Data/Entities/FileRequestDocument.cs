using DocumentManagement.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManagement.Data
{
    public class FileRequestDocument
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public Guid FileRequestId { get; set; }
        [ForeignKey("FileRequestId")]
        public FileRequest FileRequest { get; set; }
        public FileRequestDocumentStatus FileRequestDocumentStatus { get; set; }
        public DateTime? ApprovedRejectedDate { get; set; }
        public Guid? ApprovalOrRjectedById { get; set; }
        [ForeignKey("ApprovalOrRjectedById")]
        public User ApprovalRejectedBy { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedDate { get; set; }

    }
    public enum FileRequestDocumentStatus
    {
        PENDING = 0,
        APPROVED = 1,
        REJECTED = 2
    }
}

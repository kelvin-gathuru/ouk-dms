using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace DocumentManagement.Data.Entities
{
    public class FileRequest
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int? MaxDocument { get; set; }
        public int? SizeInMb { get; set; }
        public string AllowExtension { get; set; }
        public FileRequestStatus FileRequestStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedById { get; set; }
        [ForeignKey("CreatedById")]
        public User CreatedBy { get; set; }
        public DateTime? LinkExpiryTime { get; set; }
        public bool IsLinkExpired { get; set; }
        public bool IsDeleted { get; set; }
        public List<FileRequestDocument> FileRequestDocuments { get; set; }
    }

    public enum FileRequestStatus
    {
        CREATED = 0,
        UPLOADED = 1
    }
}

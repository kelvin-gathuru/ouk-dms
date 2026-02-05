using DocumentManagement.Data.Entities;
using System;
using System.Collections.Generic;

namespace DocumentManagement.Data.Dto
{
    public class FileRequestDto
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
        public string CreatedById { get; set; }
        public UserDto CreatedBy { get; set; }
        public DateTime? LinkExpiryTime { get; set; }
        public bool IsLinkExpired { get; set; }
        public bool HasPassword { get; set; }
        public List<FileRequestDocumentDto> FileRequestDocuments { get; set; }
    }
}

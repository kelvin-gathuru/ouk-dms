using System;

namespace DocumentManagement.Data.Dto
{
    public class FileRequestDocumentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public Guid FileRequestId { get; set; }
        public FileRequestDto FileRequest { get; set; }
        public FileRequestDocumentStatus FileRequestDocumentStatus { get; set; }
        public DateTime? ApprovedRejectedDate { get; set; }
        public Guid? ApprovalOrRjectedById { get; set; }
        public UserDto ApprovalRejectedBy { get; set; }
        public string Reason { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}

using DocumentManagement.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManagement.Data
{
    public class UserNotification : BaseEntity
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public Guid? DocumentId { get; set; }
        public Document Document { get; set; }
        public Guid? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public Guid? WorkflowInstanceId { get; set; }
        [ForeignKey("WorkflowInstanceId")]
        public WorkflowInstance WorkflowInstance { get; set; }
        public Guid? FileRequestDocumentId { get; set; }
        [ForeignKey("FileRequestDocumentId")]
        public FileRequestDocument FileRequestDocument { get; set; }
        public NotificationsType NotificationsType { get; set; }
    }
}

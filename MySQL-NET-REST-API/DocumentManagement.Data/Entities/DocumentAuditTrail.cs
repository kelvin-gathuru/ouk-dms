using DocumentManagement.Data.Dto;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManagement.Data.Entities
{
    public class DocumentAuditTrail: BaseEntity
    {
        public Guid Id { get; set; }
        public Guid? DocumentId { get; set; }
        [ForeignKey("DocumentId")]
        public Document Document { get; set; }
        [ForeignKey("CreatedBy")]
        public User CreatedByUser { get; set; }
        public DocumentOperation OperationName { get; set; }
        public Guid? AssignToUserId { get; set; }
        public Guid? AssignToRoleId { get; set; }
        [ForeignKey("AssignToUserId")]
        public User AssignToUser { get; set; }
        [ForeignKey("AssignToRoleId")]
        public Role AssignToRole { get; set; }
        public string Comment { get; set; }
        public Guid? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

    }
}

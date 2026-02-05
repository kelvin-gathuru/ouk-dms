using DocumentManagement.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManagement.Data
{
    public class DocumentRolePermission : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid DocumentId { get; set; }
        public Guid RoleId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsTimeBound { get; set; }
        public bool IsAllowDownload { get; set; }
        [ForeignKey("CreatedBy")]
        public User CreatedByUser { get; set; }
        [ForeignKey("DocumentId")]
        public Document Document { get; set; }
        [ForeignKey("RoleId")]
        public Role Role { get; set; }
    }
}

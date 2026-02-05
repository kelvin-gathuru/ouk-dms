using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace DocumentManagement.Data.Entities
{
    public class CategoryUserPermission: BaseEntity
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public Guid UserId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsTimeBound { get; set; }
        public bool IsAllowDownload { get; set; }
        [ForeignKey("CreatedBy")]
        public User CreatedByUser { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public Guid? ParentId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManagement.Data.Entities;

public class Category : BaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid? ParentId { get; set; }
    [ForeignKey("ParentId")]
    public Category Parent { get; set; }
    public virtual ICollection<Category> Children { get; set; } = new List<Category>();
    [ForeignKey("CreatedBy")]
    public User CreatedByUser { get; set; }
    public bool IsArchive { get; set; }
    public Guid? ArchiveParentId { get; set; }
    public Guid? ArchiveById { get; set; }
    [ForeignKey("ArchiveById")]
    public User ArchiveBy { get; set; }
    public ICollection<CategoryRolePermission> CategoryRolePermissions { get; set; } = new List<CategoryRolePermission>();
    public ICollection<CategoryUserPermission> CategoryUserPermissions { get; set; } = new List<CategoryUserPermission>();
    public List<Document> Documents { get; set; } = new List<Document>();
}

using System;
using System.ComponentModel.DataAnnotations.Schema;
using DocumentManagement.Data.Entities;

namespace DocumentManagement.Data;

public class DocumentComment : BaseEntity
{
    public Guid Id { get; set; }
    public Guid DocumentId { get; set; }
    public Document Document { get; set; }
    public string Comment { get; set; }
    [ForeignKey("CreatedBy")]
    public User CreatedByUser { get; set; }
}

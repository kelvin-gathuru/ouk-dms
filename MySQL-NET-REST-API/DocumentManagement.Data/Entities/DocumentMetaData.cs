using DocumentManagement.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManagement.Data;

public class DocumentMetaData : BaseEntity
{
    public Guid Id { get; set; }
    public Guid DocumentId { get; set; }
    public Document Document { get; set; }
    public Guid DocumentMetaTagId { get; set; }
    [ForeignKey("DocumentMetaTagId")]
    public DocumentMetaTag DocumentMetaTag { get; set; }
    public string Metatag { get; set; }
    public DateTime? MetaTagDate { get; set; }

}

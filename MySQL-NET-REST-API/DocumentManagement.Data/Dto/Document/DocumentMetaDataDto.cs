using System;

namespace DocumentManagement.Data.Dto;

public class DocumentMetaDataDto
{
    public Guid? Id { get; set; }
    public Guid? DocumentId { get; set; }
    public Guid? DocumentMetaTagId { get; set; }
    public string Metatag { get; set; }
    public DateTime? MetaTagDate { get; set; }
    public MetaTagType MetaTagType { get; set; }
}

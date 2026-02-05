using System;

namespace DocumentManagement.Data.Dto;
public class DocumentMetaTagDto
{
    public Guid Id { get; set; }
    public MetaTagType Type { get; set; }
    public string Name { get; set; }
    public bool IsEditable { get; set; } = true;
}

using System;

namespace DocumentManagement.Data;
public class DocumentMetaTag : BaseEntity
{
    public Guid Id { get; set; }
    public MetaTagType Type { get; set; }
    public string Name { get; set; }
    public bool IsEditable { get; set; } = true;
}

public enum MetaTagType
{
    STRING = 0,
    DATETIME = 1
}
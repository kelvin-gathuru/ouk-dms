using System;

namespace DocumentManagement.Data.Entities;
public class ArchiveRetention : BaseEntity
{
    public Guid Id { get; set; }
    public int? RetentionPeriodInDays { get; set; }
    public bool IsEnabled { get; set; }
}

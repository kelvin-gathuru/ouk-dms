using System;

namespace DocumentManagement.Data.Dto;
public class ArchiveRetentionDto
{
    public Guid Id { get; set; }
    public int? RetentionPeriodInDays { get; set; }
    public bool IsEnabled { get; set; }
}

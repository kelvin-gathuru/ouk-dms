using DocumentManagement.Data.Entities;
using System;

namespace DocumentManagement.Data.Dto;

public class FileRequestListDataDto
{
    public Guid? Id { get; set; }
    public string Subject { get; set; }
    public string Email { get; set; }
    public int? MaxDocument { get; set; }
    public int? SizeInMb { get; set; }
    public string AllowExtension { get; set; }
    public FileRequestStatus FileRequestStatus { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? CreatedById { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? LinkExpiryTime { get; set; }
}

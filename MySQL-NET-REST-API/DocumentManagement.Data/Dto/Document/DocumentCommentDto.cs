using System;

namespace DocumentManagement.Data.Dto;

public class DocumentCommentDto
{
    public Guid Id { get; set; }
    public Guid DocumentId { get; set; }
    public string Comment { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; }
}

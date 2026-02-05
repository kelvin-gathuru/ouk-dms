using System;
using System.Collections.Generic;

namespace DocumentManagement.Data.Dto;

public class CategoryDto : ErrorStatusCode
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? Level { get; set; }
    public Guid? ParentId { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedUserName { get; set; }
    public bool IsShared { get; set; } = false;
    public bool? IsAssignedToMe { get; set; } = false;
    public bool IsArchive { get; set; }
    public List<CategoryDto> Children { get; set; } = new List<CategoryDto>();
}

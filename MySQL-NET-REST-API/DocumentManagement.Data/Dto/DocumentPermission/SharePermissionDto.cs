using System.Collections.Generic;

namespace DocumentManagement.Data.Dto;
public class SharePermissionDto
{
    public List<DocumentPermissionDto> DocumentPermissions { get; set; } = new List<DocumentPermissionDto>();
    public List<CategoryPermissionDto> CategoryPermissions { get; set; } = new List<CategoryPermissionDto>();
}

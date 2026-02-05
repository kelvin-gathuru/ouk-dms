using DocumentManagement.Data.Dto;
using MediatR;
using System.Collections.Generic;


namespace DocumentManagement.MediatR.Commands
{
    public class AddCategoryRolePermissionCommand : IRequest<CategoryRolePermissionDto>
    {
        public ICollection<CategoryRolePermissionDto> CategoryRolePermissions { get; set; }
    }
}

using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System.Collections.Generic;


namespace DocumentManagement.MediatR.Commands
{
    public class AddCategoryUserPermissionCommand: IRequest<CategoryUserPermissionDto>
    {
        public ICollection<CategoryUserPermissionDto> CategoryUserPermissions { get; set; }
    }
}

using DocumentManagement.Data.Dto;
using MediatR;
using System.Collections.Generic;

namespace DocumentManagement.MediatR.Commands
{
    public class AddDocumentRolePermissionCommand : IRequest<DocumentRolePermissionDto>
    {
        public ICollection<DocumentRolePermissionDto> DocumentRolePermissions { get; set; }
    }
}

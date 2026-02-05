using DocumentManagement.Data.Dto;
using MediatR;
using System.Collections.Generic;

namespace DocumentManagement.MediatR.Commands
{
    public class AddDocumentUserPermissionCommand : IRequest<DocumentUserPermissionDto>
    {
        public ICollection<DocumentUserPermissionDto> DocumentUserPermissions { get; set; }
    }
}

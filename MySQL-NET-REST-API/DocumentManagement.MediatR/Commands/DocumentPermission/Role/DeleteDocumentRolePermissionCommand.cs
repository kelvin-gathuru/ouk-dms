using DocumentManagement.Data.Dto;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class DeleteDocumentRolePermissionCommand : IRequest<DocumentRolePermissionDto>
    {
        public Guid Id { get; set; }
    }
}

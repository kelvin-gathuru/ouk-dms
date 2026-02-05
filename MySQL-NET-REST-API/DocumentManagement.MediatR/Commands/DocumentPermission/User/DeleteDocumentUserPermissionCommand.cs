using DocumentManagement.Data.Dto;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class DeleteDocumentUserPermissionCommand : IRequest<DocumentUserPermissionDto>
    {
        public Guid Id { get; set; }
    }
}

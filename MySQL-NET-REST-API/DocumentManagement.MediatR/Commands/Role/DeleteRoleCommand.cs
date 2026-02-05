using DocumentManagement.Data.Dto;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class DeleteRoleCommand : IRequest<RoleDto>
    {
        public Guid Id { get; set; }
    }
}

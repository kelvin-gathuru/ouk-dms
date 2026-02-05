using DocumentManagement.Data.Dto;
using MediatR;
using System;
using System.Collections.Generic;

namespace DocumentManagement.MediatR.Commands
{
    public class UpdateUserRoleCommand : IRequest<UserRoleDto>
    {
        public Guid Id { get; set; }
        public List<UserRoleDto> UserRoles { get; set; }
    }
}

using DocumentManagement.Data.Dto;
using MediatR;
using System.Collections.Generic;

namespace DocumentManagement.MediatR.Commands
{
    public class AddRoleCommand : IRequest<RoleDto>
    {
        public string Name { get; set; }
        public List<RoleClaimDto> RoleClaims { get; set; } = new List<RoleClaimDto>();
    }
}

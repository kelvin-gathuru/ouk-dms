using DocumentManagement.Data.Dto;
using MediatR;
using System;
using System.Collections.Generic;

namespace DocumentManagement.MediatR.Queries
{
    public class GetRoleUsersQuery : IRequest<List<UserRoleDto>>
    {
        public Guid RoleId { get; set; }
    }
}

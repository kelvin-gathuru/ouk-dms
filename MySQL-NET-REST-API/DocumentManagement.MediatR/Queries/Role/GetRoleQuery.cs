using DocumentManagement.Data.Dto;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Queries
{
    public class GetRoleQuery: IRequest<RoleDto>
    {
        public Guid Id { get; set; }
    }
}

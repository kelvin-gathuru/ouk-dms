using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Resources;
using DocumentManagement.Repository;
using MediatR;
using System.Collections.Generic;

namespace DocumentManagement.MediatR.Queries
{
    public class GetUsersQuery : IRequest<UserList>
    {
        public UserResource UserResource { get; set; }
    }
}

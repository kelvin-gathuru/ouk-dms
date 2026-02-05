using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Data;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace DocumentManagement.Repository
{
    public interface IUserRoleRepository : IGenericRepository<UserRole>
    {
        Task<List<Guid>> GetUsersByRoles(List<Guid> roleIds);
        Task<List<User>> GetUserDetailsByRoles(List<Guid> roleIds);
    }
}

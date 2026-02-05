using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Domain;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DocumentManagement.Repository
{
    public class UserRoleRepository : GenericRepository<UserRole, DocumentContext>,
       IUserRoleRepository
    {
        public UserRoleRepository(
            IUnitOfWork<DocumentContext> uow
            ) : base(uow)
        {
        }

        public async Task<List<Guid>> GetUsersByRoles(List<Guid> roleIds)
        {
            return await All.Where(c => roleIds.Contains(c.RoleId)).Select(c => c.UserId).Distinct().ToListAsync();
        }

        public async Task<List<User>> GetUserDetailsByRoles(List<Guid> roleIds)
        {
            return await All.Include(c=>c.User).Where(c => roleIds.Contains(c.RoleId)).Select(c => c.User).Distinct().ToListAsync();
        }
    }
}

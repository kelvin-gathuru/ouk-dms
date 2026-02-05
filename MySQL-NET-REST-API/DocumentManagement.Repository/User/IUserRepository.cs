using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DocumentManagement.Repository;

public interface IUserRepository : IGenericRepository<User>
{
    Task<UserAuthDto> BuildUserAuthObject(User appUser);

    Task<List<User>> GetUsersByIds(List<Guid> ids);
    Task<UserList> GetUsers(UserResource userResource);
    Task<List<Guid>> GetUsersByRoleId(Guid roleId);
}

using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;

public class GetCategoryPermissionQueryHandler
    (ICategoryRolePermissionRepository categoryRolePermissionRepository,
        IMapper mapper,
        ICategoryUserPermissionRepository categoryUserPermissionRepository) : IRequestHandler<GetCategoryPermissionQuery, List<CategoryPermissionDto>>
{
    public async Task<List<CategoryPermissionDto>> Handle(GetCategoryPermissionQuery request, CancellationToken cancellationToken)
    {
        var result = new List<CategoryPermissionDto>();
        var categoryRolePermissions = await categoryRolePermissionRepository
            .AllIncluding(c => c.Role)
            .Where(c => c.CategoryId == request.CategoryId)
            .ToListAsync();
        var rolePermissions = mapper.Map<List<CategoryPermissionDto>>(categoryRolePermissions);
        rolePermissions.ForEach(p => p.Type = "Role");
        result.AddRange(rolePermissions);

        var categoryUserPermissions = await categoryUserPermissionRepository
            .AllIncluding(c => c.User)
            .Where(c => c.CategoryId == request.CategoryId)
            .ToListAsync();
        var userPermissions = mapper.Map<List<CategoryPermissionDto>>(categoryUserPermissions);
        userPermissions.ForEach(p => p.Type = "User");
        result.AddRange(userPermissions);
        return result;
    }
}

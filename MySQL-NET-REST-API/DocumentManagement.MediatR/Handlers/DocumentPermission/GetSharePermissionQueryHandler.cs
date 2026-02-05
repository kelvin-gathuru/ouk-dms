using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.MediatR.Handlers;
public class GetSharePermissionQueryHandler(
    IDocumentRolePermissionRepository documentRolePermissionRepository,
    IDocumentUserPermissionRepository documentUserPermissionRepository,
    ICategoryRolePermissionRepository categoryRolePermissionRepository,
    ICategoryUserPermissionRepository categoryUserPermissionRepository,
    IMapper mapper
    ) : IRequestHandler<GetSharePermissionQuery, SharePermissionDto>
{
    public async Task<SharePermissionDto> Handle(GetSharePermissionQuery request, CancellationToken cancellationToken)
    {
        var result = new SharePermissionDto();

        // Document Role Permissions
        var documentRolePermissions = await documentRolePermissionRepository
            .AllIncluding(c => c.Role)
            .Where(c => c.DocumentId == request.DocumentId)
            .ToListAsync();
        var roleDocumentPermissions = mapper.Map<List<DocumentPermissionDto>>(documentRolePermissions);
        roleDocumentPermissions.ForEach(p => p.Type = "Role");
        result.DocumentPermissions.AddRange(roleDocumentPermissions);

        // Document User Permissions
        var documentUserPermissions = await documentUserPermissionRepository
            .AllIncluding(c => c.User)
            .Where(c => c.DocumentId == request.DocumentId)
            .ToListAsync();
        var userDocumentPermissions = mapper.Map<List<DocumentPermissionDto>>(documentUserPermissions);
        userDocumentPermissions.ForEach(p => p.Type = "User");
        result.DocumentPermissions.AddRange(userDocumentPermissions);

        // Category Role Permissions
        var categoryRolePermissions = await categoryRolePermissionRepository
            .AllIncluding(c => c.Role)
            .Where(c => c.CategoryId == request.CategoryId)
            .ToListAsync();
        var roleCategoryPermissions = mapper.Map<List<CategoryPermissionDto>>(categoryRolePermissions);
        roleCategoryPermissions.ForEach(p => p.Type = "Role");
        result.CategoryPermissions.AddRange(roleCategoryPermissions);

        // Category User Permissions
        var categoryUserPermissions = await categoryUserPermissionRepository
            .AllIncluding(c => c.User)
            .Where(c => c.CategoryId == request.CategoryId)
            .ToListAsync();
        var userCategoryPermissions = mapper.Map<List<CategoryPermissionDto>>(categoryUserPermissions);
        userCategoryPermissions.ForEach(p => p.Type = "User");
        result.CategoryPermissions.AddRange(userCategoryPermissions);

        return result;
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.MediatR.Handlers;

public class GetDocumentsByCategoryQueryHandler(UserInfoToken _userInfoToken, IUserRepository _userRepository, ICategoryRepository _categoryRepository) : IRequestHandler<GetDocumentsByCategoryQuery, List<DocumentByCategory>>
{
    public async Task<List<DocumentByCategory>> Handle(GetDocumentsByCategoryQuery request, CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow;
        var user = await _userRepository.AllIncluding(c => c.UserRoles).FirstOrDefaultAsync(c => c.Id == _userInfoToken.Id);
        var userRoles = user.UserRoles.Select(c => c.RoleId).ToList();
        var categories = new List<DocumentByCategory>();
        if (user.IsSuperAdmin)
        {
            categories = await _categoryRepository.All
           .Where(c => c.ParentId == null)
           .Select(c => new DocumentByCategory
           {
               CategoryName = c.Name,
               DocumentCount = c.Documents.Count()
           })
           .OrderByDescending(cs => cs.DocumentCount)
           .Take(10)
           .ToListAsync();
        }
        else
        {
            categories = await _categoryRepository
                .AllIncluding(c => c.CategoryUserPermissions, c => c.CategoryRolePermissions)
                .AsNoTracking()
                .Where(d => !d.IsDeleted && !d.IsArchive &&
                   (d.CategoryUserPermissions.Any(c => c.UserId == user.Id && c.ParentId == null && (!c.IsTimeBound || (c.IsTimeBound && c.StartDate < today && c.EndDate > today))) ||
                    d.CategoryRolePermissions.Any(c => userRoles.Contains(c.RoleId) && c.ParentId == null && (!c.IsTimeBound || (c.IsTimeBound && c.StartDate < today && c.EndDate > today)))))
               .Select(c => new DocumentByCategory
               {
                   CategoryName = c.Name,
                   DocumentCount = c.Documents.Count()
               })
               .OrderByDescending(c => c.DocumentCount)
               .Take(10)
               .ToListAsync();
        }
        return categories;
    }
}

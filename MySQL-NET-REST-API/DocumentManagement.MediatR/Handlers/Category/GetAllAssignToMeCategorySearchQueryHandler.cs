using System;
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

namespace DocumentManagement.MediatR.Handlers.Category;
public class GetAllAssignToMeCategorySearchQueryHandler(IDocumentRepository _documentRepository, IUserRepository userRepository, UserInfoToken userInfoToken, ICategoryRepository _categoryRepository, IMapper _mapper) : IRequestHandler<GetAllAssignToMeCategorySearchQuery, List<CategoryDto>>
{
    public async Task<List<CategoryDto>> Handle(GetAllAssignToMeCategorySearchQuery request, CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow;
        var user = await userRepository.AllIncluding(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.Id == userInfoToken.Id);
        var userRoles = user.UserRoles.Select(r => r.RoleId).ToList();
        var directlySharedParentIds = await _categoryRepository.All
            .AsNoTracking()
            .Where(c =>
                !c.IsDeleted && !c.IsArchive &&
                (
                    c.CategoryUserPermissions.Any(p => p.UserId == user.Id && (!p.IsTimeBound || (p.StartDate < today && p.EndDate > today))) ||
                    c.CategoryRolePermissions.Any(p => userRoles.Contains(p.RoleId) && (!p.IsTimeBound || (p.StartDate < today && p.EndDate > today)))
                )
            )
            .Select(c => c.Id)
            .ToListAsync();
        var allCategoryIds = new HashSet<Guid>(directlySharedParentIds);

        foreach (var parentId in directlySharedParentIds)
        {
            var childCategories = _categoryRepository.GetAllDescendantsUsingCTEByParentId(parentId);
            foreach (var child in childCategories)
            {
                allCategoryIds.Add(child.Id);
            }
        }

        var sharedDocCategoryIds = await _documentRepository.All
            .AsNoTracking()
            .Where(d =>
                !d.IsDeleted && !d.IsArchive &&
                (
                    d.DocumentUserPermissions.Any(p => p.UserId == user.Id && (!p.IsTimeBound || (p.StartDate < today && p.EndDate > today))) ||
                    d.DocumentRolePermissions.Any(p => userRoles.Contains(p.RoleId) && (!p.IsTimeBound || (p.StartDate < today && p.EndDate > today)))
                )
            )
            .Select(d => d.CategoryId)
            .Distinct()
            .ToListAsync();

        foreach (var docCatId in sharedDocCategoryIds)
        {
            allCategoryIds.Add(docCatId);
        }

        var allCategories = await _categoryRepository.All
            .Include(c => c.CreatedByUser)
            .AsNoTracking()
            .Where(c => allCategoryIds.Contains(c.Id))
            .OrderBy(c => c.Name)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                ParentId = c.ParentId,
                IsArchive = c.IsArchive,
                CreatedDate = c.CreatedDate,
                IsShared = true,
                CreatedUserName = c.CreatedByUser.FirstName + " " + c.CreatedByUser.LastName,
                Children = new List<CategoryDto>()
            })
            .ToListAsync();

        var categoryDict = allCategories.ToDictionary(c => c.Id, c => c);

        List<CategoryDto> tree = new();

        foreach (var category in allCategories)
        {
            if (category.ParentId != null && categoryDict.ContainsKey(category.ParentId.Value))
            {
                categoryDict[category.ParentId.Value].Children.Add(category);
            }
            else
            {
                tree.Add(category);
            }
        }

        return tree;

    }

}

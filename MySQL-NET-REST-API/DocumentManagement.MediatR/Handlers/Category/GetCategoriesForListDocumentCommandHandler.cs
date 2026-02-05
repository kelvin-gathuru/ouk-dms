using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;
public class GetCategoriesForListDocumentCommandHandler(
    ICategoryRepository categoryRepository,
    IUserRepository userRepository,
    UserInfoToken userInfoToken) : IRequestHandler<GetCategoriesForListDocumentCommand, List<CategoryDto>>
{
    public async Task<List<CategoryDto>> Handle(GetCategoriesForListDocumentCommand request, CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow;
        var user = await userRepository.AllIncluding(c => c.UserRoles).FirstOrDefaultAsync(c => c.Id == userInfoToken.Id);
        var userRoles = user.UserRoles.Select(c => c.RoleId).ToList();

        var flatCategories = categoryRepository.AllIncluding(
          c => c.CategoryUserPermissions,
          c => c.CategoryRolePermissions,
          c => c.Documents // Include related documents to check document-level permissions
      )
      .Where(c => !c.IsDeleted && !c.IsArchive &&
          (
              // Category-level permissions
              c.CategoryUserPermissions.Any(p => p.UserId == user.Id &&
                  (!p.IsTimeBound || (p.StartDate < today && p.EndDate > today))) ||
              c.CategoryRolePermissions.Any(p => userRoles.Contains(p.RoleId) &&
                  (!p.IsTimeBound || (p.StartDate < today && p.EndDate > today))) ||

              // Document-level permissions affecting category access
              c.Documents.Any(d =>
                  !d.IsArchive && !d.IsDeleted &&
                  (
                      d.DocumentUserPermissions.Any(p => p.UserId == user.Id &&
                          (!p.IsTimeBound || (p.StartDate < today && p.EndDate > today))) ||
                      d.DocumentRolePermissions.Any(p => userRoles.Contains(p.RoleId) &&
                          (!p.IsTimeBound || (p.StartDate < today && p.EndDate > today)))
                  )
              )
          )
      )
      .Select(c => new CategoryDto
      {
          Id = c.Id,
          Name = c.Name,
          ParentId = c.ParentId,
      })
      .Distinct() // Ensure unique categories
      .ToList();

        if (flatCategories.Count == 0)
        {
            return new List<CategoryDto>();
        }
        return BuildCategoryHierarchy(flatCategories);


    }

    public List<CategoryDto> BuildCategoryHierarchy(List<CategoryDto> flatCategories)
    {
        var categoryDict = flatCategories.ToDictionary(c => c.Id); // Convert list to Dictionary for O(1) lookup
        List<CategoryDto> rootCategories = new List<CategoryDto>();

        foreach (var category in flatCategories)
        {
            if (category.ParentId == null || !categoryDict.ContainsKey(category.ParentId.Value))
            {
                // Root category (ParentId is null or missing in the list)
                rootCategories.Add(category);
            }
            else
            {
                // Add as a child of its parent
                var parent = categoryDict[category.ParentId.Value];
                parent.Children.Add(category);
            }
        }

        return rootCategories;
    }

}

using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
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
public class GetArchiveCategoriesByParentIdCommandHandler(UserInfoToken userInfoToken,
    ICategoryRepository categoryRepository
    ) : IRequestHandler<GetArchiveCategoriesByParentIdCommand, ServiceResponse<List<CategoryDto>>>
{
    public async Task<ServiceResponse<List<CategoryDto>>> Handle(GetArchiveCategoriesByParentIdCommand request, CancellationToken cancellationToken)
    {
        var categories = new List<CategoryDto>();
        var today = DateTime.UtcNow;

        if (request.ParentId == null || request.ParentId == Guid.Empty)
        {
            var categoryQuery = categoryRepository.AllIncluding(c => c.CreatedByUser, c => c.CategoryUserPermissions, c => c.CategoryRolePermissions)
            .Where(c => c.IsArchive && c.ArchiveParentId == null);

            if (!userInfoToken.IsSuperAdmin)
            {
                categoryQuery = categoryQuery.Where(c => c.CreatedBy == userInfoToken.Id || c.ArchiveById == userInfoToken.Id);
            }
            categories = await categoryQuery.OrderBy(c => c.Name)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                ParentId = c.ParentId,
                CreatedDate = c.CreatedDate,
                IsArchive = c.IsArchive,
                CreatedUserName = c.CreatedByUser.FirstName + " " + c.CreatedByUser.LastName,
                IsShared = c.CategoryUserPermissions.Any(c => c.ParentId == null &&
                (!c.IsTimeBound || (c.StartDate < today && c.EndDate > today)))
                ||
                             c.CategoryRolePermissions.Any(c => c.ParentId == null &&
                         (!c.IsTimeBound || (c.StartDate < today && c.EndDate > today)))

            }).ToListAsync();

        }
        else
        {
            var categoryQuery = categoryRepository.AllIncluding(c => c.CreatedByUser, c => c.CategoryUserPermissions, c => c.CategoryRolePermissions)
                .Where(c => c.ParentId == request.ParentId && c.IsArchive);

            if (!userInfoToken.IsSuperAdmin)
            {
                categoryQuery = categoryQuery.Where(c => c.CreatedBy == userInfoToken.Id);
            }
            categories = await categoryQuery.OrderBy(c => c.Name)
              .Select(c => new CategoryDto
              {
                  Id = c.Id,
                  Name = c.Name,
                  ParentId = c.ParentId,
                  CreatedDate = c.CreatedDate,
                  IsArchive = c.IsArchive,
                  CreatedUserName = c.CreatedByUser.FirstName + " " + c.CreatedByUser.LastName,
                  IsShared = c.CategoryUserPermissions.Any(c => c.ParentId == null &&
              (!c.IsTimeBound || (c.StartDate < today && c.EndDate > today)))
                ||
                             c.CategoryRolePermissions.Any(c => c.ParentId == null &&
                         (!c.IsTimeBound || (c.StartDate < today && c.EndDate > today)))
              }).ToListAsync();
        }

        return ServiceResponse<List<CategoryDto>>.ReturnResultWith200(categories);
    }
}

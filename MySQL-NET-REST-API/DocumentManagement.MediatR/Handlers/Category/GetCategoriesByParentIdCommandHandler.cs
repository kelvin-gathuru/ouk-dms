using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.MediatR.Handlers;

public class GetCategoriesByParentIdCommandHandler(
    ICategoryRepository categoryRepository
    ) : IRequestHandler<GetCategoriesByParentIdCommand, ServiceResponse<List<CategoryDto>>>
{
    public async Task<ServiceResponse<List<CategoryDto>>> Handle(GetCategoriesByParentIdCommand request, CancellationToken cancellationToken)
    {
        var categories = new List<CategoryDto>();
        var today = DateTime.UtcNow;

        if (request.ParentId == null || request.ParentId == Guid.Empty)
        {
            categories = await categoryRepository.AllIncluding(c => c.CreatedByUser, c => c.CategoryUserPermissions, c => c.CategoryRolePermissions)
                .Where(c => c.ParentId == null && !c.IsArchive).OrderBy(c => c.Name)
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
            categories = await categoryRepository.AllIncluding(c => c.CreatedByUser, c => c.CategoryUserPermissions, c => c.CategoryRolePermissions)
                .Where(c => c.ParentId == request.ParentId && !c.IsArchive).OrderBy(c => c.Name)
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

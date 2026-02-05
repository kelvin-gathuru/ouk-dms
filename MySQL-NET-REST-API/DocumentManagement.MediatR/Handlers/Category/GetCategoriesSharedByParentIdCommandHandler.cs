using AutoMapper;
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

public class GetCategoriesSharedByParentIdCommandHandler(
    ICategoryRepository categoryRepository,
    IMapper mapper,
     UserInfoToken userInfoToken,
      IUserRepository userRepository
    ) : IRequestHandler<GetCategoriesSharedByParentIdCommand, ServiceResponse<List<CategoryDto>>>
{
    public async Task<ServiceResponse<List<CategoryDto>>> Handle(GetCategoriesSharedByParentIdCommand request, CancellationToken cancellationToken)
    {
        var categories = new List<CategoryDto>();

        if (request.ParentId == null || request.ParentId == Guid.Empty)
        {
            var today = DateTime.UtcNow;
            var user = await userRepository.AllIncluding(c => c.UserRoles).FirstOrDefaultAsync(c => c.Id == userInfoToken.Id);
            var userRoles = user.UserRoles.Select(c => c.RoleId).ToList();

            categories = await categoryRepository.All
                .Include(c => c.CreatedByUser)
             .AsNoTracking()
            .Where(d => !d.IsDeleted && !d.IsArchive &&
                (d.CategoryUserPermissions.Any(c => c.UserId == user.Id && c.ParentId == null &&
                     (!c.IsTimeBound || (c.StartDate < today && c.EndDate > today))) ||
                 d.CategoryRolePermissions.Any(c => userRoles.Contains(c.RoleId) && c.ParentId == null &&
                     (!c.IsTimeBound || (c.StartDate < today && c.EndDate > today)))))
             .OrderBy(c => c.Name)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                ParentId = c.ParentId,
                IsArchive = c.IsArchive,
                CreatedDate = c.CreatedDate,
                IsShared = true,
                CreatedUserName = c.CreatedByUser.FirstName + " " + c.CreatedByUser.LastName
            })
            .ToListAsync();

        }
        else
        {

            var today = DateTime.UtcNow;
            var user = await userRepository.AllIncluding(c => c.UserRoles).FirstOrDefaultAsync(c => c.Id == userInfoToken.Id);
            var userRoles = user.UserRoles.Select(c => c.RoleId).ToList();

            categories = await categoryRepository.All.Include(c => c.CreatedByUser)
                    .AsNoTracking()
                    .Where(d => !d.IsDeleted && d.ParentId == request.ParentId && !d.IsArchive &&
                        (d.CategoryUserPermissions.Any(c => c.UserId == user.Id &&
                        (!c.IsTimeBound || (c.StartDate < today && c.EndDate > today))) ||
                         d.CategoryRolePermissions.Any(c => userRoles.Contains(c.RoleId) &&
                         (!c.IsTimeBound || (c.StartDate < today && c.EndDate > today)))))
                    .OrderBy(c => c.Name)
                      .Select(c => new CategoryDto
                      {
                          Id = c.Id,
                          Name = c.Name,
                          ParentId = c.ParentId,
                          IsArchive = c.IsArchive,
                          CreatedDate = c.CreatedDate,
                          IsShared = c.CategoryUserPermissions.Any(c => c.UserId == user.Id && c.ParentId == null &&
                                (!c.IsTimeBound || (c.StartDate < today && c.EndDate > today)))
                          ||
                                 c.CategoryRolePermissions.Any(c => userRoles.Contains(c.RoleId)
                             && c.ParentId == null &&
                             (!c.IsTimeBound || (c.StartDate < today && c.EndDate > today))),
                          CreatedUserName = c.CreatedByUser.FirstName + " " + c.CreatedByUser.LastName
                      }).ToListAsync();
        }


        return ServiceResponse<List<CategoryDto>>.ReturnResultWith200(categories);

    }
}


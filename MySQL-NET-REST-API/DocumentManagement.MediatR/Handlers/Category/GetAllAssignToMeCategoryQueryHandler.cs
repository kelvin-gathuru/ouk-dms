using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;

public class GetAllAssignToMeCategoryQueryHandler(
    ICategoryRepository categoryRepository,
    UserInfoToken userInfoToken,
    IUserRepository userRepository,
    ICategoryRepository _categoryRepository,
    IMapper _mapper) : IRequestHandler<GetAllAssignToMeCategoryQuery, List<CategoryDto>>
{
    public async Task<List<CategoryDto>> Handle(GetAllAssignToMeCategoryQuery request, CancellationToken cancellationToken)
    {
        var categories = new List<CategoryDto>();
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

        if (categories == null || categories.Count == 0)
        {
            return new List<CategoryDto>();
        }
        categories.ForEach(c =>
        {
            c.Children = _categoryRepository.GetAllDescendantsUsingCTEByParentId(c.Id);
        });


        return _mapper.Map<List<CategoryDto>>(categories);
    }

}

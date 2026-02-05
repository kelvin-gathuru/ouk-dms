using System;
using System.Collections.Generic;
using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;

namespace DocumentManagement.Repository;

public interface ICategoryRepository : IGenericRepository<Category>
{
    List<Guid> GetAllChildCategoryIdsUsingRawSql(Guid parentId);
    List<CategoryDto> GetAllDescendantsUsingCTE();
    List<CategoryDto> GetAllAssignedToMeDescendantsUsingCTE(Guid userId);
    List<CategoryDto> GetAllAssignedToMeSearchDropDownDescendantsUsingCTE(Guid userId);
    List<Category> GetAllChildrenAsync(Guid parentId);
    List<CategoryDto> GetAllDescendantsUsingCTEByParentId(Guid parentId);
}

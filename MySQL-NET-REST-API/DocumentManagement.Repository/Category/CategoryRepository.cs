using System;
using System.Collections.Generic;
using System.Linq;
using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace DocumentManagement.Repository;

public class CategoryRepository : GenericRepository<Category, DocumentContext>,
       ICategoryRepository
{
    public CategoryRepository(
        IUnitOfWork<DocumentContext> uow
        ) : base(uow)
    {
    }


    public List<Guid> GetAllChildCategoryIdsUsingRawSql(Guid parentId)
    {
        var query = @"
             WITH RECURSIVE RecursiveCategories AS (
                 SELECT Id,Name, ParentId, IsDeleted,IsArchive
                 FROM Categories
                 WHERE ParentId = {0} AND IsDeleted = 0
                 UNION ALL
                 SELECT c.Id,c.Name, c.ParentId, c.IsDeleted, c.IsArchive
                 FROM Categories c
                 INNER JOIN RecursiveCategories rc ON c.ParentId = rc.Id
    
             )
             SELECT Id,Name, ParentId, IsArchive FROM RecursiveCategories WHERE  IsDeleted=0;
    ";

        return _uow.Context.CustomCategories
            .FromSqlRaw(query, parentId)
            .AsEnumerable()
            .Select(c => c.Id)
            .ToList();
    }

    public List<CategoryDto> GetAllDescendantsUsingCTE()
    {
        var query = @"
         WITH RECURSIVE RecursiveCategories AS (
         SELECT Id, Name, ParentId, IsDeleted, IsArchive
         FROM Categories
         WHERE ParentId IS NULL AND IsDeleted = 0 AND IsArchive = 0
         UNION ALL
         SELECT c.Id, c.Name, c.ParentId, c.IsDeleted, c.IsArchive
         FROM Categories c
         INNER JOIN RecursiveCategories rc ON c.ParentId = rc.Id
         WHERE c.IsArchive = 0 AND c.IsDeleted = 0
     )
     SELECT Id, Name, ParentId, IsArchive FROM RecursiveCategories WHERE IsDeleted = 0;
    ";

        var flatCategories = _uow.Context.CustomCategories
            .FromSqlRaw(query)
            .AsEnumerable()
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                ParentId = c.ParentId,
                IsArchive = c.IsArchive,
                Children = new List<CategoryDto>()
            })
            .ToList();

        var categoryIds = flatCategories.Select(c => c.Id).ToList(); // No need to filter IsArchive here as all are already non-archived
        var sharedCategoryIds = _uow.Context.CategoryRolePermissions
            .Where(crp => categoryIds.Contains(crp.CategoryId))
            .Select(crp => crp.CategoryId)
            .Union(
                _uow.Context.CategoryUserPermissions
                .Where(cup => categoryIds.Contains(cup.CategoryId))
                .Select(cup => cup.CategoryId)
            )
            .ToHashSet();

        foreach (var category in flatCategories)
        {
            category.IsShared = sharedCategoryIds.Contains(category.Id);
        }

        var categoryLookup = flatCategories.ToLookup(c => c.ParentId);
        foreach (var category in flatCategories)
        {
            category.Children = categoryLookup[category.Id].ToList();
        }

        return categoryLookup[null].ToList();
    }



    public List<CategoryDto> GetAllDescendantsUsingCTEByParentId(Guid parentId)
    {
        var query = @"
            WITH RECURSIVE RecursiveCategories AS (
                SELECT Id, Name, ParentId, IsDeleted, IsArchive
                FROM Categories
                WHERE ParentId = @parentId AND IsDeleted = 0
                UNION ALL
                SELECT c.Id, c.Name, c.ParentId, c.IsDeleted, c.IsArchive
                FROM Categories c
                INNER JOIN RecursiveCategories rc ON c.ParentId = rc.Id
            )
            SELECT Id, Name, ParentId, IsArchive FROM RecursiveCategories WHERE IsDeleted = 0;
        ";

        var flatCategories = _uow.Context.CustomCategories
            .FromSqlRaw(query, new MySqlParameter("@parentId", parentId))
            .AsEnumerable()
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                ParentId = c.ParentId,
                IsArchive = c.IsArchive,
                Children = new List<CategoryDto>()
            })
            .ToList();

        var categoryLookup = flatCategories.ToLookup(c => c.ParentId);
        foreach (var category in flatCategories)
        {
            category.Children = categoryLookup[category.Id].ToList();
        }

        return categoryLookup[parentId].ToList();
    }

    public List<CategoryDto> GetAllAssignedToMeDescendantsUsingCTE(Guid userId)
    {
        var query = @"
            WITH RECURSIVE RecursiveCategories AS (
                SELECT Id, Name, ParentId, IsDeleted ,IsArchive
                FROM Categories
                WHERE ParentId IS NULL AND IsDeleted = 0 AND IsArchive = 0
                UNION ALL
                SELECT c.Id, c.Name, c.ParentId, c.IsDeleted, c.IsArchive
                FROM Categories c
                INNER JOIN RecursiveCategories rc ON c.ParentId = rc.Id
                    WHERE rc.IsArchive = 0 AND rc.IsDeleted = 0
            )
            SELECT Id, Name, ParentId, IsArchive FROM RecursiveCategories WHERE IsDeleted = 0 AND IsArchive = 0;
            ";

        var flatCategories = _uow.Context.CustomCategories
            .FromSqlRaw(query)
            .AsEnumerable()
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                ParentId = c.ParentId,
                Children = new List<CategoryDto>()
            })
            .ToList();

        var userRoles = _uow.Context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.RoleId)
            .ToList();

        var categoryIds = flatCategories.Select(c => c.Id).ToList();
        var assignedCategoryIds = _uow.Context.CategoryUserPermissions
            .Where(cup => cup.UserId == userId && categoryIds.Contains(cup.CategoryId))
            .Select(cup => cup.CategoryId)
            .Union(
                _uow.Context.CategoryRolePermissions
                .Where(crp => userRoles.Contains(crp.RoleId) && categoryIds.Contains(crp.CategoryId))
                .Select(crp => crp.CategoryId)
            )
            .ToHashSet();

        foreach (var category in flatCategories)
        {
            category.IsAssignedToMe = assignedCategoryIds.Contains(category.Id);
        }

        var categoryLookup = flatCategories.ToLookup(c => c.ParentId);
        foreach (var category in flatCategories)
        {
            category.Children = categoryLookup[category.Id].ToList();
        }

        var result = flatCategories.Where(c => (bool)c.IsAssignedToMe).ToList();
        result.ForEach(category =>
        {
            category.Children = category.Children.Where(child => (bool)child.IsAssignedToMe).ToList();
        });

        return result;
    }

    public List<CategoryDto> GetAllAssignedToMeSearchDropDownDescendantsUsingCTE(Guid userId)
    {
        var query = @"
            WITH RECURSIVE RecursiveCategories AS (
                SELECT Id, Name, ParentId, IsDeleted, IsArchive
                FROM Categories
                WHERE ParentId IS NULL AND IsDeleted = 0
                UNION ALL
                SELECT c.Id, c.Name, c.ParentId, c.IsDeleted, c.IsArchive
                FROM Categories c
                INNER JOIN RecursiveCategories rc ON c.ParentId = rc.Id
            )
            SELECT Id, Name, ParentId, IsArchive FROM RecursiveCategories WHERE IsDeleted = 0;
            ";

        var flatCategories = _uow.Context.CustomCategories
            .FromSqlRaw(query)
            .AsEnumerable()
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                ParentId = c.ParentId,
                Children = new List<CategoryDto>()
            })
            .ToList();

        var userRoles = _uow.Context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.RoleId)
            .ToList();

        var categoryIds = flatCategories.Select(c => c.Id).ToList();
        var assignedCategoryIds = _uow.Context.CategoryUserPermissions
            .Where(cup => cup.UserId == userId && categoryIds.Contains(cup.CategoryId))
            .Select(cup => cup.CategoryId)
            .Union(
                _uow.Context.CategoryRolePermissions
                .Where(crp => userRoles.Contains(crp.RoleId) && categoryIds.Contains(crp.CategoryId))
                .Select(crp => crp.CategoryId)
            )
            .Union(
                _uow.Context.DocumentUserPermissions
                .Where(dup => dup.UserId == userId && categoryIds.Contains(dup.Document.CategoryId))
                .Select(dup => dup.Document.CategoryId)
            )
            .Union(
                _uow.Context.DocumentRolePermissions
                .Where(drp => userRoles.Contains(drp.RoleId) && categoryIds.Contains(drp.Document.CategoryId))
                .Select(drp => drp.Document.CategoryId)
            )
            .ToHashSet();

        foreach (var category in flatCategories)
        {
            category.IsAssignedToMe = assignedCategoryIds.Contains(category.Id);
        }

        var categoryLookup = flatCategories.ToLookup(c => c.ParentId);
        foreach (var category in flatCategories)
        {
            category.Children = categoryLookup[category.Id].ToList();
        }

        var result = flatCategories.Where(c => (bool)c.IsAssignedToMe).ToList();
        result.ForEach(category =>
        {
            category.Children = category.Children.Where(child => (bool)child.IsAssignedToMe).ToList();
        });

        return result;
    }

    public List<Category> GetAllChildrenAsync(Guid parentId)
    {
        var result = new List<Category>();
        var children = _uow.Context.Categories
            .Where(c => c.ParentId == parentId && !c.IsDeleted)
            .AsNoTracking()
            .ToList();

        result.AddRange(children);

        foreach (var child in children)
        {
            result.AddRange(GetAllChildrenAsync(child.Id));
        }

        return result;
    }
}

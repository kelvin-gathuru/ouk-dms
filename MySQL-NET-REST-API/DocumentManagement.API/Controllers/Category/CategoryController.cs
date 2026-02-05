using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthChecker;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.API.Controllers;

/// <summary>
/// Category
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoryController : ControllerBase
{
    public IMediator _mediator { get; set; }
    /// <summary>
    /// Category
    /// </summary>
    /// <param name="mediator"></param>
    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }
    /// <summary>
    /// Get Specific Category by ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [Produces("application/json", "application/xml", Type = typeof(CategoryDto))]
    public async Task<IActionResult> GetCategory(Guid id)
    {
        var getCategoryQuery = new GetCategoryQuery
        {
            Id = id
        };
        var result = await _mediator.Send(getCategoryQuery);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id}/children")]
    [Produces("application/json", "application/xml", Type = typeof(CategoryDto))]
    public async Task<IActionResult> GetCategoriesByParentId(Guid id)
    {
        var getCategoriesByParentIdCommand = new GetCategoriesByParentIdCommand
        {
            ParentId = id
        };
        var result = await _mediator.Send(getCategoriesByParentIdCommand);
        return StatusCode(result.StatusCode, result.Data);
    }

    [HttpGet("{id}/shared/children")]
    [Produces("application/json", "application/xml", Type = typeof(CategoryDto))]
    public async Task<IActionResult> GetCategoriesSharedByParentId(Guid id)
    {
        var getCategoriesSharedByParentIdCommand = new GetCategoriesSharedByParentIdCommand
        {
            ParentId = id
        };
        var result = await _mediator.Send(getCategoriesSharedByParentIdCommand);
        return StatusCode(result.StatusCode, result.Data);
    }
    [HttpGet("{id}/hierarchical")]
    [Produces("application/json", "application/xml", Type = typeof(CategoryDto))]
    public async Task<IActionResult> GetCategoriesHierarchicalBChildId(Guid id)
    {
        var getCategoriesHierarchicalBChildIdCommand = new GetCategoriesHierarchicalBChildIdCommand
        {
            ChildId = id
        };
        var result = await _mediator.Send(getCategoriesHierarchicalBChildIdCommand);
        return StatusCode(result.StatusCode, result.Data);
    }
    [HttpGet("{id}/hierarchical/windowshared")]
    [Produces("application/json", "application/xml", Type = typeof(CategoryDto))]
    public async Task<IActionResult> GetCategoriesHierarchicalBChildIdForWindowShared(Guid id)
    {
        var getCategoriesHierarchicalBChildIdWindowSharedCommand = new GetCategoriesHierarchicalBChildIdWindowSharedCommand
        {
            Id = id
        };
        var result = await _mediator.Send(getCategoriesHierarchicalBChildIdWindowSharedCommand);
        return StatusCode(result.StatusCode, result.Data);
    }
    /// <summary>
    /// Get All Categories.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Produces("application/json", "application/xml", Type = typeof(List<CategoryDto>))]
    public async Task<IActionResult> GetCategories()
    {
        var getAllCategoryQuery = new GetAllCategoryQuery { IsParentOnly = true };
        var result = await _mediator.Send(getAllCategoryQuery);
        return Ok(result);
    }
    /// <summary>
    /// Create a Category.
    /// </summary>
    [HttpPost]
    [Produces("application/json", "application/xml", Type = typeof(CategoryDto))]
    // [ClaimCheck("create_category")]
    public async Task<IActionResult> AddCategory([FromBody] AddCategoryCommand addCategoryCommand)
    {
        var result = await _mediator.Send(addCategoryCommand);
        if (result.StatusCode != 200)
        {
            return StatusCode(result.StatusCode, result);
        }
        return CreatedAtAction("GetCategory", new { id = result.Id }, result);
    }
    [HttpPost("AssignToMe")]
    [Produces("application/json", "application/xml", Type = typeof(CategoryDto))]
    public async Task<IActionResult> AddCategoryAssignToMe([FromBody] AddCategoryAssignToMeCommand addCategoryAssignToMeCommand)
    {
        var result = await _mediator.Send(addCategoryAssignToMeCommand);
        if (result.StatusCode != 200)
        {
            return StatusCode(result.StatusCode, result);
        }
        return CreatedAtAction("GetCategory", new { id = result.Id }, result);
    }
    [HttpPut("AssignToMe/{Id}")]
    [Produces("application/json", "application/xml", Type = typeof(CategoryDto))]
    public async Task<IActionResult> UpdateCategoryAssignToMe(Guid Id, [FromBody] UpdateCategoryAssignToMeCommand category)
    {
        category.Id = Id;
        var result = await _mediator.Send(category);
        return StatusCode(result.StatusCode, result);

    }
    /// <summary>
    /// Update Category.
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="updateCategoryCommand"></param>
    /// <returns></returns>
    [HttpPut("{Id}")]
    [Produces("application/json", "application/xml", Type = typeof(CategoryDto))]
    // [ClaimCheck("edit_category")]
    public async Task<IActionResult> UpdateCategory(Guid Id, [FromBody] UpdateCategoryCommand category)
    {
        category.Id = Id;
        var result = await _mediator.Send(category);
        return StatusCode(result.StatusCode, result);

    }
    /// <summary>
    /// Delete Category.
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    [HttpDelete("{Id}")]
    // [ClaimCheck("delete_category")]
    public async Task<IActionResult> DeleteCategory(Guid Id)
    {
        var deleteCategoryCommand = new DeleteCategoryCommand
        {
            Id = Id
        };
        var result = await _mediator.Send(deleteCategoryCommand);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id}/subcategories")]
    [Produces("application/json", "application/xml", Type = typeof(List<CategoryDto>))]
    public async Task<IActionResult> GetSubCategories(Guid id)
    {
        var getSubCategoriesQuery = new GetSubCategoriesQuery { Id = id };
        var result = await _mediator.Send(getSubCategoriesQuery);
        return Ok(result);
    }
    [HttpGet("{id}/AllDescendantsChilds")]
    [Produces("application/json", "application/xml", Type = typeof(List<CategoryDto>))]
    public async Task<IActionResult> GetAllDescendantsUsingCTEByParentId(Guid id)
    {
        var getAllDescendantsUsingCTEByParentIdCommand = new GetAllDescendantsUsingCTEByParentIdCommand { ParentId = id };
        var result = await _mediator.Send(getAllDescendantsUsingCTEByParentIdCommand);
        return Ok(result.Data);
    }

    [HttpGet("dropdown")]
    [Produces("application/json", "application/xml", Type = typeof(List<CategoryDto>))]
    public async Task<IActionResult> GetAllCategoriesForDropDown()
    {
        var getAllCategoryQuery = new GetAllCategoryQuery { IsParentOnly = false };
        var result = await _mediator.Send(getAllCategoryQuery);
        return Ok(result);
    }

    [HttpGet("AssignToMe/dropdown")]
    [Produces("application/json", "application/xml", Type = typeof(List<CategoryDto>))]
    public async Task<IActionResult> GetAllAssignToMeCategoriesForDropDown()
    {
        var getAllAssignToMeCategoryQuery = new GetAllAssignToMeCategoryQuery { IsParentOnly = false };
        var result = await _mediator.Send(getAllAssignToMeCategoryQuery);
        return Ok(result);
    }

    [HttpGet("AssignToMe/searchdropdown")]
    [Produces("application/json", "application/xml", Type = typeof(List<CategoryDto>))]
    public async Task<IActionResult> GetAllAssignToMeCategoriesSearchForDropDown()
    {
        var getCategoriesForListDocumentCommand = new GetCategoriesForListDocumentCommand { };
        var result = await _mediator.Send(getCategoriesForListDocumentCommand);
        return Ok(result);
    }

    [HttpPost("{Id}/archive")]
    // [ClaimCheck("Category_archive_folder", "all_archive_folder")]
    public async Task<IActionResult> ArchiveCategory(Guid Id)
    {
        var archiveCategoryCommand = new ArchiveCategoryCommand
        {
            CategoryId = Id
        };
        var result = await _mediator.Send(archiveCategoryCommand);
        return StatusCode(result.StatusCode, result);
    }
    [HttpPost("{Id}/AssignFolders/archive")]
    // [ClaimCheck("assigned_archive_folder")]
    public async Task<IActionResult> ArchiveAssignFolder(Guid Id)
    {
        var archieveCategoryFromAssignUserCommand = new ArchieveCategoryFromAssignUserCommand
        {
            CategoryId = Id
        };
        var result = await _mediator.Send(archieveCategoryFromAssignUserCommand);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{Id}/archive")]
    public async Task<IActionResult> GetArchieveFolders(Guid Id)
    {
        var getArchiveCategoriesByParentIdCommand = new GetArchiveCategoriesByParentIdCommand
        {
            ParentId = Id
        };
        var result = await _mediator.Send(getArchiveCategoriesByParentIdCommand);
        return Ok(result.Data);
    }

    [HttpPost("{Id}/restore")]
    // [ClaimCheck("restore_folder")]
    public async Task<IActionResult> RestoreCategory(Guid Id)
    {
        var restoreCategoryCommand = new RestoreCategoryCommand
        {
            CategoryId = Id
        };
        var result = await _mediator.Send(restoreCategoryCommand);
        return StatusCode(result.StatusCode, result);
    }
}

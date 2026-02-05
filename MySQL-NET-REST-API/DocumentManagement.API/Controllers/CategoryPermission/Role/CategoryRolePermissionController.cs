using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DocumentManagement.API.Controllers.CategoryPermission
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryRolePermissionController : ControllerBase
    {
        public IMediator _mediator { get; set; }
        /// <summary>
        /// DocumentRolePermission
        /// </summary>
        /// <param name="mediator"></param>
        public CategoryRolePermissionController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Get Category Permissions
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Produces("application/json", "application/xml", Type = typeof(List<CategoryPermissionDto>))]
        public async Task<IActionResult> GetCategoryPermissions(Guid id)
        {
            var getCategoryPermissionQuery = new GetCategoryPermissionQuery
            {
                CategoryId = id
            };
            var result = await _mediator.Send(getCategoryPermissionQuery);
            return Ok(result);
        }
     
        [HttpPost]
        [Produces("application/json", "application/xml", Type = typeof(CategoryRolePermissionDto))]
        public async Task<IActionResult> AddCategoryRolePermission(AddCategoryRolePermissionCommand addCategoryRolePermissionCommand)
        {
            var result = await _mediator.Send(addCategoryRolePermissionCommand);
            return StatusCode(result.StatusCode, result);
        }
        /// <summary>
        /// Shared Category to Users And Roles
        /// </summary>
        /// <param name="categoryPermissionUserRoleCommand"></param>
        /// <returns></returns>
        [HttpPost("multiple")]
        [Produces("application/json", "application/xml", Type = typeof(CategoryPermissionUserRoleCommand))]
        public async Task<IActionResult> AddMultipleCategoriesUsersAndRoles(CategoryPermissionUserRoleCommand categoryPermissionUserRoleCommand)
        {
            var result = await _mediator.Send(categoryPermissionUserRoleCommand);
            return Ok(result);
        }
        /// <summary>
        /// Delete Category Role Permission
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteCategoryRolePermission(Guid Id)
        {
            var deleteRolePermissionCommand = new DeleteCategoryRolePermissionCommand
            {
                Id = Id
            };
            var result = await _mediator.Send(deleteRolePermissionCommand);
            return StatusCode(result.StatusCode, result);
        }
    }
}

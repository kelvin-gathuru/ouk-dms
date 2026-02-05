using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DocumentManagement.API.Controllers.CategoryPermission
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryUserPermissionController : ControllerBase
    {
        public IMediator _mediator { get; set; }
        /// <summary>
        /// DocumentUserPermission
        /// </summary>
        /// <param name="mediator"></param>
        public CategoryUserPermissionController(IMediator mediator)
        {
            _mediator = mediator;
        }
  
        [HttpPost]
        [Produces("application/json", "application/xml", Type = typeof(CategoryUserPermissionDto))]
        public async Task<IActionResult> AddCategoryUserPermission(AddCategoryUserPermissionCommand addCategoryUserPermissionCommand)
        {
            var result = await _mediator.Send(addCategoryUserPermissionCommand);
            return StatusCode(result.StatusCode, result);
        }
        /// <summary>
        /// Delete Document User Permission By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteCategoryUserPermission(Guid Id)
        {
            var deleteCategoryUserPermissionCommand = new DeleteCategoryUserPermissionCommand
            {
                Id = Id
            };
            var result = await _mediator.Send(deleteCategoryUserPermissionCommand);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Gets the reminder by documentId.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet("{categoryId}/check")]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> CheckShareUserByCategoryId(Guid categoryId)
        {
            var command = new CheckShareUserByCategoryCommand()
            {
                CategoryId = categoryId
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}

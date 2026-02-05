using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using AuthChecker;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DocumentManagement.API.Controllers;

/// <summary>
/// Role
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RoleController : ControllerBase
{
    public IMediator _mediator { get; set; }
    private readonly ILogger<RoleController> _logger;
    /// <summary>
    /// Role
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="logger"></param>
    public RoleController(
        IMediator mediator,
        ILogger<RoleController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    /// <summary>
    /// Create A Role
    /// </summary>
    /// <param name="addRoleCommand"></param>
    /// <returns></returns>
    [HttpPost]
    [Produces("application/json", "application/xml", Type = typeof(RoleDto))]
    // [ClaimCheck("create_role")]
    public async Task<IActionResult> AddRole(AddRoleCommand addRoleCommand)
    {
        var result = await _mediator.Send(addRoleCommand);
        if (result.StatusCode != 200)
        {
            _logger.LogError(result.StatusCode,
                            JsonSerializer.Serialize(result), "");
            return StatusCode(result.StatusCode, result);
        }
        return CreatedAtAction("GetRole", new { id = result.Id }, result);
    }
    /// <summary>
    /// Update Exist Role By Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="updateRoleCommand"></param>
    /// <returns></returns>

    [HttpPut("{id}")]
    [Produces("application/json", "application/xml", Type = typeof(RoleDto))]
    // [ClaimCheck("edit_role")]
    public async Task<IActionResult> UpdateRole(Guid id, UpdateRoleCommand updateRoleCommand)
    {
        updateRoleCommand.Id = id;
        var result = await _mediator.Send(updateRoleCommand);
        if (result.StatusCode != 200)
        {
            _logger.LogError(result.StatusCode,
                            JsonSerializer.Serialize(result), "");
            return StatusCode(result.StatusCode, result);
        }
        return NoContent();
    }
    /// <summary>
    /// Get Role By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>

    [HttpGet("{id}", Name = "GetRole")]
    [Produces("application/json", "application/xml", Type = typeof(RoleDto))]
    public async Task<IActionResult> GetRole(Guid id)
    {
        _logger.LogTrace("GetRole");
        var getRoleQuery = new GetRoleQuery
        {
            Id = id
        };

        var result = await _mediator.Send(getRoleQuery);
        return StatusCode(result.StatusCode, result);
    }
    /// <summary>
    /// Get All Roles
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetRoles")]
    [Produces("application/json", "application/xml", Type = typeof(List<RoleDto>))]
    public async Task<IActionResult> GetRoles()
    {
        var getAllRoleQuery = new GetAllRoleQuery
        {
        };
        var result = await _mediator.Send(getAllRoleQuery);
        return Ok(result);
    }
    /// <summary>
    /// Delete Role By Id
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    [HttpDelete("{Id}")]
    [Produces("application/json", "application/xml", Type = typeof(List<RoleDto>))]
    // [ClaimCheck("delete_role")]
    public async Task<IActionResult> DeleteRole(Guid Id)
    {
        var deleteOperationCommand = new DeleteRoleCommand
        {
            Id = Id
        };
        var result = await _mediator.Send(deleteOperationCommand);
        return StatusCode(result.StatusCode, result);
    }
}

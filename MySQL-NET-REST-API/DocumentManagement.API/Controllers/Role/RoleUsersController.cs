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
/// RoleUsers
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RoleUsersController : ControllerBase
{
    public IMediator _mediator { get; set; }
    private readonly ILogger<RoleUsersController> _logger;
    /// <summary>
    /// RoleUsers
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="logger"></param>
    public RoleUsersController(IMediator mediator,
        ILogger<RoleUsersController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    /// <summary>
    /// Get Role Users By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}", Name = "RoleUsers")]
    [Produces("application/json", "application/xml", Type = typeof(List<UserRoleDto>))]
    public async Task<IActionResult> RoleUsers(Guid id)
    {
        var getUserQuery = new GetRoleUsersQuery
        {
            RoleId = id
        };
        var result = await _mediator.Send(getUserQuery);
        return Ok(result);
    }
    /// <summary>
    /// Update Role Users By Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="updateRoleCommand"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [Produces("application/json", "application/xml", Type = typeof(UserRoleDto))]
    // [ClaimCheck("assign_user_role")]
    public async Task<IActionResult> UpdateRoleUsers(Guid id, UpdateUserRoleCommand updateRoleCommand)
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
}

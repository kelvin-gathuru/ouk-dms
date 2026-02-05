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

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class StorageSettingController : BaseController
{
    private readonly IMediator _mediator;

    public StorageSettingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Creates the storage setting.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <returns></returns>
    [HttpPost]
    // [ClaimCheck("manage_storage_settings")]
    public async Task<IActionResult> CreateStorageSetting(AddStorageSettingCommand command)
    {
        var result = await _mediator.Send(command);
        return GenerateResponse(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStorageSetting(Guid id)
    {
        var query = new GetStorageSettingQuery { Id = id };
        var result = await _mediator.Send(query);
        return Ok(result.Data);
    }

    /// <summary>
    /// Gets the storage setting.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpGet("default")]
    public async Task<IActionResult> GetDefaultStorageSetting()
    {
        var query = new GetDefaultStorageSettingCommand { };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Get all storage settings
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetStorageSettings")]
    [Produces("application/json", "application/xml", Type = typeof(List<StorageSettingDto<object>>))]
    public async Task<IActionResult> GetStorageSettings()
    {
        var getAllStorageSettingQuery = new GetAllStorageSettingQuery
        {
        };
        var result = await _mediator.Send(getAllStorageSettingQuery);
        return Ok(result);
    }

    /// <summary>
    /// Updates the storage setting.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="updateStorageSettingCommand">The update storage setting command.</param>
    /// <returns></returns>
    [HttpPost("{id}")]
    [Produces("application/json", "application/xml", Type = typeof(StorageSettingDto<object>))]
    // [ClaimCheck("manage_storage_settings")]
    public async Task<IActionResult> UpdateStorageSetting(Guid id, UpdateStorageSettingCommand updateStorageSettingCommand)
    {
        var result = await _mediator.Send(updateStorageSettingCommand);
        return GenerateResponse(result);
    }

    // <summary>
    /// Deletes the storage setting.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    // [ClaimCheck("manage_storage_settings")]
    public async Task<IActionResult> DeleteStorageSetting(Guid id)
    {
        var query = new DeleteStorageSettingCommand { Id = id };
        var result = await _mediator.Send(query);
        return GenerateResponse(result);
    }
}

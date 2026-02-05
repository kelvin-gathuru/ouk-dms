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
public class AllowFileExtensionController(IMediator _mediator) : BaseController
{
    /// <summary>
    /// Creates the Allow file extension.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <returns></returns>
    [HttpPost]
    // [ClaimCheck("add_allow_file_extensions")]
    public async Task<IActionResult> CreateAllowFileExtension(AddAllowFileExtensionCommand command)
    {
        var result = await _mediator.Send(command);
        return GenerateResponse(result);
    }

    /// <summary>
    /// Updates the Allow file extension.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="updateStorageSettingCommand">The update storage setting command.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [Produces("application/json", "application/xml", Type = typeof(WorkflowDto))]
    // [ClaimCheck("edit_allow_file_extensions")]
    public async Task<IActionResult> UpdateAllowFileExtension(Guid id, UpdateAllowFileExtensionCommand command)
    {
        var result = await _mediator.Send(command);
        return GenerateResponse(result);
    }

    // <summary>
    /// Deletes the allow file extension.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    // [ClaimCheck("delete_allow_file_extensions")]
    public async Task<IActionResult> DeleteAllowFileExtenstion(Guid id)
    {
        var query = new DeleteAllowFileExtensionCommand { Id = id };
        var result = await _mediator.Send(query);
        return GenerateResponse(result);
    }

    /// <summary>
    /// Gets the document status.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAllowFileExtension(Guid id)
    {
        var query = new GetAllowFileExtensionQuery { Id = id };
        var result = await _mediator.Send(query);
        return Ok(result.Data);
    }

    /// <summary>
    /// Get all document status
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetAllowFileExtension")]
    [Produces("application/json", "application/xml", Type = typeof(List<AllowFileExtensionDto>))]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllowFileExtensions()
    {
        var query = new GetAllAllowFileExtensionQuery { };
        var result = await _mediator.Send(query);
        return Ok(result.Data);
    }
}

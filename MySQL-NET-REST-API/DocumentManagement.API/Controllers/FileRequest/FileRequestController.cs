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
public class FileRequestController(IMediator _mediator) : BaseController
{
    /// <summary>
    /// Creates the file request.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <returns></returns>
    [HttpPost]
    // [ClaimCheck("add_file_request")]
    public async Task<IActionResult> CreateFileRequest([FromBody] AddFileRequestCommand command)
    {
        var result = await _mediator.Send(command);
        return GenerateResponse(result);
    }

    /// <summary>
    /// Updates the file request.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="updateStorageSettingCommand">The update storage setting command.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [Produces("application/json", "application/xml", Type = typeof(WorkflowDto))]
    // [ClaimCheck("edit_file_request")]
    public async Task<IActionResult> UpdateFileRequest(Guid id, UpdateFileRequestCommand command)
    {
        var result = await _mediator.Send(command);
        return GenerateResponse(result);
    }

    // <summary>
    /// Deletes the file request.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    // [ClaimCheck("delete_file_request")]
    public async Task<IActionResult> DeleteFileRequest(Guid id)
    {
        var query = new DeleteFileRequestCommand { Id = id };
        var result = await _mediator.Send(query);
        return GenerateResponse(result);
    }

    /// <summary>
    /// Gets the file request.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFileRequest(Guid id)
    {
        var query = new GetFileRequestQuery { Id = id };
        var result = await _mediator.Send(query);
        return Ok(result.Data);
    }

    /// <summary>
    /// Gets the file request.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpGet("{id}/data")]
    [AllowAnonymous]
    public async Task<IActionResult> GetFileRequestData(Guid id)
    {
        var query = new GetFileRequestDataQuery { Id = id };
        var result = await _mediator.Send(query);
        return GenerateResponse(result);
    }

    /// <summary>
    /// Get all file requests
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetAllFileRequest")]
    [Produces("application/json", "application/xml", Type = typeof(List<FileRequestDto>))]
    public async Task<IActionResult> GetAllFileRequests()
    {
        var query = new GetAllFileRequestQuery { };
        var result = await _mediator.Send(query);
        return GenerateResponse(result);
    }

    /// <summary>
    /// Gets the file request.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpGet("{id}/verify-password")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyPassword(VerifyPasswordQuery command)
    {
        var result = await _mediator.Send(command);
        return Ok(result.Data);
    }

}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.API.Controllers;

/// <summary>
/// Screen
/// </summary>
[Route("api")]
[ApiController]
[Authorize]
public class ScreenController : BaseController
{
    public IMediator _mediator { get; set; }
    /// <summary>
    /// Screen
    /// </summary>
    /// <param name="mediator"></param>
    public ScreenController(IMediator mediator)
    {
        _mediator = mediator;
    }
    /// <summary>
    /// Get Screen By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("Screen/{id}", Name = "GetScreen")]
    [Produces("application/json", "application/xml", Type = typeof(ScreenDto))]
    public async Task<IActionResult> GetScreen(Guid id)
    {
        var getScreenQuery = new GetScreenQuery
        {
            Id = id
        };
        var result = await _mediator.Send(getScreenQuery);
        return GenerateResponse(result);
    }
    /// <summary>
    /// Get All Screens
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///     POST /Todo
    ///     {
    ///        "id": 1,
    ///        "name": "Item1",
    ///        "isComplete": true
    ///     }
    /// </remarks>
    /// <returns>Test</returns>
    /// <response code="200">Returns the newly created item</response>
    [HttpGet("Screen")]
    [Produces("application/json", "application/xml", Type = typeof(List<ScreenDto>))]
    public async Task<IActionResult> GetScreens()
    {
        await Task.Delay(1000);
        var getAllScreenQuery = new GetAllScreenQuery { };
        var result = await _mediator.Send(getAllScreenQuery);
        return Ok(result);
    }
    /// <summary>
    /// Create a Screen
    /// </summary>
    /// <param name="addScreenCommand"></param>
    /// <returns></returns>
    [HttpPost("Screen")]
    [Produces("application/json", "application/xml", Type = typeof(ScreenDto))]
    public async Task<IActionResult> AddScreen(AddScreenCommand addScreenCommand)
    {
        var result = await _mediator.Send(addScreenCommand);
        if (result.StatusCode != 200)
        {
            return StatusCode(result.StatusCode, result);
        }
        return CreatedAtAction("GetScreen", new { id = result.Id }, result);
    }
    /// <summary>
    /// Update Screen By Id
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="updateScreenCommand"></param>
    /// <returns></returns>
    [HttpPut("Screen/{Id}")]
    [Produces("application/json", "application/xml", Type = typeof(ScreenDto))]
    public async Task<IActionResult> UpdateScreen(Guid Id, UpdateScreenCommand updateScreenCommand)
    {
        updateScreenCommand.Id = Id;
        var result = await _mediator.Send(updateScreenCommand);
        return StatusCode(result.StatusCode, result);
    }
    /// <summary>
    /// Delete Screen By Id
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    [HttpDelete("Screen/{Id}")]
    public async Task<IActionResult> DeleteScreen(Guid Id)
    {
        var deleteScreenCommand = new DeleteScreenCommand
        {
            Id = Id
        };
        var result = await _mediator.Send(deleteScreenCommand);
        return StatusCode(result.StatusCode, result);
    }
}

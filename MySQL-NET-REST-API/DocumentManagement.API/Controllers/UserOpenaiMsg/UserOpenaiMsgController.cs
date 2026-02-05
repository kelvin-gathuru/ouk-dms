using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AuthChecker;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Resources;
using DocumentManagement.MediatR.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.API.Controllers.UserOpenaiMsg;
[Route("api/[controller]")]
[ApiController]
public class UserOpenaiMsgController(IMediator _mediator) : BaseController
{

    [HttpPost]
    public async Task<IActionResult> AddUserOpenaiMsg([FromBody] AddUserOpenaiMsgCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result.Success)
        {
            return StatusCode(result.StatusCode, result.Errors);
        }
        return StatusCode(result.StatusCode, result.Data);
    }
    //Get Message by Id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserOpenaiMsgById(Guid id)
    {
        var query = new GetUserOpenaiMsgByIdQuery { Id = id };
        var result = await _mediator.Send(query);
        if (!result.Success)
        {
            return StatusCode(result.StatusCode, result.Errors);
        }
        return StatusCode(result.StatusCode, result.Data);
    }

    /// <summary>
    /// Get all User Open ai msg
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Produces("application/json", "application/xml", Type = typeof(List<UserOpenaiMsgDto>))]
    // [ClaimCheck("view_ai_document_generator")]
    public async Task<IActionResult> GetAllUserOpenaiMsg([FromQuery] UserOpenaiMsgResource userOpenaiMsgResource)
    {
        var query = new GetAllUserOpenaiMsgCommand
        {
            userOpenaiMsgResource = userOpenaiMsgResource
        };
        var result = await _mediator.Send(query);
        var paginationMetadata = new
        {
            totalCount = result.TotalCount,
            pageSize = result.PageSize,
            skip = result.Skip,
            totalPages = result.TotalPages
        };

        Response.Headers.Append("X-Pagination",
            JsonSerializer.Serialize(paginationMetadata));

        return Ok(result);
    }

    // <summary>
    /// Deletes the Open ai Msg.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    // [ClaimCheck("delete_ai_document_generator")]
    public async Task<IActionResult> DeleteUserOpenaiMsg(Guid id)
    {
        var query = new DeleteUserOpenaiMsgCommand { Id = id };
        var result = await _mediator.Send(query);
        return GenerateResponse(result);
    }

    //Get Message by Id
    [HttpGet("{id}/response")]
    public async Task<IActionResult> GetUserOpenaiMsgResponseById(Guid id)
    {
        var query = new GetUserOpenaiMsgResponseByIdCommand { Id = id };
        var result = await _mediator.Send(query);
        if (!result.Success)
        {
            return StatusCode(result.StatusCode, result.Errors);
        }
        return GenerateResponse(result);
    }
}

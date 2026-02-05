using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DocumentManagement.API.Controllers;
[Route("api")]
[ApiController]
[Authorize]
public class ActionController(IMediator _mediator) : BaseController
{
    /// <summary>
    /// Get Action By Id
    /// </summary>
    /// <param name="pageId"></param>
    /// <returns></returns>

    [HttpGet("Action/{pageId}", Name = "GetAction")]
    [Produces("application/json", "application/xml", Type = typeof(List<PageActionDto>))]
    //// [ClaimCheck("action_list")]
    public async Task<IActionResult> GetAction(Guid pageId)
    {
        var getActionQuery = new GetPageActionsByPageQuery { PageId = pageId };
        var result = await _mediator.Send(getActionQuery);
        return Ok(result);
    }
    /// <summary>
    /// Get All Actions
    /// </summary>
    /// <returns></returns>
    [HttpGet("Action")]
    //// [ClaimCheck("action_list", "role_add", "role_edit", "page_action_edit", "user_permission_edit")]
    [Produces("application/json", "application/xml", Type = typeof(List<PageActionDto>))]
    public async Task<IActionResult> GetActions()
    {
        var getAllActionQuery = new GetAllPageActionQuery { };
        var result = await _mediator.Send(getAllActionQuery);
        return Ok(result);
    }
    /// <summary>
    /// Create A Action
    /// </summary>
    /// <param name="addActionCommand"></param>
    /// <returns></returns>
    [HttpPost("Action")]
    //// [ClaimCheck("action_add")]
    [Produces("application/json", "application/xml", Type = typeof(PageActionDto))]
    public async Task<IActionResult> AddAction(AddPageActionCommand addActionCommand)
    {
        var response = await _mediator.Send(addActionCommand);
        return GenerateResponse(response);
    }
    /// <summary>
    /// Update Exist Action By Id
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="updateActionCommand"></param>
    /// <returns></returns>
    [HttpPut("Action/{Id}")]
    //// [ClaimCheck("action_edit")]
    [Produces("application/json", "application/xml", Type = typeof(PageActionDto))]
    public async Task<IActionResult> UpdateAction(Guid Id, UpdatePageActionCommand updateActionCommand)
    {
        updateActionCommand.Id = Id;
        var result = await _mediator.Send(updateActionCommand);
        return GenerateResponse(result);

    }
    /// <summary>
    /// Delete Action By Id
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    [HttpDelete("Action/{Id}")]
    //// [ClaimCheck("action_delete")]
    public async Task<IActionResult> DeleteAction(Guid Id)
    {
        var deleteActionCommand = new DeletePageActionCommand { Id = Id };
        var result = await _mediator.Send(deleteActionCommand);
        return GenerateResponse(result);
    }
}

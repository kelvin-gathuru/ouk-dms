using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthChecker;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.MediatR.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AIPromptTemplateController(IMediator _mediator) : ControllerBase
{
    /// <summary>
    /// Create AI Prompt Template
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost]
    // [ClaimCheck("add_prompt_templates")]
    public async Task<IActionResult> CreateAIPromptTemplate(AddAIPromptTemplateCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result.Success)
        {
            return StatusCode(result.StatusCode, result.Errors);
        }
        return StatusCode(result.StatusCode, result.Data);
    }

    /// <summary>
    /// Update AI Prompt Template
    /// </summary>
    /// <param name="id"></param>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [Produces("application/json", "application/xml", Type = typeof(WorkflowDto))]
    // [ClaimCheck("edit_prompt_templates")]
    public async Task<IActionResult> UpdateAIPromptTemplate(Guid id, UpdateAIPromptTemplateCommand command)
    {
        command.Id = id;
        var result = await _mediator.Send(command);
        if (!result.Success)
        {
            return StatusCode(result.StatusCode, result.Errors);
        }
        return StatusCode(result.StatusCode, result.Data);
    }

    /// <summary>
    /// Delete AI Prompt Template
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    // [ClaimCheck("delete_prompt_templates")]
    public async Task<IActionResult> DeleteAIPromptTemplate(Guid id)
    {
        var query = new DeleteAIPromptTemplateCommand { Id = id };
        var result = await _mediator.Send(query);
        if (!result.Success)
        {
            return StatusCode(result.StatusCode, result.Errors);
        }
        return StatusCode(result.StatusCode, result.Data);
    }

    /// <summary>
    /// Get AI Promt Template by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAIPromptTemplate(Guid id)
    {
        var query = new GetAIPromptTemplateCommand { Id = id };
        var result = await _mediator.Send(query);
        if (!result.Success)
        {
            return StatusCode(result.StatusCode, result.Errors);
        }
        return Ok(result.Data);
    }

    /// <summary>
    /// Get All AI Prompt Template
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetAllAIPromptTemplate")]
    [Produces("application/json", "application/xml", Type = typeof(List<AIPromptTemplate>))]
    // [ClaimCheck("view_prompt_templates")]
    public async Task<IActionResult> GetAllAIPromptTemplate()
    {
        var query = new GetAllAIPromptTemplateCommand { };
        var result = await _mediator.Send(query);
        if (!result.Success)
        {
            return StatusCode(result.StatusCode, result.Errors);
        }
        return Ok(result.Data);
    }
}

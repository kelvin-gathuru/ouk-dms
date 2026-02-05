using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DocumentManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class WorkflowStepController(IMediator _mediator) : BaseController
{
    /// <summary>
    /// Creates the workflows step.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateWorkflowStep(AddWorkflowStepCommand command)
    {
        var result = await _mediator.Send(command);
        return GenerateResponse(result);

    }

    [HttpGet("test")]
    [AllowAnonymous]
    public IActionResult Test()
    {
        return Ok("WorkflowStepController is working");
    }

    /// <summary>
    /// Updates the workflow step.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="UpdateWorkflowStepCommand">The update wrokflow setting command.</param>
    /// <returns></returns>
    [HttpPut]
    [Produces("application/json", "application/xml", Type = typeof(WorkflowStepDto))]
    public async Task<IActionResult> UpdateWorkflowStep(UpdateWorkflowStepCommand command)
    {
        var result = await _mediator.Send(command);
        return GenerateResponse(result);
    }

    // <summary>
    /// Deletes the workflow step.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWorkflowStep(Guid id)
    {
        var query = new DeleteWorkflowStepCommand { Id = id };
        var result = await _mediator.Send(query);
        return GenerateResponse(result);
    }

    /// <summary>
    /// Gets the workflow step.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetWorkflowStep(Guid id)
    {
        var query = new GetWorkflowStepQuery { Id = id };
        var result = await _mediator.Send(query);
        return Ok(result.Data);
    }

    /// <summary>
    /// Get all document status
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetWorkflowSteps ")]
    [Produces("application/json", "application/xml", Type = typeof(List<WorkflowStepDto>))]
    public async Task<IActionResult> GetWorkflowSteps()
    {
        var query = new GetAllWorkflowStepQuery { };
        var result = await _mediator.Send(query);
        return Ok(result.Data);
    }

}

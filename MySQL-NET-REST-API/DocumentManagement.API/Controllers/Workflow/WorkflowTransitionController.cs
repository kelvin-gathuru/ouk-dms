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
public class WorkflowTransitionController(IMediator _mediator) : BaseController
{
    /// <summary>
    /// Creates the workflows transition.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateWorkflowTransition(AddWorkflowTransitionCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result.Success)
        {
            return StatusCode(result.StatusCode, result.Errors);
        }
        return Ok(result.Data);
    }

    /// <summary>
    /// Updates the workflow transition.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="UpdateWorkflowTransitionCommand">The update wrokflow setting command.</param>
    /// <returns></returns>
    [HttpPut]
    [Produces("application/json", "application/xml", Type = typeof(WorkflowStepDto))]
    public async Task<IActionResult> UpdateWorkflowTransition(UpdateWorkflowTransitionCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result.Success)
        {
            return StatusCode(result.StatusCode, result.Errors);
        }
        return Ok(result.Data);
    }

    // <summary>
    /// Deletes the workflow transition.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWorkflowTransition(Guid id)
    {
        var query = new DeleteWorkflowTransitionCommand { Id = id };
        var result = await _mediator.Send(query);
        if (!result.Success)
        {
            return StatusCode(result.StatusCode, result.Errors);
        }
        return Ok(result.Data);
    }

    /// <summary>
    /// Gets the workflow step.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetWorkflowTransition(Guid id)
    {
        var query = new GetWorkflowTransitionQuery { Id = id };
        var result = await _mediator.Send(query);
        return Ok(result.Data);
    }

    /// <summary>
    /// Get all workflow transition
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetWorkflowTransitions ")]
    [Produces("application/json", "application/xml", Type = typeof(List<WorkflowTransitionDto>))]
    public async Task<IActionResult> GetWorkflowTransitions()
    {
        var query = new GetAllWorkflowTransitionQuery { };
        var result = await _mediator.Send(query);
        return Ok(result.Data);
    }

    [HttpPost("nextTransition")]
    [Produces("application/json", "application/xml", Type = typeof(PerformWorkflowTransitionToNextTransitionCommand))]
    public async Task<IActionResult> PerformNextTransition(PerformWorkflowTransitionToNextTransitionCommand command)
    {
        var result = await _mediator.Send(command);
        return GenerateResponse(result);

    }
    [HttpPost("nextTransitionWithDocumentAndSignature")]
    [Produces("application/json", "application/xml", Type = typeof(PerformWorkflowTransitionWithSignatureAndDocumentCommand))]
    public async Task<IActionResult> PerformWorkflowTransitionWithSignatureAndDocumentCommand(PerformWorkflowTransitionWithSignatureAndDocumentCommand command)
    {
        var result = await _mediator.Send(command);
        return GenerateResponse(result);

    }

}

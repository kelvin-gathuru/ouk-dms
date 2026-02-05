using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Resources;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class WorkflowInstanceController(IMediator _mediator) : BaseController
{
    /// <summary>
    /// Creates the workflow Instance.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateWorkflowInstance(AddWorkflowInstanceCommand command)
    {
        var result = await _mediator.Send(command);
        return GenerateResponse(result);
    }

    /// <summary>
    /// Updates the workflow Instance.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="updateWorkflowInstanceCommand">The update workflow Instance command.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [Produces("application/json", "application/xml", Type = typeof(WorkflowInstanceDto))]
    public async Task<IActionResult> UpdateWorkflowInstance(Guid id, UpdateWorkflowInstanceCommand command)
    {
        var result = await _mediator.Send(command);
        return GenerateResponse(result);
    }

    // <summary>
    /// Deletes the workflow Instance.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWorkflowInstance(Guid id)
    {
        var query = new DeleteWorkflowInstanceCommand { Id = id };
        var result = await _mediator.Send(query);
        return GenerateResponse(result);
    }

    /// <summary>
    /// Gets the workflow Instance.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetWorkflowInstance(Guid id)
    {
        var query = new GetWorkflowInstanceQuery { Id = id };
        var result = await _mediator.Send(query);
        return Ok(result.Data);
    }

    /// <summary>
    /// Get all document status
    /// </summary>
    /// <returns></returns>
    [HttpGet("CurrentWorkflowInstances", Name = "GetCurrentWorkflowInstances ")]
    [Produces("application/json", "application/xml", Type = typeof(List<WorkflowInstanceDto>))]
    public async Task<IActionResult> GetCurrentWorkflowInstances()
    {
        var query = new GetCurrentWorkflowsInstanceQuery { };
        var result = await _mediator.Send(query);
        return Ok(result.Data);
    }

    /// <summary>
    /// Get all document status
    /// </summary>
    /// <returns></returns>
    [HttpGet("AllWorkflowInstances", Name = "GetAllWorkflowInstances")]
    [Produces("application/json", "application/xml", Type = typeof(List<WorkflowInstanceDto>))]
    public async Task<IActionResult> GetAllWorkflowInstances([FromQuery] AllWorkflowInstanceResource allWorkflowInstanceResource)
    {
        var query = new GetAllWorkflowsInstanceQuery
        {
            allWorkflowInstanceResource = allWorkflowInstanceResource
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

    [HttpGet("{id}/visualWorkflowInstance")]
    public async Task<IActionResult> GetVisualWorkflowInstance(Guid id)
    {
        var query = new GetVisualWorkflowInstanceCommand { WorkflowInstanceId = id };
        var result = await _mediator.Send(query);
        return Ok(result.Data);
    }

    [HttpGet("documents")]
    public async Task<IActionResult> GetAllWorkflowInstaceDocuments()
    {
        var query = new GetAllWorkflowInstanceDocumentsCommand { };
        var result = await _mediator.Send(query);
        return Ok(result);
    }


    [HttpGet("workflows")]
    public async Task<IActionResult> GetAllWorkflowInstaceWorkflow()
    {
        var query = new GetAllWorkflowInstanceWorkflowsCommand { };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Cancel the workflow Instance.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> CancelWorkflowInstance(Guid id)
    {
        var query = new CancelWorkflowInstanceQuery { Id = id };
        var result = await _mediator.Send(query);
        return Ok(result.Data);
    }

    /// <summary>
    /// Gets the workflow Instance By workflowId.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpGet("{id}/byWorkflowId")]
    public async Task<IActionResult> GetWorkflowInstanceByWorkflowId(Guid id)
    {
        var query = new GetWorkflowInstanceByWorkflowIdQuery { WorkflowId = id };
        var result = await _mediator.Send(query);
        return Ok(result.Data);
    }

}

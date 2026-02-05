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
public class WorkflowController(IMediator _mediator) : BaseController
{
    /// <summary>
    /// Creates the Document Status.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <returns></returns>
    [HttpPost]
    // [ClaimCheck("add_workflow_settings")]
    public async Task<IActionResult> CreateWorkflow(AddWorkflowCommand command)
    {
        var result = await _mediator.Send(command);
        return GenerateResponse(result);
    }

    /// <summary>
    /// Updates the document status.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="updateStorageSettingCommand">The update storage setting command.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [Produces("application/json", "application/xml", Type = typeof(WorkflowDto))]
    // [ClaimCheck("edit_workflow_settings")]
    public async Task<IActionResult> UpdateWorkflow(Guid id, UpdateWorkflowCommand command)
    {
        var result = await _mediator.Send(command);
        return GenerateResponse(result);
    }

    // <summary>
    /// Deletes the document status.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    // [ClaimCheck("delete_workflow_settings")]
    public async Task<IActionResult> DeleteWorkflow(Guid id)
    {
        var query = new DeleteWorkflowCommand { Id = id };
        var result = await _mediator.Send(query);
        return GenerateResponse(result);
    }

    [HttpGet("ReqDocument/Workflows")]
    public async Task<IActionResult> GetReqDocumentWorkflows()
    {
        var query = new GetReqDocumentWorkflowsQuery { };
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    [HttpPost("ReqDocument/Workflow/create")]
    public async Task<IActionResult> CreateReqDocumentWorkflow(CreateReqDocumentWorkflowQuery createReqDocumentWorkflowQuery)
    {
        var result = await _mediator.Send(createReqDocumentWorkflowQuery);
        return Ok(result.Data);
    }

    /// <summary>
    /// Gets the document status.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetWorkflow(Guid id)
    {
        var query = new GetWorkflowQuery { Id = id };
        var result = await _mediator.Send(query);
        return Ok(result.Data);
    }

    /// <summary>
    /// Get all document status
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetWorkflows")]
    [Produces("application/json", "application/xml", Type = typeof(List<WorkflowDto>))]
    public async Task<IActionResult> GetWorkflows()
    {
        var query = new GetAllWorkflowQuery { };
        var result = await _mediator.Send(query);
        return Ok(result.Data);
    }
    [HttpGet("NotStarted/document/{documentId}", Name = "GetWorkflowsNotStarted")]
    [Produces("application/json", "application/xml", Type = typeof(List<WorkflowDto>))]
    public async Task<IActionResult> GetWorkflowsNotStarted(Guid documentId)
    {
        var query = new GetAllWorkflowNotStartedQuery { DocumentId = documentId };
        var result = await _mediator.Send(query);
        return Ok(result.Data);
    }

    [HttpGet("{id}/visualWorkflow")]
    public async Task<IActionResult> GetVisualWorkflowInstance(Guid id)
    {
        var query = new GetVisualWorkflowCommand { WorkflowId = id };
        var result = await _mediator.Send(query);
        return Ok(result.Data);
    }


}

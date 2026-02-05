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
public class DocumentStatusController(IMediator _mediator) : BaseController
{
    /// <summary>
    /// Creates the Document Status.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateDocumentStatus(AddDocumentStatusCommand command)
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
    [HttpPost("{id}")]
    [Produces("application/json", "application/xml", Type = typeof(StorageSettingDto<object>))]
    // [ClaimCheck("manage_document_status")]
    public async Task<IActionResult> UpdateDocumentStatus(Guid id, UpdateDocumentStatusCommand updateDocumentStatusCommand)
    {
        var result = await _mediator.Send(updateDocumentStatusCommand);
        return GenerateResponse(result);
    }

    // <summary>
    /// Deletes the document status.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    // [ClaimCheck("manage_document_status")]
    public async Task<IActionResult> DeleteDocumentStatus(Guid id)
    {
        var query = new DeleteDocumentStatusCommand { Id = id };
        var result = await _mediator.Send(query);
        return GenerateResponse(result);
    }

    /// <summary>
    /// Gets the document status.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDocumentStatus(Guid id)
    {
        var query = new GetDocumentStatusQuery { Id = id };
        var result = await _mediator.Send(query);
        return Ok(result.Data);
    }

    /// <summary>
    /// Get all document status
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetDocumentStatuss")]
    [Produces("application/json", "application/xml", Type = typeof(List<DocumentStatusDto>))]
    public async Task<IActionResult> GetDocumentStatuss()
    {
        var getAllDocumentStatusQuery = new GetAllDocumentStatusQuery
        {
        };
        var result = await _mediator.Send(getAllDocumentStatusQuery);
        return Ok(result.Data);
    }

}

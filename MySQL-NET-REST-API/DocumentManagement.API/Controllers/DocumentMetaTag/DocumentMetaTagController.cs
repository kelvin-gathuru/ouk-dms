using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthChecker;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DocumentMetaTagController(IMediator _mediator) : BaseController
{
    /// <summary>
    /// Creates the DocumentMetaTag.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <returns></returns>
    [HttpPost]
    // [ClaimCheck("add_document_meta_tags")]
    public async Task<IActionResult> AddDocumentMetaTag(AddDocumentMetaTagCommand command)
    {
        var result = await _mediator.Send(command);
        return GenerateResponse(result);
    }

    /// <summary>
    /// Updates the DocumentMetaTag.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="updateClientCommand">The update document meta tag command.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [Produces("application/json", "application/xml", Type = typeof(WorkflowDto))]
    // [ClaimCheck("edit_document_meta_tags")]
    public async Task<IActionResult> UpdateDocumentMetaTag(Guid id, UpdateDocumentMetaTagCommand command)
    {
        var result = await _mediator.Send(command);
        return GenerateResponse(result);
    }

    // <summary>
    /// Deletes the DocumentMetaTag.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    // [ClaimCheck("delete_document_meta_tags")]
    public async Task<IActionResult> DeleteDocumentMetaTag(Guid id)
    {
        var query = new DeleteDocumentMetaTagCommand { Id = id };
        var result = await _mediator.Send(query);
        return GenerateResponse(result);
    }

    /// <summary>
    /// Gets the DocumentMetaTag.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDocumentMetaTag(Guid id)
    {
        var query = new GetDocumentMetaTagCommand { Id = id };
        var result = await _mediator.Send(query);
        return Ok(result.Data);
    }

    /// <summary>
    /// Get All DocumentMetaTags.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Produces("application/json", "application/xml", Type = typeof(List<DocumentMetaTagDto>))]
    public async Task<IActionResult> GetDocumentMetaTags()
    {
        var query = new GetAllDocumentMetaTagCommand { };
        var result = await _mediator.Send(query);
        return Ok(result.Data);
    }
}

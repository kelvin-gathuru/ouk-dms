using System.Text.Json;
using System.Threading.Tasks;
using AuthChecker;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Resources;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.API.Controllers;

/// <summary>
/// DocumentAuditTrail
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DocumentAuditTrailController : ControllerBase
{
    public IMediator _mediator { get; set; }
    /// <summary>
    /// DocumentAuditTrail
    /// </summary>
    /// <param name="mediator"></param>
    public DocumentAuditTrailController(
        IMediator mediator
        )
    {
        _mediator = mediator;
    }
    /// <summary>
    /// Get All Document Audit Trail detail
    /// </summary>
    /// <param name="documentResource"></param>
    /// <returns></returns>
    [HttpGet]
    [Produces("application/json", "application/xml", Type = typeof(DocumentAuditTrailList))]
    // [ClaimCheck("view_document_audit_trail")]
    public async Task<IActionResult> GetDocumentAuditTrails([FromQuery] DocumentResource documentResource)
    {
        var getAllDocumentAuditTrailQuery = new GetAllDocumentAuditTrailQuery
        {
            DocumentResource = documentResource
        };
        var result = await _mediator.Send(getAllDocumentAuditTrailQuery);

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
    /// <summary>
    /// Add Document Audit Trail
    /// </summary>
    /// <param name="addDocumentAuditTrailCommand"></param>
    /// <returns></returns>
    [HttpPost]
    [Produces("application/json", "application/xml", Type = typeof(DocumentAuditTrailDto))]
    public async Task<IActionResult> AddDocumentAuditTrail(AddDocumentAuditTrailCommand addDocumentAuditTrailCommand)
    {
        var result = await _mediator.Send(addDocumentAuditTrailCommand);
        if (result.StatusCode != 200)
        {
            return StatusCode(result.StatusCode, result);
        }
        return StatusCode(StatusCodes.Status201Created, result);
    }
}

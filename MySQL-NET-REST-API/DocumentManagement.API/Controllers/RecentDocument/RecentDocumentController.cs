using DocumentManagement.Data.Resources;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DocumentManagement.API.Controllers.RecentDocument;

/// <summary>
/// RecentDocument
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RecentDocumentController : ControllerBase
{
    public IMediator _mediator { get; set; }
    /// <summary>
    /// DocumentAuditTrail
    /// </summary>
    /// <param name="mediator"></param>
    public RecentDocumentController(
        IMediator mediator
        )
    {
        _mediator = mediator;
    }
    /// <summary>
    /// Get All Recent Documents
    /// </summary>
    /// <param name="documentResource"></param>
    /// <returns></returns>
    [HttpGet]
    [Produces("application/json", "application/xml", Type = typeof(RecentDocumentList))]
    //// [ClaimCheck("document_audit_trail_view_document_audit_trail")]
    public async Task<IActionResult> GetRecentDocuments([FromQuery] DocumentResource documentResource)
    {
        var getAllRecentDocumentQuery = new GetAllRecentDocumentQuery
        {
            DocumentResource = documentResource
        };
        var result = await _mediator.Send(getAllRecentDocumentQuery);

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
}

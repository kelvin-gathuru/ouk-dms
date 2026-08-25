using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DocumentManagement.Api.Helpers;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Resources;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.API.Controllers;

/// <summary>
/// Read-only integration API for the external intranet application.
///
/// Every request must carry the intranet API key in the X-Api-Key header.
/// Only documents explicitly flagged as "accessible on intranet" are ever
/// exposed through these endpoints; anything else returns 404.
///
/// Base URL: https://srv2.ouk.ac.ke/api/intranet
/// </summary>
[Route("api/intranet")]
[ApiController]
[Authorize(AuthenticationSchemes = IntranetApiKeyAuthenticationHandler.SchemeName)]
public class IntranetController : BaseController
{
    public IMediator _mediator { get; set; }
    private readonly IDocumentRepository _documentRepository;

    public IntranetController(
        IMediator mediator,
        IDocumentRepository documentRepository)
    {
        _mediator = mediator;
        _documentRepository = documentRepository;
    }

    /// <summary>
    /// List documents available to the intranet.
    /// </summary>
    /// <remarks>
    /// Returns a paged list of documents that are marked as intranet accessible
    /// and not archived. Use the query string to filter, sort and page.
    ///
    /// Pagination details are returned in the X-Pagination response header as JSON:
    /// { "totalCount": 2, "pageSize": 10, "skip": 0, "totalPages": 1 }
    ///
    /// Sample request:
    ///
    ///     GET /api/intranet/documents?PageSize=10&amp;Skip=0&amp;OrderBy=Name
    ///
    /// Headers:
    ///     X-Api-Key: &lt;your-intranet-api-key&gt;
    /// </remarks>
    /// <param name="documentResource">Filter, sort and paging options (all optional).</param>
    /// <returns>A paged collection of intranet accessible documents.</returns>
    /// <response code="200">Paged document list returned. Pagination is in the X-Pagination header.</response>
    /// <response code="401">Missing or invalid X-Api-Key header.</response>
    [HttpGet("documents")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DocumentList))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetIntranetDocuments([FromQuery] DocumentResource documentResource)
    {
        documentResource.IsArchive = false;
        documentResource.IsIntranetAccessible = true;
        var getAllDocumentQuery = new GetAllDocumentQuery
        {
            DocumentResource = documentResource
        };
        var result = await _mediator.Send(getAllDocumentQuery);

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
    /// Get a single document by id.
    /// </summary>
    /// <remarks>
    /// Returns the full detail of one document. The document is only returned if
    /// it is flagged as intranet accessible; otherwise 404 is returned.
    ///
    /// Sample request:
    ///
    ///     GET /api/intranet/documents/d249eec1-eef0-4cae-b9e7-9448dc620c8e
    /// </remarks>
    /// <param name="id">The document id (GUID).</param>
    /// <returns>The requested document, including category, status, metadata and workflow details.</returns>
    /// <response code="200">Document detail returned.</response>
    /// <response code="401">Missing or invalid X-Api-Key header.</response>
    /// <response code="404">Document not found or not intranet accessible.</response>
    [HttpGet("documents/{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DocumentDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetIntranetDocument(Guid id)
    {
        if (!await IsIntranetAccessible(id))
        {
            return NotFound("Document is not found or not accessible to the intranet.");
        }

        var getDocumentQuery = new GetDocumentQuery
        {
            Id = id
        };
        var response = await _mediator.Send(getDocumentQuery);
        if (!response.Success)
        {
            return StatusCode(response.StatusCode, response.Errors);
        }
        return Ok(response.Data);
    }

    /// <summary>
    /// Get the version history of a document.
    /// </summary>
    /// <remarks>
    /// Returns every version of the document, oldest first. The current version is
    /// flagged with isCurrentVersion = true.
    /// </remarks>
    /// <param name="id">The document id (GUID).</param>
    /// <returns>A list of document versions.</returns>
    /// <response code="200">Version list returned.</response>
    /// <response code="401">Missing or invalid X-Api-Key header.</response>
    /// <response code="404">Document not found or not intranet accessible.</response>
    [HttpGet("documents/{id}/versions")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DocumentVersionDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetIntranetDocumentVersions(Guid id)
    {
        if (!await IsIntranetAccessible(id))
        {
            return NotFound("Document is not found or not accessible to the intranet.");
        }

        var command = new GetDocumentVersionCommand
        {
            Id = id
        };
        var versions = await _mediator.Send(command);
        return Ok(versions);
    }

    /// <summary>
    /// Download the current version of a document.
    /// </summary>
    /// <remarks>
    /// Streams the current file version as a binary download. The file name and
    /// content type are provided by the Content-Disposition / Content-Type
    /// response headers respectively.
    /// </remarks>
    /// <param name="id">The document id (GUID).</param>
    /// <returns>The current file as a binary stream.</returns>
    /// <response code="200">File stream returned.</response>
    /// <response code="401">Missing or invalid X-Api-Key header.</response>
    /// <response code="404">Document not found or not intranet accessible.</response>
    [HttpGet("documents/{id}/download")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DownloadIntranetDocument(Guid id)
    {
        if (!await IsIntranetAccessible(id))
        {
            return NotFound("Document is not found or not accessible to the intranet.");
        }

        var command = new DownloadDocumentCommand
        {
            Id = id,
            IsVersion = false
        };
        var response = await _mediator.Send(command);
        if (!response.Success)
        {
            return GenerateResponse(response);
        }

        var download = response.Data;
        return File(download.Data, download.ContentType, download.FileName);
    }

    /// <summary>
    /// Download a specific version of a document.
    /// </summary>
    /// <remarks>
    /// Streams the requested file version as a binary download. Use the document
    /// versions endpoint first to obtain the version id.
    /// </remarks>
    /// <param name="id">The document id (GUID).</param>
    /// <param name="versionId">The document version id (GUID).</param>
    /// <returns>The requested file version as a binary stream.</returns>
    /// <response code="200">File stream returned.</response>
    /// <response code="401">Missing or invalid X-Api-Key header.</response>
    /// <response code="404">Document or version not found / not intranet accessible.</response>
    [HttpGet("documents/{id}/versions/{versionId}/download")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DownloadIntranetDocumentVersion(Guid id, Guid versionId)
    {
        if (!await IsIntranetAccessible(id))
        {
            return NotFound("Document is not found or not accessible to the intranet.");
        }

        var command = new DownloadDocumentCommand
        {
            Id = versionId,
            IsVersion = true
        };
        var response = await _mediator.Send(command);
        if (!response.Success)
        {
            return GenerateResponse(response);
        }

        var download = response.Data;
        return File(download.Data, download.ContentType, download.FileName);
    }

    /// <summary>
    /// Get the metadata of a document.
    /// </summary>
    /// <remarks>
    /// Returns the custom metadata tags attached to the document.
    /// </remarks>
    /// <param name="id">The document id (GUID).</param>
    /// <returns>A list of document metadata tags.</returns>
    /// <response code="200">Metadata list returned.</response>
    /// <response code="401">Missing or invalid X-Api-Key header.</response>
    /// <response code="404">Document not found or not intranet accessible.</response>
    [HttpGet("documents/{id}/metadata")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DocumentMetaDataDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetIntranetDocumentMetadata(Guid id)
    {
        if (!await IsIntranetAccessible(id))
        {
            return NotFound("Document is not found or not accessible to the intranet.");
        }

        var command = new GetDocumentMetaDataByIdQuery
        {
            DocumentId = id
        };
        var metadata = await _mediator.Send(command);
        return Ok(metadata);
    }

    /// <summary>
    /// Get the comments of a document.
    /// </summary>
    /// <remarks>
    /// Returns the comments attached to the document, oldest first.
    /// </remarks>
    /// <param name="id">The document id (GUID).</param>
    /// <returns>A list of document comments.</returns>
    /// <response code="200">Comment list returned.</response>
    /// <response code="401">Missing or invalid X-Api-Key header.</response>
    /// <response code="404">Document not found or not intranet accessible.</response>
    [HttpGet("documents/{id}/comments")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DocumentCommentDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetIntranetDocumentComments(Guid id)
    {
        if (!await IsIntranetAccessible(id))
        {
            return NotFound("Document is not found or not accessible to the intranet.");
        }

        var query = new GetDocumentCommentByIdQuery
        {
            DocumentId = id
        };
        var comments = await _mediator.Send(query);
        return Ok(comments);
    }

    /// <summary>
    /// Get the audit trail of a document.
    /// </summary>
    /// <remarks>
    /// Returns the full audit trail (who created, modified, viewed and downloaded
    /// the document and when) for the requested document.
    /// </remarks>
    /// <param name="id">The document id (GUID).</param>
    /// <returns>A paged list of audit trail entries.</returns>
    /// <response code="200">Audit trail list returned.</response>
    /// <response code="401">Missing or invalid X-Api-Key header.</response>
    /// <response code="404">Document not found or not intranet accessible.</response>
    [HttpGet("documents/{id}/audittrail")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DocumentAuditTrailList))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetIntranetDocumentAuditTrail(Guid id)
    {
        if (!await IsIntranetAccessible(id))
        {
            return NotFound("Document is not found or not accessible to the intranet.");
        }

        var query = new GetAllDocumentAuditTrailQuery
        {
            DocumentResource = new DocumentResource
            {
                Id = id.ToString()
            }
        };
        var auditTrails = await _mediator.Send(query);
        return Ok(auditTrails);
    }

    private async Task<bool> IsIntranetAccessible(Guid documentId)
    {
        return await _documentRepository.All
            .AnyAsync(c => c.Id == documentId && c.IsIntranetAccessible);
    }
}

using System;
using System.Text;
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
public class FileRequestDocumentController(IMediator _mediator) : BaseController
{
    /// <summary>
    /// Creates the file request document.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateFileRequestDocument([FromForm] AddFileRequestDocumentCommand command)
    {
        var result = await _mediator.Send(command);
        return GenerateResponse(result);
    }

    /// <summary>
    /// Updates the file request document.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="updateStorageSettingCommand">The update storage setting command.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [Produces("application/json", "application/xml", Type = typeof(WorkflowDto))]
    public async Task<IActionResult> UpdateFileRequest(Guid id, UpdateFileRequestDocumentCommand command)
    {
        var result = await _mediator.Send(command);
        return GenerateResponse(result);
    }

    /// <summary>
    /// Gets the file request.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFileRequestDocuments(Guid id)
    {
        var query = new GetFileRequestDocumentsQuery { Id = id };
        var result = await _mediator.Send(query);
        return Ok(result.Data);
    }

    [HttpGet("{id}/download")]
    public async Task<IActionResult> DownloadDocument(Guid id)
    {
        var command = new DownloadFileRequestDocumentCommand
        {
            Id = id
        };

        var response = await _mediator.Send(command);
        if (!response.Success)
        {
            return StatusCode(response.StatusCode, new
            {
                error = new
                {
                    message = response.Errors
                }
            });
        }

        var downloadDocument = response.Data;
        return File(downloadDocument.Data, downloadDocument.ContentType, downloadDocument.FileName);
    }

    [HttpGet("{id}/token")]
    public async Task<IActionResult> GetDocumentToken(Guid id)
    {
        var command = new GetFileRequestDocumentQuery { Id = id };
        var infoResult = await _mediator.Send(command);

        if (!infoResult.Success || infoResult.Data == null)
        {
            return StatusCode(infoResult.StatusCode, new
            {
                error = new
                {
                    message = infoResult.Errors
                }
            });
        }

        var getDocumentTokenQuery = new GetFileRequestDocumentTokenQuery
        {
            Id = infoResult.Data.Id,
        };

        var result = await _mediator.Send(getDocumentTokenQuery);
        return Ok(new { result });
    }

    [HttpGet("{id}/readtext")]
    public async Task<IActionResult> ReadText(Guid id)
    {
        var command = new GetFileRequestDocumentQuery { Id = id };
        var result = await _mediator.Send(command);

        if (!result.Success || result.Data == null)
        {
            return StatusCode(result.StatusCode, new
            {
                message = result.Errors
            });
        }

        var link = result.Data;
        var commnad = new DownloadFileRequestDocumentCommand
        {
            Id = link.Id,
        };

        var downloadDocumentResponse = await _mediator.Send(commnad);
        if (!downloadDocumentResponse.Success)
        {
            return StatusCode(downloadDocumentResponse.StatusCode, new
            {
                message = downloadDocumentResponse.Errors
            });
        }

        var downloadDocument = downloadDocumentResponse.Data;
        string utfString = Encoding.UTF8.GetString(downloadDocument.Data, 0, downloadDocument.Data.Length);
        return Ok(new { result = new string[] { utfString } });
    }

    /// <summary>
    /// Create a document.
    /// </summary>
    /// <param name="addDocumentCommand"></param>
    /// <returns></returns>
    [HttpPost("Document")]
    [DisableRequestSizeLimit]
    [Produces("application/json", "application/xml", Type = typeof(DocumentDto))]
    // [ClaimCheck("approved_file_request_documents")]
    public async Task<IActionResult> AddDocument([FromForm] ApproveDocumentCommand apporveDocumentCommand)
    {
        var result = await _mediator.Send(apporveDocumentCommand);
        if (!result.Success)
        {
            return StatusCode(result.StatusCode, result.Errors);
        }
        return GenerateResponse(result);
    }
}

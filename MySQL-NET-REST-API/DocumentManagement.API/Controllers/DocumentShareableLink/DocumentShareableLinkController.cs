using System;
using System.Text;
using System.Threading.Tasks;
using AuthChecker;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DocumentShareableLinkController : BaseController
{
    private readonly IMediator _mediator;

    public DocumentShareableLinkController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Creates the shareable link.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <returns></returns>
    [HttpPost]
    // [ClaimCheck("all_create_shareable_link", "assigned_create_shareable_link")]
    public async Task<IActionResult> CreateShareableLink(CreateDocumentShareableLinkCommand command)
    {
        var result = await _mediator.Send(command);
        return GenerateResponse(result);
    }

    /// <summary>
    /// Creates the shareable link.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    // [ClaimCheck("all_create_shareable_link", "assigned_create_shareable_link")]
    public async Task<IActionResult> GetDocumentShareableLink(Guid id)
    {
        var query = new GetDocumentShareableLinkQuery { Id = id };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Deletes the document shareable link.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    // [ClaimCheck("all_create_shareable_link", "assigned_create_shareable_link")]
    public async Task<IActionResult> DeleteDocumentShareableLink(Guid id)
    {
        var query = new DeleteDocumentShareableLinkCommand { Id = id };
        var result = await _mediator.Send(query);
        return GenerateResponse(result);
    }

    [HttpGet("{code}/download")]
    [AllowAnonymous]
    public async Task<IActionResult> DownloadDocument(string code, string password)
    {
        var command = new DownloadSharedDocumentCommand
        {
            Code = code,
            Password = password
        };

        var response = await _mediator.Send(command);
        if (!response.Success)
        {
            return StatusCode(response.StatusCode, response.Errors);
        }

        var downloadDocument = response.Data;
        return File(downloadDocument.Data, downloadDocument.ContentType, downloadDocument.FileName);
    }

    [HttpGet("{code}/token")]
    [AllowAnonymous]
    public async Task<IActionResult> GetDocumentToken(string code)
    {
        var command = new GetLinkInfoByCodeQuery { Code = code };
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

        var getDocumentTokenQuery = new GetDocumentTokenQuery
        {
            Id = infoResult.Data.DocumentId,
        };

        var result = await _mediator.Send(getDocumentTokenQuery);
        return Ok(new { result });
    }

    [HttpGet("{code}/readtext")]
    [AllowAnonymous]
    public async Task<IActionResult> ReadText(string code, string password)
    {
        var command = new GetLinkInfoByCodeQuery { Code = code };
        var result = await _mediator.Send(command);

        if (!result.Success || result.Data == null)
        {
            return StatusCode(result.StatusCode, new
            {
                message = result.Errors
            });
        }

        var link = result.Data;
        if (link.IsLinkExpired)
        {
            return StatusCode(404, new
            {
                message = new string[] { "Link is expired" }
            });
        }

        if (link.HasPassword)
        {
            if (link.Password != password)
            {
                return StatusCode(404, new
                {
                    message = new string[] { "Invalid password" }
                });
            }
        }

        var commnad = new DownloadDocumentCommand
        {
            Id = link.DocumentId,
            IsVersion = false
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

    [HttpGet("{code}/info")]
    [AllowAnonymous]
    public async Task<IActionResult> GetLinkInfo(string code)
    {
        var command = new GetLinkInfoByCodeQuery { Code = code };
        var result = await _mediator.Send(command);
        if (!result.Success || result.Data == null)
        {
            return StatusCode(result.StatusCode, new
            {
                error = new
                {
                    message = result.Errors
                }
            });
        }

        return Ok(new
        {
            result.Data.HasPassword,
            result.Data.IsLinkExpired,
        });
    }

    [HttpGet("{code}/document")]
    [AllowAnonymous]
    public async Task<IActionResult> GetDocument(string code)
    {
        var command = new GetLinkInfoByCodeQuery { Code = code };
        var result = await _mediator.Send(command);
        if (!result.Success || result.Data == null)
        {
            return StatusCode(result.StatusCode, new
            {
                error = new
                {
                    message = result.Errors
                }
            });
        }

        var link = result.Data;
        return Ok(new
        {
            Name = link.DocumentName,
            link.IsAllowDownload,
            link.Url,
            DocumentNumber = link.DocumentNumber,
        });
    }
}

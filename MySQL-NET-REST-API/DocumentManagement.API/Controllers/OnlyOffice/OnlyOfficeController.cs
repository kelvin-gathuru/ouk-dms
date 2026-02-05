using DocumentManagement.API.Helpers;
using DocumentManagement.MediatR.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DocumentManagement.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class OnlyOfficeController(IMediator mediator, OnlyOfficeTokenService _tokenService) : BaseController
{
    /// <summary>
    /// Get OnlyOffice configuration for the editor.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("config/{id}")]
    public async Task<IActionResult> GetOnlyOfficeConfig(Guid id)
    {
        var getDocumentDetailById = new GetDocumentDetailById { Id = id };
        var response = await mediator.Send(getDocumentDetailById);
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        if (response == null)
        {
            return StatusCode(500, "Server Error");
        }
        var docConfig = new
        {
            documentType = GetTypeByExtension(response.Extension.Replace(".", "")),
            document = new
            {
                fileType = response.Extension.Replace(".", ""),
                key = id.ToString(),
                title = response.Name,
                url = $"{baseUrl}/api/OnlyOffice/files/{id}"  // replace with your actual doc URL
            },
            editorConfig = new
            {
                mode = "edit",
                callbackUrl = $"{baseUrl}/api/OnlyOffice/save"
            }
        };
        var token = _tokenService.GenerateToken(docConfig);

        return Ok(new { docConfig, token });
    }
    /// <summary>
    /// Pass FIle to ONLYOFFICE editor.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("files/{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetFile(Guid id)
    {
        var commnad = new DownloadDocumentChunkCommand
        {
            Id = id,
            DocumentVersionId = Guid.Empty
        };

        var downloadDocumentResponse = await mediator.Send(commnad);

        if (!downloadDocumentResponse.Success)
        {
            return GenerateResponse(downloadDocumentResponse);
        }
        var contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        var downloadDocument = downloadDocumentResponse.Data;
        return File(downloadDocument.Data, contentType, downloadDocument.FileName);
    }
    /// <summary>
    /// Save Editor changes callback from ONLYOFFICE editor.
    /// </summary>
    /// <param name="payload"></param>
    /// <returns></returns>
    [HttpPost("save")]
    [AllowAnonymous]
    public async Task<IActionResult> SaveEditedDocument([FromBody] JsonElement payload)
    {
        try
        {
            var status = payload.GetProperty("status").GetInt32();
            var fileUrl = payload.GetProperty("url").GetString();
            var fileKey = payload.GetProperty("key").GetString(); // Optional: to track changes

            var uploadNewDocumentVersionOnlyCommand = new UploadNewDocumentVersionOnlyCommand
            {
                DocumentId = Guid.Parse(fileKey), // Assuming fileKey is the document ID
                File = await new HttpClient().GetByteArrayAsync(fileUrl),
            };

            var result = await mediator.Send(uploadNewDocumentVersionOnlyCommand);
            if (!result.Success)
            {
                return Ok(new { error = 1 });
            }

            return Ok(new { error = 0 });

        }
        catch (Exception)
        {
            // Log the exception
            return Ok(new { error = 1 }); // inform ONLYOFFICE there was a failure
        }
    }
    private string GetTypeByExtension(string extension)
    {
        return extension switch
        {
            "doc" or "docx" => "word",
            "xls" or "xlsx" => "spreadsheet",
            "ppt" or "pptx" => "presentation",
            _ => "text"
        };
    }
}

using DocumentManagement.MediatR.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DocumentManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DocumentSignatureController(IMediator _mediator) : BaseController
{
    /// <summary>
    /// Creates the document signature.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateDocumentSignature(AddDocumentSignatureCommand command)
    {
        var result = await _mediator.Send(command);
        return GenerateResponse(result);
    }
    /// <summary>
    /// Added Signature in Document Signature Postion
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("position")]
    public async Task<IActionResult> CreateDocumentSignatureWithPosition(AddDocumentSignatureWithPositionCommand command)
    {
        var result = await _mediator.Send(command);
        return GenerateResponse(result);
    }

    /// <summary>
    /// Gets the document signatures.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAllDocumentSignatures(Guid id)
    {
        var query = new GetAllDocumentSignatureQuery { DocumentId = id };
        var result = await _mediator.Send(query);
        return Ok(result.Data);
    }


}

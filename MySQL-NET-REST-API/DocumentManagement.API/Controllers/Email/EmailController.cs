using System.Threading.Tasks;
using AuthChecker;
using DocumentManagement.MediatR.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.API.Controllers.Email;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EmailController : BaseController
{
    readonly IMediator _mediator;
    public EmailController(IMediator mediator)
    {
        _mediator = mediator;
    }
    /// <summary>
    /// Send mail.
    /// </summary>
    /// <param name="sendEmailCommand"></param>
    /// <returns></returns>
    [HttpPost(Name = "SendEmail")]
    [Produces("application/json", "application/xml", Type = typeof(void))]
    // [ClaimCheck("all_send_email")]
    public async Task<IActionResult> SendEmail(SendEmailCommand sendEmailCommand)
    {
        var result = await _mediator.Send(sendEmailCommand);
        return Ok(result);
    }
}

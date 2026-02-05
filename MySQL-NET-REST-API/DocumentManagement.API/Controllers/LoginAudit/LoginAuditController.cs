using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AuthChecker;
using DocumentManagement.Data.Resources;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.API.Controllers.LoginAudit;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LoginAuditController : ControllerBase
{
    public IMediator _mediator { get; set; }
    public LoginAuditController(IMediator mediator)
    {
        _mediator = mediator;
    }
    /// <summary>
    /// Get All Login Audit detail
    /// </summary>
    /// <param name="loginAuditResource"></param>
    /// <returns></returns>
    [HttpGet]
    [Produces("application/json", "application/xml", Type = typeof(LoginAuditList))]
    // [ClaimCheck("view_login_audit_logs")]
    public async Task<IActionResult> GetLoginAudit([FromQuery] LoginAuditResource loginAuditResource)
    {
        var getAllLoginAuditQuery = new GetAllLoginAuditQuery
        {
            LoginAuditResource = loginAuditResource
        };
        var result = await _mediator.Send(getAllLoginAuditQuery);

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

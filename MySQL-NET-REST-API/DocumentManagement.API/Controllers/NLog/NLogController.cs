using System;
using System.Text.Json;
using System.Threading.Tasks;
using AuthChecker;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Resources;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class NLogController : BaseController
{
    public IMediator _mediator { get; set; }
    public NLogController(IMediator mediator)
    {
        _mediator = mediator;
    }
    /// <summary>
    /// Get System Logs
    /// </summary>
    /// <param name="nLogResource"></param>
    /// <returns></returns>
    [HttpGet]
    [Produces("application/json", "application/xml", Type = typeof(NLogList))]
    // [ClaimCheck("view_error_logs")]
    public async Task<IActionResult> GetNLogs([FromQuery] NLogResource nLogResource)
    {
        var getAllLoginAuditQuery = new GetNLogsQuery
        {
            NLogResource = nLogResource
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

    /// <summary>
    /// Get Log By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [Produces("application/json", "application/xml", Type = typeof(NLogDto))]
    // [ClaimCheck("view_error_logs")]
    public async Task<IActionResult> GetNLog(Guid id)
    {
        var getLogQuery = new GetLogQuery { Id = id };
        var result = await _mediator.Send(getLogQuery);
        return GenerateResponse(result);
    }

}


using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Resources;
using DocumentManagement.MediatR.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DocumentManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class WorkflowTransitionLogController(IMediator _mediator) : BaseController
{
    /// <summary>
    /// Get all workflow transition log
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetWorkflowTransitionLogs")]
    [Produces("application/json", "application/xml", Type = typeof(List<WorkflowTransitionLogDto>))]
    public async Task<IActionResult> GetWorkflowTransitionLogs([FromQuery] WorkflowLogResource workflowLogResource)
    {
        var query = new GetAllWorkflowTransitionInstanceCommand
        {
            workflowLogResource = workflowLogResource
        };
        var result = await _mediator.Send(query);
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

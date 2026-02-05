using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Resources;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace DocumentManagement.API.Controllers;

/// <summary>
/// UserNotification
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserNotificationController : ControllerBase
{
    public IMediator _mediator { get; set; }
    /// <summary>
    /// UserNotification
    /// </summary>
    /// <param name="mediator"></param>
    public UserNotificationController(IMediator mediator)
    {
        _mediator = mediator;
    }
    /// <summary>
    /// Get Notifications for login user
    /// </summary>
    /// <returns></returns>
    [HttpGet("Notification")]
    [Produces("application/json", "application/xml", Type = typeof(List<UserNotificationDto>))]
    public async Task<IActionResult> GetNotification()
    {
        var getAllUserNotificationQuery = new GetUserNotificationQuery { };
        var result = await _mediator.Send(getAllUserNotificationQuery);
        return Ok(result);
    }

    /// <summary>
    /// Get All Document Audit Trail detail
    /// </summary>
    /// <param name="notificationResource"></param>
    /// <returns></returns>
    [HttpGet("Notifications")]
    [Produces("application/json", "application/xml", Type = typeof(NotificationList))]
    public async Task<IActionResult> GetNotifications([FromQuery] NotificationResource notificationResource)
    {
        var getAllDocumentAuditTrailQuery = new GetAllUserNotificationQuery
        {
            NotificationResource = notificationResource
        };
        var result = await _mediator.Send(getAllDocumentAuditTrailQuery);

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
    /// Mark Notification as Read.
    /// </summary>
    /// <param name="markAsReadCommand"></param>
    /// <returns></returns>
    [HttpPost("MarkAsRead")]
    public async Task<IActionResult> MarkAsRead(MarkAsReadCommand markAsReadCommand)
    {
        await _mediator.Send(markAsReadCommand);
        return Ok();
    }

    /// <summary>
    /// Mark All Notification As Read.
    /// </summary>
    /// <returns></returns>
    [HttpPost("MarkAllAsRead")]
    public async Task<IActionResult> MarkAllAsRead()
    {
        var markAllAsReadCommand = new MarkAllAsReadCommand();
        await _mediator.Send(markAllAsReadCommand);
        return Ok();
    }
}

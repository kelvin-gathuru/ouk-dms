using System;
using System.Text.Json;
using System.Threading.Tasks;
using AuthChecker;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Resources;
using DocumentManagement.MediatR.CommandAndQuery;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.API.Controllers.Reminder;

[Route("api/[controller]")]
[ApiController]
public class ReminderController : BaseController
{
    public IMediator _mediator { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ReminderController"/> class.
    /// </summary>
    /// <param name="mediator">The mediator.</param>
    public ReminderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets the reminders.
    /// </summary>
    /// <param name="reminderResource">The reminder resource.</param>
    /// <returns></returns>
    [HttpGet("all")]
    [Produces("application/json", "application/xml", Type = typeof(ReminderList))]
    // [ClaimCheck("view_reminders")]
    public async Task<IActionResult> GetReminders([FromQuery] ReminderResource reminderResource)
    {
        var getAllReminderQuery = new GetAllReminderQuery
        {
            ReminderResource = reminderResource
        };

        var result = await _mediator.Send(getAllReminderQuery);

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

    [HttpGet("all/currentuser")]
    [Produces("application/json", "application/xml", Type = typeof(ReminderList))]
    public async Task<IActionResult> GetReminderForLoginUser([FromQuery] ReminderResource reminderResource)
    {
        var getReminderForLoginUserCommand = new GetReminderForLoginUserCommand
        {
            ReminderResource = reminderResource
        };

        var result = await _mediator.Send(getReminderForLoginUserCommand);

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
    /// Creates the reminder.
    /// </summary>
    /// <param name="addReminderCommand">The add reminder command.</param>
    /// <returns></returns>
    [HttpPost]
    [Produces("application/json", "application/xml", Type = typeof(ReminderDto))]
    // [ClaimCheck("create_reminder", "assigned_add_reminder", "all_add_reminder")]
    public async Task<IActionResult> CreateReminder(AddReminderCommand addReminderCommand)
    {
        var result = await _mediator.Send(addReminderCommand);
        return Ok(result);
    }

    /// <summary>
    /// Gets the reminder.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [Produces("application/json", "application/xml", Type = typeof(ReminderList))]
    public async Task<IActionResult> GetReminder(Guid id)
    {
        var getReminderByIdQuery = new GetReminderByIdQuery
        {
            Id = id
        };

        var result = await _mediator.Send(getReminderByIdQuery);

        return GenerateResponse(result);
    }

    /// <summary>
    /// Updates the reminder.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="updateReminderCommand">The update reminder command.</param>
    /// <returns></returns>
    [HttpPost("{id}")]
    [Produces("application/json", "application/xml", Type = typeof(ReminderDto))]
    // [ClaimCheck("edit_reminder")]
    public async Task<IActionResult> UpdateReminder(Guid id, UpdateReminderCommand updateReminderCommand)
    {
        var result = await _mediator.Send(updateReminderCommand);
        return GenerateResponse(result);
    }
    [HttpDelete("{id}/currentuser")]
    [Produces("application/json", "application/xml", Type = typeof(bool))]
    // [ClaimCheck("delete_reminder")]
    public async Task<IActionResult> DeleteReminderCurrentUser(Guid id)
    {
        var deleteReminderCurrentUserCommand = new DeleteReminderCurrentUserCommand { Id = id };
        var result = await _mediator.Send(deleteReminderCurrentUserCommand);
        return GenerateResponse(result);
    }

    /// <summary>
    /// Deletes the reminder.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [Produces("application/json", "application/xml", Type = typeof(bool))]
    // [ClaimCheck("delete_reminder")]
    public async Task<IActionResult> DeleteReminder(Guid id)
    {
        var deleteReminderCommaond = new DeleteReminderCommand { Id = id };
        var result = await _mediator.Send(deleteReminderCommaond);
        return GenerateResponse(result);
    }



    /// <summary>
    /// Gets the reminder.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpGet("notofication/top10")]
    [Produces("application/json", "application/xml")]
    public async Task<IActionResult> GetTop10ReminderNotification()
    {
        var getTop10ReminderNotificationQuery = new GetTop10ReminderNotificationQuery
        {
        };

        var result = await _mediator.Send(getTop10ReminderNotificationQuery);
        return Ok(result);
    }
    /// <summary>
    /// Gets the reminder.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpGet("notifications")]
    [Produces("application/json", "application/xml")]
    public async Task<IActionResult> GetNotifications([FromQuery] ReminderResource reminderResource)
    {
        var getAllReminderNotificationQuery = new GetAllReminderNotificationQuery
        {
            ReminderResource = reminderResource
        };

        var result = await _mediator.Send(getAllReminderNotificationQuery);

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

    //[HttpGet("notification/markasread")]
    //[Produces("application/json", "application/xml")]
    //public async Task<IActionResult> GetNotificationMarkasRead()
    //{
    //    var markAsReadAllNotificationsCommand = new MarkAsReadAllNotificationsCommand
    //    {
    //    };

    //    var result = await _mediator.Send(markAsReadAllNotificationsCommand);

    //    return Ok(result);
    //}

    /// <summary>
    /// Gets the reminder by documentId.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpGet("{documentId}/check")]
    [Produces("application/json", "application/xml")]
    public async Task<IActionResult> CheckRemindrByDocumentId(Guid documentId)
    {
        var command = new CheckReminderByDocumentCommand()
        {
            DocumentId = documentId
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }


}

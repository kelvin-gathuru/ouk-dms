using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthChecker;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Commands.Email;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.API.Controllers.Email;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EmailSMTPSettingController : BaseController
{
    readonly IMediator _mediator;
    public EmailSMTPSettingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Create an Email SMTP Configuration.
    /// </summary>
    /// <param name="addEmailSMTPSettingCommand"></param>
    /// <returns></returns>
    [HttpPost]
    [Produces("application/json", "application/xml", Type = typeof(EmailSMTPSettingDto))]
    // [ClaimCheck("create_smtp_setting")]
    public async Task<IActionResult> AddEmailSMTPSetting(AddEmailSMTPSettingCommand addEmailSMTPSettingCommand)
    {
        var result = await _mediator.Send(addEmailSMTPSettingCommand);
        return GenerateResponse(result);
    }

    /// <summary>
    /// Get Email SMTP Configuration.
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id}")]
    [Produces("application/json", "application/xml", Type = typeof(EmailSMTPSettingDto))]
    // [ClaimCheck("view_smtp_settings")]
    public async Task<IActionResult> GetEmailSMTPSetting(Guid id)
    {
        var query = new GetEmailSMTPSettingQuery() { Id = id };
        var result = await _mediator.Send(query);
        return GenerateResponse(result);
    }

    /// <summary>
    /// Get Email SMTP Configuration list.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Produces("application/json", "application/xml", Type = typeof(List<EmailSMTPSettingDto>))]
    // [ClaimCheck("view_smtp_settings")]
    public async Task<IActionResult> GetEmailSMTPSettings()
    {
        var query = new GetEmailSMTPSettingsQuery() { };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Update an Email SMTP Configuration.
    /// </summary>
    /// <param name="addEmailSMTPSettingCommand"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [Produces("application/json", "application/xml", Type = typeof(EmailSMTPSettingDto))]
    // [ClaimCheck("edit_smtp_setting")]
    public async Task<IActionResult> UpdateEmailSMTPSetting(Guid id, UpdateEmailSMTPSettingCommand updateEmailSMTPSettingCommand)
    {
        updateEmailSMTPSettingCommand.Id = id;
        var result = await _mediator.Send(updateEmailSMTPSettingCommand);
        return GenerateResponse(result);
    }

    /// <summary>
    /// Delete an Email SMTP Configuration.
    /// </summary>
    /// <param name="addEmailSMTPSettingCommand"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [Produces("application/json", "application/xml", Type = typeof(EmailSMTPSettingDto))]
    // [ClaimCheck("delete_smtp_setting")]
    public async Task<IActionResult> DeleteEmailSMTPSetting(Guid id)
    {
        var deleteEmailSMTPSettingCommand = new DeleteEmailSMTPSettingCommand() { Id = id };
        var result = await _mediator.Send(deleteEmailSMTPSettingCommand);
        return GenerateResponse(result);
    }

    /// <summary>
    /// Get Email SMTP Configuration.
    /// </summary>
    /// <returns></returns>
    [HttpGet("check")]
    [Produces("application/json", "application/xml")]
    public async Task<IActionResult> CheckEmailSMTPSetting()
    {
        var query = new GetEmailSMTPSettingCommand();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("test")]
    [Produces("application/json", "application/xml", Type = typeof(EmailSMTPSettingDto))]
    public async Task<IActionResult> TestEmailSMTPSetting(SendTestEmailCommand sendTestEmailCommand)
    {
        var result = await _mediator.Send(sendTestEmailCommand);
        return GenerateResponse(result);
    }
}

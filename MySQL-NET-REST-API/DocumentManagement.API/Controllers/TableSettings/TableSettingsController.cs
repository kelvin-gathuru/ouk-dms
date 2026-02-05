using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DocumentManagement.API.Controllers.TableSettings;

[Route("api/[controller]")]
[ApiController]
public class TableSettingsController : ControllerBase
{
    public IMediator _mediator { get; set; }
    public TableSettingsController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpGet("{screenName}")]
    public async Task<IActionResult> GetTableSettings(string screenName)
    {
        var query = new GetTableSettingsQuery { ScreenName = screenName };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> SaveTableSettings(AddOrUpdateTableSettingCommand addOrUpdateTableSettingCommand)
    {

        var result = await _mediator.Send(addOrUpdateTableSettingCommand);
        if (result.StatusCode != 200)
        {
            return StatusCode(result.StatusCode, result);
        }
        return StatusCode(result.StatusCode, result.Data);
    }
}

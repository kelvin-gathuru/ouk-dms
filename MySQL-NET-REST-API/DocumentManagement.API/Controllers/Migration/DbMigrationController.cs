using DocumentManagement.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DocumentManagement.API.Controllers.Migration
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class DbMigrationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DbMigrationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> MigrateDb()
        {
            var getCategoryQuery = new DbMigrationCommand();
            await _mediator.Send(getCategoryQuery);
            return Ok("Database Updated Successfully.");
        }
    }
}

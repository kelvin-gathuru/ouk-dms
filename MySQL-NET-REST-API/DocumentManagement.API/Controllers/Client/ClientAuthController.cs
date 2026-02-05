using DocumentManagement.MediatR.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DocumentManagement.API.Controllers
{
    [Route("api/client")]
    [ApiController]
    public class ClientAuthController : BaseController
    {
        private readonly IMediator _mediator;

        public ClientAuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<IActionResult> Signup([FromBody] ClientSignupCommand command)
        {
            var result = await _mediator.Send(command);
            return GenerateResponse(result);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] ClientLoginCommand command)
        {
            var result = await _mediator.Send(command);
            return GenerateResponse(result);
        }

        [HttpPost("activate")]
        [AllowAnonymous]
        public async Task<IActionResult> Activate([FromBody] ClientActivationCommand command)
        {
            var result = await _mediator.Send(command);
            return GenerateResponse(result);
        }
    }
}

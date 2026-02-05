using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DocumentManagement.API.Controllers
{
    /// </summary>
    [Route("api/DocumentToken")]
    [ApiController]
    [Authorize]
    public class DocumenTokenController : BaseController
    {
        public IMediator _mediator { get; set; }
        private PathHelper _pathHelper;

        /// <summary>
        /// DocumentLibrary
        /// </summary>
        /// <param name="mediator"></param>
        public DocumenTokenController(IMediator mediator, PathHelper pathHelper
             )
        {
            _mediator = mediator;
            _pathHelper = pathHelper;
        }

        [HttpGet("token/{token}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetDocumentByToken(Guid token)
        {
            var getDocumentTokenCommand = new GetDocumentTokenCommand
            {
                Token = token,
            };
            var result = await _mediator.Send(getDocumentTokenCommand);
            return Ok(result);
        }

        /// <summary>
        /// Gets the document token.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="isVersion"></param>
        /// <returns></returns>
        [HttpGet("{id}/token")]
        [AllowAnonymous]
        public async Task<ActionResult> GetDocumentToken(Guid id)
        {
            var getDocumentTokenQuery = new GetDocumentTokenQuery
            {
                Id = id,
            };
            var result = await _mediator.Send(getDocumentTokenQuery);
            return Ok(new { result });
        }

        [HttpPost("token")]
        public async Task<ActionResult> AddDocumentToken(AddDocumentTokenCommand addDocumentTokenCommand)
        {
            var result = await _mediator.Send(addDocumentTokenCommand);
            return Ok(new { result });
        }

        /// <summary>
        /// Deletes the document token.
        /// </summary>
        /// <param name="token">The identifier.</param>
        /// <returns></returns>
        [HttpDelete("{token}")]
        public async Task<ActionResult> DeleteDocumentToken(Guid token)
        {
            var deleteDocumentTokenCommand = new DeleteDocumentTokenCommand { Token = token };
            var result = await _mediator.Send(deleteDocumentTokenCommand);
            return Ok(result);
        }

    }
}

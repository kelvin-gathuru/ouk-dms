using System.Text.Json;
using System.Threading.Tasks;
using DocumentManagement.Data.Resources;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.API.Controllers
{
    /// <summary>
    /// DocumentLibrary
    /// </summary>
    [Route("api")]
    [ApiController]
    [Authorize]
    public class DocumentLibraryController : BaseController
    {
        public IMediator _mediator { get; set; }

        /// <summary>
        /// DocumentLibrary
        /// </summary>
        /// <param name="mediator"></param>
        public DocumentLibraryController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Get Document list which user allow to view
        /// </summary>
        /// <param name="documentResource"></param>
        /// <returns></returns>
        [HttpGet("DocumentLibraries")]
        [Produces("application/json", "application/xml", Type = typeof(DocumentList))]
       
        public async Task<IActionResult> GetDocumentLibraries([FromQuery] DocumentResource documentResource)
        {
            var getDocumentLibraryQuery = new GetDocumentLibraryQuery
            {
                DocumentResource = documentResource
            };

            if (string.IsNullOrWhiteSpace(Email))
            {
                return Unauthorized();
            }

            getDocumentLibraryQuery.Email = Email;
            var result = await _mediator.Send(getDocumentLibraryQuery);
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
}

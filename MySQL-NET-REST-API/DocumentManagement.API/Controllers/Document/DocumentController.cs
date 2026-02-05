using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AuthChecker;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Dto.Document;
using DocumentManagement.Data.Resources;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Handlers;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;


namespace DocumentManagement.API.Controllers;

/// <summary>
/// Document
/// </summary>
[Route("api")]
[ApiController]
[Authorize]
public class DocumentController : BaseController
{
    public IMediator _mediator { get; set; }
    private readonly PathHelper _pathHelper;
    private readonly IConfiguration _configuration;
    /// <summary>
    /// Document
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="webHostEnvironment"></param>
    /// <param name="pathHelper"></param>
    public DocumentController(
        IMediator mediator,
        IConfiguration configuration,
        PathHelper pathHelper
        )
    {
        _mediator = mediator;
        _configuration = configuration;
        _pathHelper = pathHelper;

    }
    /// <summary>
    /// Get Archive Documents
    /// </summary>
    /// <param name="documentResource"></param>
    /// <returns></returns>
    [HttpGet("Documents/archive")]
    [Produces("application/json", "application/xml", Type = typeof(DocumentList))]
    public async Task<IActionResult> GetArchieveDocuments([FromQuery] DocumentResource documentResource)
    {
        documentResource.IsArchive = true;
        var getAllArchiveDocumentQuery = new GetAllArchiveDocumentQuery
        {
            DocumentResource = documentResource
        };
        var result = await _mediator.Send(getAllArchiveDocumentQuery);

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
    [HttpPost("Documents/ocr_content_extractor")]
    [Produces("application/json", "application/xml", Type = typeof(DocumentList))]
    // [ClaimCheck("ocr_content_extractor")]
    public async Task<IActionResult> GetContentByOCR([FromForm] OCRContentExtractorCommand oCRContentExtractorCommand)
    {
        var result = await _mediator.Send(oCRContentExtractorCommand);
        if (!result.Success)
        {
            return StatusCode(result.StatusCode, result.Errors);
        }
        return StatusCode(result.StatusCode, result.Data);
    }

    [HttpGet("Document/{id}/isChunked")]
    [Produces("application/json", "application/xml", Type = typeof(bool))]
    [AllowAnonymous]
    public async Task<IActionResult> checkDocumentStoreAsChunk(Guid id)
    {

        var checkDocumentStoreAsChunkCommand = new CheckDocumentStoreAsChunkCommand
        {
            DocumentId = id
        };
        var result = await _mediator.Send(checkDocumentStoreAsChunkCommand);

        return Ok(result.Data);
    }

    [HttpGet("Document/{id}/summary")]
    [Produces("application/json", "application/xml", Type = typeof(bool))]
    // [ClaimCheck("ALL_Get_Document_Summary", "assigned_get_document_summary")]
    public async Task<IActionResult> getDocumentSummary(Guid id)
    {
        var getDocumentSummaryCommand = new GetDocumentSummaryCommand
        {
            DocumentId = id
        };
        var result = await _mediator.Send(getDocumentSummaryCommand);
        if (!result.Success)
        {
            return StatusCode(result.StatusCode, result.Errors);
        }
        return Ok(result.Data);
    }
    /// <summary>
    /// Get Document By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("Document/{id}/sharedUsersRoles", Name = "GetDocumentSharedUsersRoles")]
    [Produces("application/json", "application/xml", Type = typeof(DocumentDto))]
    public async Task<IActionResult> GetDocumentSharedUsersRoles(Guid id)
    {
        var getDocumentQuery = new GetDocumentQuery
        {
            Id = id
        };
        var response = await _mediator.Send(getDocumentQuery);
        if (!response.Success)
        {
            return StatusCode(response.StatusCode, response.Errors);
        }
        var result = response.Data;
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("Document/{id}/Detail", Name = "GetDocumentDetailById")]
    [Produces("application/json", "application/xml", Type = typeof(DocumentDto))]
    public async Task<IActionResult> GetDocumentDetailById(Guid id)
    {
        var getDocumentDetailById = new GetDocumentDetailById
        {
            Id = id
        };
        var response = await _mediator.Send(getDocumentDetailById);

        return Ok(response);
    }
    [HttpGet("Document/ArchiveRetentionPeriod")]
    public async Task<IActionResult> GetArchiveRetentionPeriod()
    {
        var getArchiveRetentionCommand = new GetArchiveRetentionCommand();
        var archiveRetention = await _mediator.Send(getArchiveRetentionCommand);
        return Ok(archiveRetention);
    }

    /// <summary>
    /// Get Document By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("Document/{id}", Name = "GetDocument")]
    [Produces("application/json", "application/xml", Type = typeof(DocumentDto))]
    public async Task<IActionResult> GetDocument(Guid id)
    {
        var getDocumentQuery = new GetDocumentQuery
        {
            Id = id
        };
        var response = await _mediator.Send(getDocumentQuery);
        if (!response.Success)
        {
            return StatusCode(response.StatusCode, response.Errors);
        }
        var result = response.Data;
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("Document/deepSearch", Name = "DeepSearch")]
    [Produces("application/json", "application/xml", Type = typeof(List<DocumentDto>))]
    // [ClaimCheck("deep_search")]
    public async Task<IActionResult> DeepSearch(string searchQuery)
    {
        var deepSearchCommand = new DeepSearchCommand
        {
            SearchQuery = searchQuery
        };
        var response = await _mediator.Send(deepSearchCommand);
        if (!response.Success)
        {
            return StatusCode(response.StatusCode, response.Errors);
        }
        return Ok(response.Data);
    }

    [HttpPost("Document/{id}/remove/pageindexing", Name = "RemovePageIndexing")]
    [Produces("application/json", "application/xml", Type = typeof(List<DocumentDto>))]
    // [ClaimCheck("remove_document_index")]
    public async Task<IActionResult> RemovePageIndexing(Guid Id)
    {
        var removePageIndexingCommand = new RemovePageIndexingCommand
        {
            DocumentId = Id
        };
        var response = await _mediator.Send(removePageIndexingCommand);
        if (!response.Success)
        {
            return StatusCode(response.StatusCode, response.Errors);
        }
        return Ok(response);
    }

    [HttpPost("Document/{id}/add/pageindexing", Name = "AddPageIndexing")]
    [Produces("application/json", "application/xml", Type = typeof(List<DocumentDto>))]
    // [ClaimCheck("add_document_index")]
    public async Task<IActionResult> AddPageIndexing(Guid Id)
    {
        var addPageIndexingCommand = new AddPageIndexingCommand
        {
            DocumentId = Id
        };
        var response = await _mediator.Send(addPageIndexingCommand);
        if (!response.Success)
        {
            return StatusCode(response.StatusCode, response.Errors);
        }
        return Ok(response);
    }

    /// <summary>
    /// Get All Documents
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="Id"></param>
    /// <param name="CreatedBy"></param>
    /// <returns></returns>
    [HttpGet("Documents")]
    [Produces("application/json", "application/xml", Type = typeof(DocumentList))]
    // [ClaimCheck("ALL_view_documents")]
    public async Task<IActionResult> GetDocuments([FromQuery] DocumentResource documentResource)
    {
        documentResource.IsArchive = false;
        var getAllDocumentQuery = new GetAllDocumentQuery
        {
            DocumentResource = documentResource
        };
        var result = await _mediator.Send(getAllDocumentQuery);

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

    [HttpGet("Documents/public/petitions")]
    [Produces("application/json", "application/xml", Type = typeof(DocumentList))]
    [AllowAnonymous]
    public async Task<IActionResult> GetPublicPetitionDocuments()
    {
        var categoryId = _configuration["PetitionCategoryId"];
        if (string.IsNullOrEmpty(categoryId))
        {
            return BadRequest("Petition Category ID is not configured.");
        }

        var documentResource = new DocumentResource
        {
            CategoryId = categoryId,
            IsArchive = false
        };

        var getAllDocumentQuery = new GetAllDocumentQuery
        {
            DocumentResource = documentResource
        };

        var result = await _mediator.Send(getAllDocumentQuery);
        return Ok(result);
    }

    [HttpGet("Documents/public/petitions/{id}/download")]
    [AllowAnonymous]
    public async Task<IActionResult> DownloadPublicPetitionDocument(Guid id)
    {
        var categoryIdStr = _configuration["PetitionCategoryId"];
        if (string.IsNullOrEmpty(categoryIdStr))
        {
            return BadRequest("Petition Category ID is not configured.");
        }

        if (!Guid.TryParse(categoryIdStr, out var categoryId))
        {
            return BadRequest("Invalid Petition Category ID configuration.");
        }

        // 1. Verify Document Category
        var getDocumentQuery = new GetDocumentQuery
        {
            Id = id
        };
        var docResponse = await _mediator.Send(getDocumentQuery);
        
        if (!docResponse.Success || docResponse.Data == null)
        {
            return NotFound("Document not found.");
        }

        if (docResponse.Data.CategoryId != categoryId)
        {
            return Forbid("Access to this document is restricted.");
        }

        // 2. Download Document
        var downloadCommand = new DownloadDocumentCommand
        {
            Id = id,
            IsVersion = false
        };

        var downloadResponse = await _mediator.Send(downloadCommand);
        if (!downloadResponse.Success)
        {
            return GenerateResponse(downloadResponse);
        }

        var downloadDocument = downloadResponse.Data;
        return File(downloadDocument.Data, downloadDocument.ContentType, downloadDocument.FileName);
    }
    /// <param name="addDocumentCommand"></param>
    /// <returns></returns>
    [HttpPost("Document")]
    [DisableRequestSizeLimit]
    [Produces("application/json", "application/xml", Type = typeof(DocumentDto))]
    // [ClaimCheck("ALL_create_document")]
    public async Task<IActionResult> AddDocument([FromForm] AddDocumentCommand addDocumentCommand)
    {
        var result = await _mediator.Send(addDocumentCommand);
        if (!result.Success)
        {
            return StatusCode(result.StatusCode, result.Errors);
        }
        return CreatedAtAction("GetDocument", new { id = result.Data.Id }, result.Data);
    }
    
    /// <summary>
    /// Create a document from client (external user).
    /// </summary>
    /// <param name="addClientDocumentCommand"></param>
    /// <returns></returns>
    [HttpPost("ClientDocument")]
    [DisableRequestSizeLimit]
    [Produces("application/json", "application/xml", Type = typeof(DocumentDto))]
    [AllowAnonymous]
    public async Task<IActionResult> AddClientDocument([FromForm] AddClientDocumentCommand addClientDocumentCommand)
    {
        var result = await _mediator.Send(addClientDocumentCommand);
        if (!result.Success)
        {
            return StatusCode(result.StatusCode, result.Errors);
        }
        return CreatedAtAction("GetDocument", new { id = result.Data.Id }, result.Data);
    }
    /// <summary>
    /// AI Document Generated
    /// </summary>
    /// <param name="addDocumentCommand"></param>
    /// <returns></returns>
    [HttpPost("Document/aiDocumentCreated")]
    [DisableRequestSizeLimit]
    [Produces("application/json", "application/xml", Type = typeof(DocumentDto))]
    // [ClaimCheck("ALL_create_document")]
    public async Task<IActionResult> AddAIDocumentCreated(AddAIDocumentCreatedCommand addAIDocumentCreatedCommand)
    {
        var result = await _mediator.Send(addAIDocumentCreatedCommand);
        if (!result.Success)
        {
            return StatusCode(result.StatusCode, result.Errors);
        }
        return CreatedAtAction("GetDocument", new { id = result.Data.Id }, result.Data);
    }
    [HttpPost("Document/WindowShared")]
    [DisableRequestSizeLimit]
    [Produces("application/json", "application/xml", Type = typeof(DocumentDto))]
    // [ClaimCheck("ALL_create_document")]
    public async Task<IActionResult> AddDocumentWindowShared([FromForm] AddDocumentWindowSharedCommand addDocumentCommand)
    {
        var result = await _mediator.Send(addDocumentCommand);
        if (!result.Success)
        {
            return StatusCode(result.StatusCode, result.Errors);
        }
        return CreatedAtAction("GetDocument", new { id = result.Data.Id }, result.Data);
    }
    [HttpPost("Document/upload/image")]
    [DisableRequestSizeLimit]
    [Produces("application/json", "application/xml", Type = typeof(DocumentDto))]
    public async Task<IActionResult> UploadDocumentEditor([FromForm] AddDocumenFromEditorCommand addDocumenFromEditorCommand)
    {
        var result = await _mediator.Send(addDocumenFromEditorCommand);
        if (!result.Success)
        {
            return StatusCode(result.StatusCode, result.Errors);
        }
        return StatusCode(result.StatusCode, result.Data);
    }


    [HttpPost("Document/chunk/upload")]
    [DisableRequestSizeLimit]
    [Produces("application/json", "application/xml", Type = typeof(DocumentDto))]
    // [ClaimCheck("ALL_create_document")]
    public async Task<IActionResult> UploadDocumentChunk([FromForm] UploadDocumentChunkCommand uploadDocumentChunkCommand)
    {
        var result = await _mediator.Send(uploadDocumentChunkCommand);
        if (!result.Success)
        {
            return StatusCode(result.StatusCode, result.Errors);
        }
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("Document/chunk")]
    [Produces("application/json", "application/xml", Type = typeof(DocumentDto))]
    // [ClaimCheck("ALL_create_document")]
    public async Task<IActionResult> AddDocumentChunk(AddDocumentChunkCommand addDocumentChunkCommand)
    {
        var result = await _mediator.Send(addDocumentChunkCommand);
        if (!result.Success)
        {
            return StatusCode(result.StatusCode, result.Errors);
        }
        return CreatedAtAction("GetDocument", new { id = result.Data.Id }, result.Data);
    }
    [HttpPost("Document/chunk/windowshared")]
    [Produces("application/json", "application/xml", Type = typeof(DocumentDto))]
    // [ClaimCheck("ALL_create_document")]
    public async Task<IActionResult> AddDocumentChunkWindowShared(AddDocumentChunkWindowSharedCommand addDocumentChunkWindowSharedCommand)
    {
        var result = await _mediator.Send(addDocumentChunkWindowSharedCommand);
        if (!result.Success)
        {
            return StatusCode(result.StatusCode, result.Errors);
        }
        return CreatedAtAction("GetDocument", new { id = result.Data.Id }, result.Data);
    }

    [HttpPost("Document/chunk/uploadStatus")]
    [Produces("application/json", "application/xml", Type = typeof(DocumentDto))]
    // [ClaimCheck("ALL_create_document")]
    public async Task<IActionResult> MarkDocumentAllChunkUpload(MarkDocumentChunksUploadedStatusCommand markDocumentChunksUploadedStatusCommand)
    {
        var result = await _mediator.Send(markDocumentChunksUploadedStatusCommand);
        if (!result.Success)
        {
            return StatusCode(result.StatusCode, result.Errors);
        }
        return Ok(result.Data);
    }

    [HttpPost("Document/assign")]
    [DisableRequestSizeLimit]
    [Produces("application/json", "application/xml", Type = typeof(DocumentDto))]
    // [ClaimCheck("ALL_create_document", "assigned_create_document")]
    public async Task<IActionResult> AddDocumentToMe([FromForm] AddDocumentToMeCommand addDocumentCommand)
    {
        var result = await _mediator.Send(addDocumentCommand);
        if (!result.Success)
        {
            return StatusCode(result.StatusCode, result.Errors);
        }
        return CreatedAtAction("GetDocument", new { id = result.Data.Id }, result.Data);
    }
    /// <summary>
    /// Upload document.
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="updateDocumentCommand"></param>
    /// <returns></returns>
    [HttpPut("Document/{Id}")]
    [Produces("application/json", "application/xml", Type = typeof(DocumentDto))]
    // [ClaimCheck("all_edit_document", "assigned_edit_document")]
    public async Task<IActionResult> UpdateDocument(Guid Id, UpdateDocumentCommand updateDocumentCommand)
    {
        updateDocumentCommand.Id = Id;
        var result = await _mediator.Send(updateDocumentCommand);
        return StatusCode(result.StatusCode, result);
    }
    /// <summary>
    /// Delete Document.
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    [HttpDelete("Document/{Id}")]
    // [ClaimCheck("delete_document")]
    public async Task<IActionResult> DeleteDocument(Guid Id)
    {
        var deleteDocumentCommand = new DeleteDocumentCommand
        {
            Id = Id
        };
        var result = await _mediator.Send(deleteDocumentCommand);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("Document/{Id}/archive")]
    // [ClaimCheck("all_archive_document", "assigned_archive_document")]
    public async Task<IActionResult> ArchiveDocument(Guid Id)
    {
        var archiveDocumentCommand = new ArchiveDocumentCommand
        {
            DocumentId = Id
        };
        var result = await _mediator.Send(archiveDocumentCommand);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("Document/{Id}/restore")]
    // [ClaimCheck("all_restore_version", "Assigned_Restore_version")]
    public async Task<IActionResult> RestoreDocument(Guid Id)
    {
        var archiveDocumentCommand = new RestoreDocumentCommand
        {
            DocumentId = Id
        };
        var result = await _mediator.Send(archiveDocumentCommand);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("Document/{id}/download")]
    public async Task<IActionResult> DownloadDocument(Guid id, bool isVersion)
    {
        var commnad = new DownloadDocumentCommand
        {
            Id = id,
            IsVersion = isVersion
        };

        var downloadDocumentResponse = await _mediator.Send(commnad);
        if (!downloadDocumentResponse.Success)
        {
            return GenerateResponse(downloadDocumentResponse);
        }

        var downloadDocument = downloadDocumentResponse.Data;

        return File(downloadDocument.Data, downloadDocument.ContentType, downloadDocument.FileName);
    }
    [HttpGet("Document/{id}/chunks")]
    public async Task<IActionResult> DocumentChunks(Guid id, bool isVersion)
    {
        var commnad = new DocumentChunksCommand
        {
            DocumentId = id,
            IsVersion = isVersion
        };

        var documentChunks = await _mediator.Send(commnad);
        if (!documentChunks.Success)
        {
            return StatusCode(documentChunks.StatusCode, documentChunks.Errors);
        }
        return Ok(documentChunks.Data);
    }
    [HttpGet("Document/{documentVersionId}/chunk/{chunkIndex}/download")]
    public async Task<IActionResult> GetFileChunk(Guid documentVersionId, int chunkIndex)
    {
        var command = new GetFileChunkCommand
        {
            DocumentVersionId = documentVersionId,
            ChunkIndex = chunkIndex
        };

        var documentChunkFile = await _mediator.Send(command);
        if (!documentChunkFile.Success)
        {
            return GenerateResponse(documentChunkFile);
        }

        return Ok(documentChunkFile.Data);
    }

    [HttpGet("Document/{id}/officeviewer")]
    [AllowAnonymous]
    public async Task<IActionResult> GetDocumentFileByToken(Guid id, [FromQuery] OfficeViewerRequest officeViewerRequest)
    {
        var getDocumentTokenCommand = new GetDocumentPathByTokenCommand
        {
            Id = id,
            Token = officeViewerRequest.Token,
            IsPublic = officeViewerRequest.IsPublic,
            IsFileRequest = officeViewerRequest.IsFileRequest
        };

        var result = await _mediator.Send(getDocumentTokenCommand);
        if (!result)
        {
            return NotFound();
        }

        if (officeViewerRequest.IsPublic)
        {
            var downloadSharedDocumentComamnd = new DownloadSharedDocumentCommand
            {
                Code = id.ToString(),
                Password = officeViewerRequest.Password
            };

            var downloadResult = await _mediator.Send(downloadSharedDocumentComamnd);
            if (!downloadResult.Success)
            {
                return GenerateResponse(downloadResult);
            }

            var sharedDocument = downloadResult.Data;
            return File(sharedDocument.Data, sharedDocument.ContentType, sharedDocument.FileName);
        }
        if (officeViewerRequest.IsFileRequest)
        {
            var downloadFileRequestDocumentCommand = new DownloadFileRequestDocumentCommand
            {
                Id = id
            };

            var downloadResult = await _mediator.Send(downloadFileRequestDocumentCommand);
            if (!downloadResult.Success)
            {
                return GenerateResponse(downloadResult);
            }
            var sharedDocument = downloadResult.Data;
            return File(sharedDocument.Data, sharedDocument.ContentType, sharedDocument.FileName);
        }

        var commnad = new DownloadDocumentChunkCommand
        {
            Id = id,
            DocumentVersionId = officeViewerRequest.DocumentVersionId
        };

        var downloadDocumentResponse = await _mediator.Send(commnad);

        if (!downloadDocumentResponse.Success)
        {
            return GenerateResponse(downloadDocumentResponse);
        }

        var downloadDocument = downloadDocumentResponse.Data;
        return File(downloadDocument.Data, downloadDocument.ContentType, downloadDocument.FileName);
    }


    [HttpGet("Document/{id}/isDownloadFlag")]
    [AllowAnonymous]
    public async Task<ActionResult> GetIsDownloadFlag(Guid id)
    {
        var deleteDocumentTokenCommand = new GetIsDownloadFlagQuery
        {
            DocumentId = id,
        };
        var result = await _mediator.Send(deleteDocumentTokenCommand);
        return Ok(result);
    }

    /// <summary>
    /// Read text Document
    /// </summary>
    /// <param name="id"></param>
    /// <param name="isVersion"></param>
    /// <returns></returns>
    [HttpGet("Document/{id}/readText")]
    public async Task<IActionResult> ReadTextDocument(Guid id, bool isVersion)
    {
        var commnad = new DownloadDocumentCommand
        {
            Id = id,
            IsVersion = isVersion
        };
        var downloadDocumentResponse = await _mediator.Send(commnad);

        if (!downloadDocumentResponse.Success)
        {
            return GenerateResponse(downloadDocumentResponse);
        }

        var downloadDocument = downloadDocumentResponse.Data;
        string utfString = Encoding.UTF8.GetString(downloadDocument.Data, 0, downloadDocument.Data.Length);
        return Ok(new { result = new string[] { utfString } });

    }

    [HttpGet("Document/{id}/getMetatag")]
    public async Task<IActionResult> GetDocumentMetatags(Guid id)
    {
        var commnad = new GetDocumentMetaDataByIdQuery
        {
            DocumentId = id,
        };
        var documentMetas = await _mediator.Send(commnad);
        return Ok(documentMetas);
    }

    [HttpGet("Document/{id}/userSignBy")]
    public async Task<IActionResult> CheckDocumentByUser(Guid id)
    {
        var commnad = new DocumentSignByUserCommand
        {
            DocumentId = id,
        };
        var flag = await _mediator.Send(commnad);
        return Ok(flag);
    }

    [HttpGet("Document/{id}/permission")]
    public async Task<IActionResult> CheckDocumentPermission(Guid id)
    {
        var commnad = new CheckDocumentPermissionCommand
        {
            DocumentId = id,
        };
        var result = await _mediator.Send(commnad);
        return Ok(result.Data);
    }

    [HttpPost("Document/ArchiveRetentionPeriod")]
    // [ClaimCheck("Manage_Archive_Retention_Period")]
    public async Task<IActionResult> ArchiveRetentionPeriod(AddUpdateArchiveRetentionCommand addUpdateArchiveRetentionCommand)
    {
        var flag = await _mediator.Send(addUpdateArchiveRetentionCommand);
        return Ok(flag);
    }


}

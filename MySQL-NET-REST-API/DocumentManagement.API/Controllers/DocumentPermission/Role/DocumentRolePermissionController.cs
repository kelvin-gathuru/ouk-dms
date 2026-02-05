using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthChecker;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.API.Controllers.DocumentPermission;

/// <summary>
/// DocumentRolePermission
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DocumentRolePermissionController : ControllerBase
{
    public IMediator _mediator { get; set; }
    /// <summary>
    /// DocumentRolePermission
    /// </summary>
    /// <param name="mediator"></param>
    public DocumentRolePermissionController(IMediator mediator)
    {
        _mediator = mediator;
    }
    /// <summary>
    /// Get Document Permissions
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [Produces("application/json", "application/xml", Type = typeof(List<DocumentPermissionDto>))]
    public async Task<IActionResult> GetDocumentPermissions(Guid id)
    {
        var getDocumentQuery = new GetDocumentPermissionQuery
        {
            DocumentId = id
        };
        var result = await _mediator.Send(getDocumentQuery);
        return Ok(result);
    }
    /// <summary>
    /// Add Document Role Permission
    /// </summary>
    /// <param name="addDocumentRolePermissionCommand"></param>
    /// <returns></returns>
    [HttpPost]
    [Produces("application/json", "application/xml", Type = typeof(DocumentRolePermissionDto))]
    // [ClaimCheck("ALL_share_document", "assigned_share_document")]
    public async Task<IActionResult> AddDocumentRolePermission(AddDocumentRolePermissionCommand addDocumentRolePermissionCommand)
    {
        var result = await _mediator.Send(addDocumentRolePermissionCommand);
        return StatusCode(result.StatusCode, result);
    }
    /// <summary>
    /// Add Users or Roles to Multiple Documents
    /// </summary>
    /// <param name="DocumentPermissionUserRoleCommand"></param>
    /// <returns></returns>
    [HttpPost("multiple")]
    [Produces("application/json", "application/xml", Type = typeof(DocumentPermissionUserRoleCommand))]
    // [ClaimCheck("all_share_document", "assigned_share_document")]
    public async Task<IActionResult> AddMultipleDocumentsUsersAndRoles(DocumentPermissionUserRoleCommand addDocumentRolePermissionCommand)
    {
        var result = await _mediator.Send(addDocumentRolePermissionCommand);
        return Ok(result);
    }
    /// <summary>
    /// Delete Document Role Permission
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    [HttpDelete("{Id}")]
    // [ClaimCheck("all_remove_share_document", "assigned_remove_share_document")]
    public async Task<IActionResult> DeleteDocumentRolePermission(Guid Id)
    {
        var deleteRolePermissionCommand = new DeleteDocumentRolePermissionCommand
        {
            Id = Id
        };
        var result = await _mediator.Send(deleteRolePermissionCommand);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Get Folder and Document Share Permissions
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{documentId}/sharepermission/{categoryId}")]
    [Produces("application/json", "application/xml", Type = typeof(SharePermissionDto))]
    public async Task<IActionResult> GetShareDocumentAndFolderPermissions(Guid documentId, Guid categoryId)
    {
        var getSharePermissionQuery = new GetSharePermissionQuery
        {
            DocumentId = documentId,
            CategoryId = categoryId
        };
        var result = await _mediator.Send(getSharePermissionQuery);
        return Ok(result);
    }
}

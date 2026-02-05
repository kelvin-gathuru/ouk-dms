using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AuthChecker;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using DocumentManagement.Data.Resources;
using DocumentManagement.Repository;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.MediatR.Commands;

namespace DocumentManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ClientController(IMediator _mediator) : BaseController
{
    /// <summary>
    /// Creates the Client.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <returns></returns>
    [HttpPost]
    // // [ClaimCheck("add_clients")]
    public async Task<IActionResult> AddClient(AddClientCommand command)
    {
        var result = await _mediator.Send(command);
        return GenerateResponse(result);
    }

    /// <summary>
    /// Updates the Client.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="updateClientCommand">The update storage setting command.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [Produces("application/json", "application/xml", Type = typeof(WorkflowDto))]
    // [ClaimCheck("edit_clients")]
    public async Task<IActionResult> UpdateClient(Guid id, UpdateClientCommand command)
    {
        var result = await _mediator.Send(command);
        return GenerateResponse(result);
    }

    // <summary>
    /// Deletes the Client.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    // [ClaimCheck("delete_clients")]
    public async Task<IActionResult> DeleteClient(Guid id)
    {
        var query = new DeleteClientCommand { Id = id };
        var result = await _mediator.Send(query);
        return GenerateResponse(result);
    }

    /// <summary>
    /// Gets the Client.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetClient(Guid id)
    {
        var query = new GetClientQuery { Id = id };
        var result = await _mediator.Send(query);
        return Ok(result.Data);
    }

    /// <summary>
    /// Get All Clients.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Produces("application/json", "application/xml", Type = typeof(List<ClientDto>))]
    public async Task<IActionResult> GetClients()
    {
        var getAllClientQuery = new GetAllClientQuery { };
        var result = await _mediator.Send(getAllClientQuery);
        return Ok(result.Data);
    }

    /// <summary>
    /// Get Client Documents.
    /// </summary>
    /// <param name="id">The client identifier.</param>
    /// <returns></returns>
    [HttpGet("{id}/Documents")]
    [Produces("application/json", "application/xml", Type = typeof(DocumentList))]
    [AllowAnonymous]
    public async Task<IActionResult> GetClientDocuments(Guid id)
    {
        var documentResource = new DocumentResource { ClientId = id.ToString() };
        var getAllDocumentQuery = new GetAllDocumentQuery
        {
            DocumentResource = documentResource
        };
        var result = await _mediator.Send(getAllDocumentQuery);
        return Ok(result);
    }

    /// <summary>
    /// Download Client Document.
    /// </summary>
    /// <param name="id">The client identifier.</param>
    /// <param name="documentId">The document identifier.</param>
    /// <returns></returns>
    [HttpGet("{id}/Documents/{documentId}/download")]
    [AllowAnonymous]
    public async Task<IActionResult> DownloadClientDocument(Guid id, Guid documentId)
    {
        // First verify the document belongs to the client
        var getDocumentQuery = new GetDocumentQuery { Id = documentId };
        var documentResponse = await _mediator.Send(getDocumentQuery);
        
        if (!documentResponse.Success)
        {
            return NotFound("Document not found.");
        }

        var document = documentResponse.Data;
        if (document.ClientId != id)
        {
            return Unauthorized("Document does not belong to this client.");
        }

        // Proceed to download
        var command = new DownloadDocumentCommand
        {
            Id = documentId,
            IsVersion = false
        };

        var downloadDocumentResponse = await _mediator.Send(command);
        if (!downloadDocumentResponse.Success)
        {
            return GenerateResponse(downloadDocumentResponse);
        }

        var downloadDocument = downloadDocumentResponse.Data;
        return File(downloadDocument.Data, downloadDocument.ContentType, downloadDocument.FileName);
    }
    /// <summary>
    /// Get Client Dashboard Data.
    /// </summary>
    /// <param name="id">The client identifier.</param>
    /// <returns></returns>
    [HttpGet("{id}/Dashboard")]
    [Produces("application/json", "application/xml", Type = typeof(ClientDashboardDto))]
    [AllowAnonymous]
    public async Task<IActionResult> GetClientDashboard(Guid id)
    {
        var documentResource = new DocumentResource 
        { 
            ClientId = id.ToString(),
            PageSize = 1000 // Fetch enough to calculate stats
        };
        var getAllDocumentQuery = new GetAllDocumentQuery
        {
            DocumentResource = documentResource
        };
        var result = await _mediator.Send(getAllDocumentQuery);
        
        if (result == null)
        {
            return NotFound("Client documents not found.");
        }

        var documents = result; // result is DocumentList which inherits from List<DocumentDto>
        
        var dashboard = new ClientDashboardDto();
        
        // Calculate stats
        dashboard.TotalPetitions = documents.Count;
        dashboard.PendingCount = documents.Count(d => d.DocumentStatus?.Name?.ToUpper() == "SUBMITTED" || d.DocumentStatus?.Name?.ToUpper() == "RECEIVED" || d.DocumentStatus?.Name?.ToUpper() == "UNDER_REVIEW");
        dashboard.ApprovedCount = documents.Count(d => d.DocumentStatus?.Name?.ToUpper() == "APPROVED" || d.DocumentStatus?.Name?.ToUpper() == "PRESENTED");
        dashboard.RejectedCount = documents.Count(d => d.DocumentStatus?.Name?.ToUpper() == "REJECTED");

        // Recent Documents (Top 5)
        dashboard.RecentDocuments = documents.OrderByDescending(d => d.CreatedDate).Take(5).ToList();

        // Generate Recent Activities
        foreach (var doc in documents.OrderByDescending(d => d.ModifiedDate).Take(10))
        {
            dashboard.RecentActivities.Add(new DashboardActivityDto
            {
                Description = $"Petition '{doc.Description ?? doc.Name}' was updated.",
                Timestamp = doc.ModifiedDate ?? doc.CreatedDate,
                Type = "UPDATE",
                DocumentId = doc.Id
            });
        }
        
        // Add creation activities
        foreach (var doc in documents.OrderByDescending(d => d.CreatedDate).Take(10))
        {
             dashboard.RecentActivities.Add(new DashboardActivityDto
            {
                Description = $"Petition '{doc.Description ?? doc.Name}' was submitted.",
                Timestamp = doc.CreatedDate,
                Type = "SUBMISSION",
                DocumentId = doc.Id
            });
        }

        // Sort and take top 10 activities
        dashboard.RecentActivities = dashboard.RecentActivities.OrderByDescending(a => a.Timestamp).Take(10).ToList();

        return Ok(dashboard);
    }

    [HttpGet("{id}/Profile")]
    [Produces("application/json", "application/xml", Type = typeof(ClientProfileDto))]
    [AllowAnonymous]
    public async Task<IActionResult> GetClientProfile(Guid id)
    {
        var getClientQuery = new GetClientQuery { Id = id };
        var result = await _mediator.Send(getClientQuery);

        if (result == null || !result.Success || result.Data == null)
        {
            return NotFound("Client not found.");
        }

        var client = result.Data;

        var profile = new ClientProfileDto
        {
            Id = client.Id,
            CompanyName = client.CompanyName,
            ContactPerson = client.ContactPerson,
            Email = client.Email,
            PhoneNumber = client.PhoneNumber,
            Address = client.Address,
            CreatedDate = client.CreatedDate
        };

        return Ok(profile);
    }

    [HttpPost("{id}/change-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ChangePassword(Guid id, [FromBody] ChangeClientPasswordCommand command)
    {
        if (id != command.Id)
        {
            // If the ID in the URL doesn't match the command (or if command.Id is empty), set it.
            // But better to enforce consistency or just use one.
            // Let's assume the frontend sends the body without ID, or we override it.
            command.Id = id;
        }

        var result = await _mediator.Send(command);
        return GenerateResponse(result);
    }
    [HttpPost("forgot-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword(ClientForgotPasswordCommand command)
    {
        var result = await _mediator.Send(command);
        return GenerateResponse(result);
    }

    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword(ClientResetPasswordCommand command)
    {
        var result = await _mediator.Send(command);
        return GenerateResponse(result);
    }
}

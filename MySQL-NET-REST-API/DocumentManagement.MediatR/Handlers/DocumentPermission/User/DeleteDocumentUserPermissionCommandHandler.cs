using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace DocumentManagement.MediatR.Handlers;

public class DeleteDocumentUserPermissionCommandHandler
    : IRequestHandler<DeleteDocumentUserPermissionCommand, DocumentUserPermissionDto>
{
    private readonly IDocumentUserPermissionRepository _documentUserPermissionRepository;
    private readonly IDocumentRolePermissionRepository _documentRolePermissionRepository;
    private readonly IDocumentRepository _documentRepository;
    private readonly IUnitOfWork<DocumentContext> _uow;
    private readonly UserInfoToken _userInfo;
    private readonly IDocumentAuditTrailRepository _documentAuditTrailRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IHubContext<UserHub, IHubClient> _hubContext;
    private readonly IConnectionMappingRepository _connectionMappingRepository;
    private readonly ILogger<DeleteDocumentUserPermissionCommandHandler> _logger;
    public DeleteDocumentUserPermissionCommandHandler(
        IDocumentRepository documentRepository,
       IDocumentUserPermissionRepository documentUserPermissionRepository,
        IUnitOfWork<DocumentContext> uow,
          UserInfoToken userInfo,
          IDocumentAuditTrailRepository documentAuditTrailRepository,
           ICategoryRepository categoryRepository,
          IHubContext<UserHub, IHubClient> hubContext,
         IConnectionMappingRepository connectionMappingRepository,
         ILogger<DeleteDocumentUserPermissionCommandHandler> logger
,
         IDocumentRolePermissionRepository documentRolePermissionRepository)
    {
        _documentUserPermissionRepository = documentUserPermissionRepository;
        _documentRepository = documentRepository;
        _uow = uow;
        _userInfo = userInfo;
        _documentAuditTrailRepository = documentAuditTrailRepository;
        _categoryRepository = categoryRepository;
        _hubContext = hubContext;
        _connectionMappingRepository = connectionMappingRepository;
        _logger = logger;
        _documentRolePermissionRepository = documentRolePermissionRepository;
    }

    public async Task<DocumentUserPermissionDto> Handle(DeleteDocumentUserPermissionCommand request, CancellationToken cancellationToken)
    {
        var entity = await _documentUserPermissionRepository.FindAsync(request.Id);
        if (entity == null)
        {
            var errorDto = new DocumentUserPermissionDto
            {
                StatusCode = 404,
                Messages = new List<string> { "Not Found" }
            };
            return errorDto;
        }

        var documentAudit = new DocumentAuditTrail()
        {
            DocumentId = entity.DocumentId,
            CreatedBy = _userInfo.Id,
            CreatedDate = DateTime.UtcNow,
            OperationName = DocumentOperation.Removed_Permission,
            AssignToUserId = entity.UserId
        };
        _documentAuditTrailRepository.Add(documentAudit);

        _documentUserPermissionRepository.Remove(entity);
        var documentInfo = _documentRepository.Find(entity.DocumentId);
        await _documentRepository.UpdateDocumentSharingFlagAsync(new List<Guid> { documentInfo.CategoryId });
        if (await _uow.SaveAsync() <= -1)
        {
            var errorDto = new DocumentUserPermissionDto
            {
                StatusCode = 500,
                Messages = new List<string> { "An unexpected fault happened. Try again later." }
            };
            return errorDto;
        }
        var category = _categoryRepository.All.Where(c => c.Id == entity.DocumentId).FirstOrDefault();
        try
        {
            var onlineUsers = _connectionMappingRepository.GetAllUsersExceptThis(new SignlarUser { Id = _userInfo.Id.ToString() });
            if (onlineUsers.Count() > 0)
            {
                var user = _connectionMappingRepository.GetUserInfoById(_userInfo.Id);
                if (user != null)
                {
                    await _hubContext.Clients.AllExcept(new List<string> { user.ConnectionId }).SendNotificationFolderChange(category.ParentId);
                }
                else
                {
                    await _hubContext.Clients.All.SendNotificationFolderChange(category.ParentId);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while sending notification to users.");
        }

        return new DocumentUserPermissionDto
        {
            StatusCode = 200,
            Messages = new List<string> { "Permission Deleted Successfully." }
        };
    }
}

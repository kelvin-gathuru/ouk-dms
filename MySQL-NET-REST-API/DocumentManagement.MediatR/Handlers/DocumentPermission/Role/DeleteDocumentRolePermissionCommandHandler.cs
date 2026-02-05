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

public class DeleteDocumentRolePermissionCommandHandler : IRequestHandler<DeleteDocumentRolePermissionCommand, DocumentRolePermissionDto>
{
    private readonly IDocumentRolePermissionRepository _documentRolePermissionRepository;
    private readonly IDocumentUserPermissionRepository _documentUserPermissionRepository;
    private readonly IUnitOfWork<DocumentContext> _uow;
    private readonly UserInfoToken _userInfo;
    private readonly IDocumentAuditTrailRepository _documentAuditTrailRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IDocumentRepository _documentRepository;
    private readonly IHubContext<UserHub, IHubClient> _hubContext;
    private readonly IConnectionMappingRepository _connectionMappingRepository;
    private readonly ILogger<DeleteDocumentRolePermissionCommandHandler> _logger;
    public DeleteDocumentRolePermissionCommandHandler(
       IDocumentRolePermissionRepository documentRolePermissionRepository,
        IUnitOfWork<DocumentContext> uow,
        UserInfoToken userInfo,
          IDocumentAuditTrailRepository documentAuditTrailRepository,
          ICategoryRepository categoryRepository,
          IDocumentRepository documentRepository,
          IHubContext<UserHub, IHubClient> hubContext,
         IConnectionMappingRepository connectionMappingRepository,
         ILogger<DeleteDocumentRolePermissionCommandHandler> logger
,
         IDocumentUserPermissionRepository documentUserPermissionRepository)
    {
        _documentRolePermissionRepository = documentRolePermissionRepository;
        _uow = uow;
        _userInfo = userInfo;
        _documentAuditTrailRepository = documentAuditTrailRepository;
        _categoryRepository = categoryRepository;
        _documentRepository = documentRepository;
        _hubContext = hubContext;
        _connectionMappingRepository = connectionMappingRepository;
        _logger = logger;
        _documentUserPermissionRepository = documentUserPermissionRepository;
    }

    public async Task<DocumentRolePermissionDto> Handle(DeleteDocumentRolePermissionCommand request, CancellationToken cancellationToken)
    {
        var entity = await _documentRolePermissionRepository.FindAsync(request.Id);
        if (entity == null)
        {
            var errorDto = new DocumentRolePermissionDto
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
            AssignToRoleId = entity.RoleId
        };
        _documentAuditTrailRepository.Add(documentAudit);

        _documentRolePermissionRepository.Remove(entity);
        var documentInfo = _documentRepository.Find(entity.DocumentId);
        await _documentRepository.UpdateDocumentSharingFlagAsync(new List<Guid> { documentInfo.CategoryId });
        if (await _uow.SaveAsync() <= -1)
        {
            var errorDto = new DocumentRolePermissionDto
            {
                StatusCode = 500,
                Messages = new List<string> { "An unexpected fault happened. Try again later." }
            };
            return errorDto;
        }

        var category = _categoryRepository.All.Where(c => c.Id == documentInfo.CategoryId).FirstOrDefault();
        try
        {
            var onlineUsers = _connectionMappingRepository.GetAllUsersExceptThis(new SignlarUser { Id = _userInfo.Id.ToString() });
            if (onlineUsers.Count() > 0)
            {
                var userInfo = _connectionMappingRepository.GetUserInfoById(_userInfo.Id);
                if (userInfo != null)
                {
                    await _hubContext.Clients.AllExcept(new List<string> { userInfo.ConnectionId }).SendNotificationFolderChange(category.ParentId);
                }
                else
                {
                    await _hubContext.Clients.All.SendNotificationFolderChange(category.ParentId);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SignalR Error");
        }

        return new DocumentRolePermissionDto
        {
            StatusCode = 200,
            Messages = new List<string> { "Permission Deleted Successfully." }
        };
    }
}

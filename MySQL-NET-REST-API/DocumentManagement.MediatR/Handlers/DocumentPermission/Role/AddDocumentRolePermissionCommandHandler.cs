using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace DocumentManagement.MediatR.Handlers.DocumentPermission;

public class AddDocumentRolePermissionCommandHandler
     : IRequestHandler<AddDocumentRolePermissionCommand, DocumentRolePermissionDto>
{

    private readonly IDocumentRolePermissionRepository _documentRolePermissionRepository;
    private readonly IUserNotificationRepository _userNotificationRepository;
    private readonly IUnitOfWork<DocumentContext> _uow;
    private readonly IMapper _mapper;
    private readonly IDocumentAuditTrailRepository _documentAuditTrailRepository;
    private readonly UserInfoToken _userInfo;
    private readonly IDocumentRepository _documentRepository;
    private readonly IUserRepository _userRepository;
    private readonly ISendEmailRepository _sendEmailRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IHubContext<UserHub, IHubClient> _hubContext;
    private readonly IConnectionMappingRepository _connectionMappingRepository;
    private readonly ILogger<AddDocumentRolePermissionCommandHandler> _logger;

    public AddDocumentRolePermissionCommandHandler(
        IDocumentRolePermissionRepository documentRolePermissionRepository,
        IUnitOfWork<DocumentContext> uow,
        IUserNotificationRepository userNotificationRepository,
        IMapper mapper,
        IDocumentAuditTrailRepository documentAuditTrailRepository,
        UserInfoToken userInfo,
        IDocumentRepository documentRepository,
        IUserRepository userRepository,
        ISendEmailRepository sendEmailRepository,
        ICategoryRepository categoryRepository,
        IHubContext<UserHub, IHubClient> hubContext,
        IConnectionMappingRepository connectionMappingRepository,
        ILogger<AddDocumentRolePermissionCommandHandler> logger)
    {
        _documentRolePermissionRepository = documentRolePermissionRepository;
        _uow = uow;
        _mapper = mapper;
        _userNotificationRepository = userNotificationRepository;
        _documentAuditTrailRepository = documentAuditTrailRepository;
        _userInfo = userInfo;
        _documentRepository = documentRepository;
        _userRepository = userRepository;
        _sendEmailRepository = sendEmailRepository;
        _categoryRepository = categoryRepository;
        _hubContext = hubContext;
        _connectionMappingRepository = connectionMappingRepository;
        _logger = logger;

    }
    public async Task<DocumentRolePermissionDto> Handle(AddDocumentRolePermissionCommand request, CancellationToken cancellationToken)
    {
        var permissions = _mapper.Map<List<DocumentRolePermission>>(request.DocumentRolePermissions);
        permissions.ForEach(permission =>
        {
            var existingPermission = _documentRolePermissionRepository.All.Where(c => c.DocumentId == permission.DocumentId && c.RoleId == permission.RoleId).FirstOrDefault();
            if (existingPermission != null)
            {
                _documentRolePermissionRepository.Remove(existingPermission);
            }

            if (permission.IsTimeBound)
            {
                permission.StartDate = permission.StartDate;
                permission.EndDate = permission.EndDate.Value.AddDays(1).AddSeconds(-1);
            }
        });
        _documentRolePermissionRepository.AddRange(permissions);
        var lstSendEmail = new List<SendEmail>();

        var documentId = request.DocumentRolePermissions.FirstOrDefault().DocumentId;
        var roleIds = request.DocumentRolePermissions.Select(c => c.RoleId).Distinct().ToList();
        List<DocumentAuditTrail> lstDocumentAuditTrail = new List<DocumentAuditTrail>();
        foreach (var roleId in roleIds)
        {
            var documentAudit = new DocumentAuditTrail()
            {
                DocumentId = documentId,
                CreatedBy = _userInfo.Id,
                CreatedDate = DateTime.UtcNow,
                OperationName = DocumentOperation.Added_Permission,
                AssignToRoleId = roleId
            };
            lstDocumentAuditTrail.Add(documentAudit);
        }
        if (lstDocumentAuditTrail.Count() > 0)
        {
            _documentAuditTrailRepository.AddRange(lstDocumentAuditTrail);
        }
        var documentInfo = await _documentRepository.FindAsync(documentId.Value);
        documentInfo.IsShared = true;
        _documentRepository.Update(documentInfo);
        var users = await _userNotificationRepository.CreateRolesDocumentNotifiction(roleIds, documentInfo.Id);
        var userIds = users.Select(c => c.Id).ToList();
        if (request.DocumentRolePermissions.Any(c => c.IsAllowEmailNotification == true) && userIds.Count() > 0)
        {

            var currentUserInfo = _userRepository.Find(_userInfo.Id);

            foreach (var user in users)
            {
                _sendEmailRepository.AddSharedEmails(new SendEmail
                {
                    Email = user.Email,
                    FromEmail = currentUserInfo.Email,
                    FromName = currentUserInfo.FirstName + ' ' + currentUserInfo.LastName,
                    ToName = user.FirstName + ' ' + user.LastName,
                    CreatedBy = _userInfo.Id,
                    CreatedDate = DateTime.UtcNow,
                }, documentInfo.Name);
            }

        }
        if (lstSendEmail.Count() > 0)
        {
            _sendEmailRepository.AddRange(lstSendEmail);
        }

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

        await _userNotificationRepository.SendNotification(userIds);
        //var documentid = request.DocumentRolePermissions.FirstOrDefault().DocumentId;
        //var roleIds = request.DocumentRolePermissions.Select(c => c.RoleId).Distinct().ToList();


        return new DocumentRolePermissionDto();
    }
}

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

namespace DocumentManagement.MediatR.Handlers;

public class AddDocumentUserPermissionCommandHandler
    : IRequestHandler<AddDocumentUserPermissionCommand, DocumentUserPermissionDto>
{
    readonly IDocumentUserPermissionRepository _documentUserPermissionRepository;
    private readonly IUserNotificationRepository _userNotificationRepository;
    private readonly IUnitOfWork<DocumentContext> _uow;
    private readonly IMapper _mapper;
    private readonly IDocumentAuditTrailRepository _documentAuditTrailRepository;
    private readonly UserInfoToken _userInfo;
    private readonly IUserRepository _userRepository;
    private readonly IDocumentRepository _documentRepository;
    private readonly ISendEmailRepository _sendEmailRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IHubContext<UserHub, IHubClient> _hubContext;
    private readonly IConnectionMappingRepository _connectionMappingRepository;
    private readonly ILogger<AddDocumentUserPermissionCommandHandler> _logger;

    public AddDocumentUserPermissionCommandHandler(
        IDocumentUserPermissionRepository documentUserPermissionRepository,
        IUnitOfWork<DocumentContext> uow,
        IUserNotificationRepository userNotificationRepository,
        IMapper mapper,
           IDocumentAuditTrailRepository documentAuditTrailRepository,
        UserInfoToken userInfo,
        IUserRepository userRepository,
        IDocumentRepository documentRepository,
        ISendEmailRepository sendEmailRepository,
         ICategoryRepository categoryRepository,
          IHubContext<UserHub, IHubClient> hubContext,
         IConnectionMappingRepository connectionMappingRepository,
         ILogger<AddDocumentUserPermissionCommandHandler> logger)
    {
        _documentUserPermissionRepository = documentUserPermissionRepository;
        _uow = uow;
        _mapper = mapper;
        _userNotificationRepository = userNotificationRepository;
        _documentAuditTrailRepository = documentAuditTrailRepository;
        _userInfo = userInfo;
        _userRepository = userRepository;
        _documentRepository = documentRepository;
        _sendEmailRepository = sendEmailRepository;
        _categoryRepository = categoryRepository;
        _hubContext = hubContext;
        _connectionMappingRepository = connectionMappingRepository;
        _logger = logger;
    }
    public async Task<DocumentUserPermissionDto> Handle(AddDocumentUserPermissionCommand request, CancellationToken cancellationToken)
    {
        var permissions = _mapper.Map<List<DocumentUserPermission>>(request.DocumentUserPermissions);
        var lstSendEmail = new List<SendEmail>();
        permissions.ForEach(permission =>
        {
            var existingPermission = _documentUserPermissionRepository.All.Where(c => c.DocumentId == permission.DocumentId && c.UserId == permission.UserId).FirstOrDefault();
            if (existingPermission != null)
            {
                _documentUserPermissionRepository.Remove(existingPermission);
            }

            if (permission.IsTimeBound)
            {
                permission.StartDate = permission.StartDate;
                permission.EndDate = permission.EndDate.Value.AddDays(1).AddSeconds(-1);
            }
        });
        _documentUserPermissionRepository.AddRange(permissions);
        var userIds = request.DocumentUserPermissions.Select(c => c.UserId).ToList();
        var documentId = request.DocumentUserPermissions.FirstOrDefault().DocumentId;

        List<DocumentAuditTrail> lstDocumentAuditTrail = new List<DocumentAuditTrail>();
        foreach (var userId in userIds)
        {
            var documentAudit = new DocumentAuditTrail()
            {
                DocumentId = documentId.Value,
                CreatedBy = _userInfo.Id,
                CreatedDate = DateTime.UtcNow,
                OperationName = DocumentOperation.Added_Permission,
                AssignToUserId = userId
            };
            lstDocumentAuditTrail.Add(documentAudit);
        }
        var documentInfo = await _documentRepository.FindAsync(documentId.Value);
        documentInfo.IsShared = true;
        _documentRepository.Update(documentInfo);
        if (request.DocumentUserPermissions.Count(d => d.IsAllowEmailNotification == true) > 0)
        {
            var users = await _userRepository.GetUsersByIds(userIds);

            var currentUserInfo = await _userRepository.FindAsync(_userInfo.Id);
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


        if (lstDocumentAuditTrail.Count() > 0)
        {
            _documentAuditTrailRepository.AddRange(lstDocumentAuditTrail);
        }
        _userNotificationRepository.CreateUsersDocumentNotifiction(userIds, documentInfo.Id);
        if (await _uow.SaveAsync() <= -1)
        {
            var errorDto = new DocumentUserPermissionDto
            {
                StatusCode = 500,
                Messages = new List<string> { "An unexpected fault happened. Try again later." }
            };
            return errorDto;
        }
        await _userNotificationRepository.SendNotification(userIds);


        var category = _categoryRepository.All.Where(c => c.Id == documentInfo.CategoryId).FirstOrDefault();
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
            _logger.LogError(ex, "Singalr Error");
        }


        return new DocumentUserPermissionDto();
    }
}

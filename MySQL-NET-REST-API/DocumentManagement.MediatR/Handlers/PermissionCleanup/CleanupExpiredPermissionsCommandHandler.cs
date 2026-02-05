using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DocumentManagement.MediatR.Handlers;
public class CleanupExpiredPermissionsCommandHandler(
    IDocumentUserPermissionRepository _documentUserPermissionRepository,
    IDocumentRolePermissionRepository _documentRolePermissionRepository,
    ICategoryUserPermissionRepository _categoryUserPermissionRepository,
    ICategoryRolePermissionRepository _categoryRolePermissionRepository,
    IDocumentRepository _documentRepository,
    IDocumentAuditTrailRepository _documentAuditTrailRepository,
    ILogger<CleanupExpiredPermissionsCommandHandler> _logger,
    IUnitOfWork<DocumentContext> _uow) : IRequestHandler<CleanupExpiredPermissionsCommand, bool>
{
    public async Task<bool> Handle(CleanupExpiredPermissionsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var now = DateTime.UtcNow;
            var affectedCategoryIds = new List<Guid>();
            var expiredDocUsers = await _documentUserPermissionRepository.All
                .Include(c => c.Document)
                .Where(p => p.IsTimeBound && p.EndDate < now)
                .ToListAsync();
            if (expiredDocUsers.Count() > 0)
            {
                expiredDocUsers.ForEach(userPermission =>
                {
                    if (!affectedCategoryIds.Any(c => c == userPermission.Document.CategoryId))
                        affectedCategoryIds.Add(userPermission.Document.CategoryId);
                    _documentAuditTrailRepository.Add(new DocumentAuditTrail
                    {
                        DocumentId = userPermission.DocumentId,
                        CreatedBy = userPermission.CreatedBy,
                        CreatedDate = DateTime.UtcNow,
                        OperationName = DocumentOperation.Removed_Permission,
                        AssignToUserId = userPermission.UserId
                    });
                    userPermission.Document = null;
                });

                if (expiredDocUsers.Count() > 0)
                {
                    _documentUserPermissionRepository.RemoveRange(expiredDocUsers);
                }
            }


            var expiredDocRoles = await _documentRolePermissionRepository.All
                .Include(c => c.Document)
                .Where(p => p.IsTimeBound && p.EndDate < now)
                .ToListAsync();
            if (expiredDocRoles.Count() > 0)
            {
                expiredDocRoles.ForEach(rolePermission =>
                {
                    if (!affectedCategoryIds.Any(c => c == rolePermission.Document.CategoryId))
                        affectedCategoryIds.Add(rolePermission.Document.CategoryId);
                    _documentAuditTrailRepository.Add(new DocumentAuditTrail
                    {
                        DocumentId = rolePermission.DocumentId,
                        CreatedBy = rolePermission.CreatedBy,
                        CreatedDate = DateTime.UtcNow,
                        OperationName = DocumentOperation.Removed_Permission,
                        AssignToRoleId = rolePermission.RoleId
                    });
                    rolePermission.Document = null;
                });

                if (expiredDocRoles.Count() > 0)
                {
                    _documentRolePermissionRepository.RemoveRange(expiredDocRoles);
                }
            }




            var expiredCatUsers = await _categoryUserPermissionRepository.All
                .Where(p => p.IsTimeBound && p.EndDate < now)
                .ToListAsync();
            if (expiredCatUsers.Count() > 0)
            {
                expiredCatUsers.ForEach(categoryUserPermission =>
                {
                    if (!affectedCategoryIds.Any(c => c == categoryUserPermission.CategoryId))
                        affectedCategoryIds.Add(categoryUserPermission.CategoryId);
                    _documentAuditTrailRepository.Add(new DocumentAuditTrail
                    {
                        CategoryId = categoryUserPermission.CategoryId,
                        CreatedBy = categoryUserPermission.CreatedBy,
                        CreatedDate = DateTime.UtcNow,
                        OperationName = DocumentOperation.Removed_Folder_Permission,
                        AssignToUserId = categoryUserPermission.UserId
                    });
                });
                if (expiredCatUsers.Count() > 0)
                {
                    _categoryUserPermissionRepository.RemoveRange(expiredCatUsers);
                }
            }

            var expiredCatRoles = await _categoryRolePermissionRepository.All
                .Where(p => p.IsTimeBound && p.EndDate < now)
                .ToListAsync();

            if (expiredCatRoles.Count() > 0)
            {
                expiredCatRoles.ForEach(categoryRolePermission =>
                {
                    if (!affectedCategoryIds.Any(c => c == categoryRolePermission.CategoryId))
                        affectedCategoryIds.Add(categoryRolePermission.CategoryId);
                    _documentAuditTrailRepository.Add(new DocumentAuditTrail
                    {
                        CategoryId = categoryRolePermission.CategoryId,
                        CreatedBy = categoryRolePermission.CreatedBy,
                        CreatedDate = DateTime.UtcNow,
                        OperationName = DocumentOperation.Removed_Folder_Permission,
                        AssignToRoleId = categoryRolePermission.RoleId
                    });
                });
                if (expiredCatRoles.Count() > 0)
                {
                    _categoryRolePermissionRepository.RemoveRange(expiredCatRoles);
                }
            }
            if (await _uow.SaveAsync() <= -1)
            {
                return false;
            }
            await _documentRepository.UpdateDocumentSharingFlagAsync(affectedCategoryIds);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
        return true;
    }
}

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.MediatR.Handlers;
public class CheckDocumentPermissionCommandHandler(
    IDocumentRepository _documentRepository,
    UserInfoToken _userInfoToken,
    IUserRepository _userRepository) : IRequestHandler<CheckDocumentPermissionCommand, ServiceResponse<bool>>
{
    public async Task<ServiceResponse<bool>> Handle(CheckDocumentPermissionCommand request, CancellationToken cancellationToken)
    {
        var document = await _documentRepository.All.FirstOrDefaultAsync(c => c.Id == request.DocumentId);
        if (document == null)
        {
            return ServiceResponse<bool>.ReturnFailed(404, "Document  is not found");
        }
        var flag = true;
        if (!_userInfoToken.IsSuperAdmin)
        {

            var today = DateTime.UtcNow;
            var user = await _userRepository.AllIncluding(c => c.UserRoles).FirstOrDefaultAsync(c => c.Id == _userInfoToken.Id);
            var userRoles = user.UserRoles.Select(c => c.RoleId).ToList();

            flag = _documentRepository.All.Any(d =>
                d.Id == request.DocumentId &&
               (d.DocumentUserPermissions.Any(c => c.UserId == user.Id && (!c.IsTimeBound || (c.IsTimeBound && c.StartDate < today && c.EndDate > today)))
                || d.DocumentRolePermissions.Any(c => userRoles.Contains(c.RoleId)
                && (!c.IsTimeBound || (c.IsTimeBound && c.StartDate < today && c.EndDate > today)))
                ||
                d.Category.CategoryUserPermissions.Any(c => c.UserId == user.Id &&
                (!c.IsTimeBound || (c.StartDate < today && c.EndDate > today)))
                ||
                          d.Category.CategoryRolePermissions.Any(c => userRoles.Contains(c.RoleId) &&
                          (!c.IsTimeBound || (c.StartDate < today && c.EndDate > today))))
                                                );
        }
        return ServiceResponse<bool>.ReturnResultWith200(flag);
    }
}

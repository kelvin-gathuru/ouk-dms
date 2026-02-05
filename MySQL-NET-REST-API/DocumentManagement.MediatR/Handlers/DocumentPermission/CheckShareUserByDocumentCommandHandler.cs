using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class CheckShareUserByDocumentCommandHandler(IDocumentRepository _documentRepository, IUserRepository _userRepository, UserInfoToken _userInfo) : IRequestHandler<CheckShareUserByDocumentCommand, ServiceResponse<bool>>
    {
        public async Task<ServiceResponse<bool>> Handle(CheckShareUserByDocumentCommand request, CancellationToken cancellationToken)
        {
            var today = DateTime.UtcNow;
            var user = await _userRepository.AllIncluding(c => c.UserRoles).FirstOrDefaultAsync(c => c.Id == _userInfo.Id);
            var userRoles = user.UserRoles.Select(c => c.RoleId).ToList();
            var flag = await _documentRepository.AllIncluding(c => c.User, c => c.DocumentRolePermissions, c => c.DocumentUserPermissions)
                                        .AnyAsync(d => d.Id == request.DocumentId && (d.DocumentUserPermissions.Any(c => c.UserId == user.Id && (!c.IsTimeBound || (c.IsTimeBound && c.StartDate < today && c.EndDate > today)))
                                                    || d.DocumentRolePermissions.Any(c => userRoles.Contains(c.RoleId) && (!c.IsTimeBound || (c.IsTimeBound && c.StartDate < today && c.EndDate > today)))));
            return ServiceResponse<bool>.ReturnResultWith200(flag);
        }
    }
}

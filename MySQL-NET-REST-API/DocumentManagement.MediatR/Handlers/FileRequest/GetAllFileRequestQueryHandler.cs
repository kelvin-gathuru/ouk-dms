using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.MediatR.Handlers;

public class GetAllFileRequestQueryHandler(IUserRoleRepository _userRoleRepository, IFileRequestsRepository _fileRequestsRepository, IUserClaimRepository _userClaimRepository, IRoleClaimRepository _roleClaimRepository, UserInfoToken _userInfoToken) : IRequestHandler<GetAllFileRequestQuery, ServiceResponse<List<FileRequestListDataDto>>>
{
    public async Task<ServiceResponse<List<FileRequestListDataDto>>> Handle(GetAllFileRequestQuery request, CancellationToken cancellationToken)
    {
        var userClaim = _userClaimRepository.All
            .Where(c => c.ClaimType.ToLower() == "file_request_allow_to_see_file_request").Any();
        var userRoles = _userRoleRepository.All.Where(c => c.UserId == _userInfoToken.Id).Select(ur => ur.RoleId).ToList();
        var roleClaim = _roleClaimRepository.All
            .Where(c => c.ClaimType.ToLower() == "file_request_allow_to_see_file_request" && userRoles.Contains(c.RoleId)).Any();
        var query = _fileRequestsRepository.All.Include(f => f.CreatedBy).AsQueryable();

        if (!userClaim && !roleClaim)
        {
            query = query.Where(f => f.CreatedById.ToString() == _userInfoToken.Id.ToString());
        }

        var entities = await query
            .Select(c => new FileRequestListDataDto
            {
                Id = c.Id,
                Subject = c.Subject,
                Email = c.Email,
                MaxDocument = c.MaxDocument,
                SizeInMb = c.SizeInMb,
                AllowExtension = c.AllowExtension,
                FileRequestStatus = c.FileRequestStatus,
                CreatedBy = c.CreatedBy != null ? $"{c.CreatedBy.FirstName} {c.CreatedBy.LastName}" : null,
                CreatedById = c.CreatedById,
                CreatedDate = c.CreatedDate,
                LinkExpiryTime = c.LinkExpiryTime,
            })
            .OrderByDescending(c => c.CreatedDate)
            .Where(c => c.CreatedById == _userInfoToken.Id)
            .ToListAsync(cancellationToken);

        if (entities.Count == 0)
        {
            return ServiceResponse<List<FileRequestListDataDto>>.ReturnResultWith200(new List<FileRequestListDataDto>());
        }
        return ServiceResponse<List<FileRequestListDataDto>>.ReturnResultWith200(entities);
    }
}

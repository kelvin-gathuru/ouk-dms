using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetRecentActivityQueryHandler(
        ILoginAuditRepository _loginAuditRepository,
        IDocumentAuditTrailRepository _documentAuditTrailRepository,
        UserInfoToken _userInfoToken) : IRequestHandler<GetRecentActivityQuery, List<DashboardRecentActivityDto>>
    {
        public async Task<List<DashboardRecentActivityDto>> Handle(GetRecentActivityQuery request, CancellationToken cancellationToken)
        {
            var activities = new List<DashboardRecentActivityDto>();
            var currentUserEmail = _userInfoToken.Email;
            var currentUserId = _userInfoToken.Id;

            // Fetch recent login activities
            var loginActivities = await _loginAuditRepository.All
                .Where(x => x.UserName == currentUserEmail)
                .OrderByDescending(x => x.LoginTime)
                .Take(5)
                .Select(x => new DashboardRecentActivityDto
                {
                    Id = x.Id,
                    ActivityTime = x.LoginTime,
                    Description = $"Logged in from {x.RemoteIP}",
                    ActivityType = "Login",
                    UserName = x.UserName
                })
                .ToListAsync(cancellationToken);

            activities.AddRange(loginActivities);

            // Fetch recent document activities
            var documentActivities = await _documentAuditTrailRepository.All
                .Include(x => x.Document)
                .Include(x => x.CreatedByUser)
                .Where(x => x.CreatedBy == currentUserId)
                .OrderByDescending(x => x.CreatedDate)
                .Take(5)
                .Select(x => new DashboardRecentActivityDto
                {
                    Id = x.Id,
                    ActivityTime = x.CreatedDate,
                    Description = $"{x.OperationName} document '{x.Document.Name}'",
                    ActivityType = "Document",
                    UserName = x.CreatedByUser.Email
                })
                .ToListAsync(cancellationToken);

            activities.AddRange(documentActivities);

            return activities.OrderByDescending(x => x.ActivityTime).Take(10).ToList();
        }
    }
}

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DocumentManagement.Data;

namespace DocumentManagement.MediatR.Handlers;

public class GetDashboardStatsQueryHandler(
    IDocumentRepository _documentRepository,
    IWorkflowInstanceRepository _workflowInstanceRepository,
    IDocumentUserPermissionRepository _documentUserPermissionRepository,
    IWorkflowStepInstanceRepository _workflowStepInstanceRepository,
    UserInfoToken _userInfoToken) : IRequestHandler<GetDashboardStatsQuery, DashboardStatsDto>
{
    public async Task<DashboardStatsDto> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow.Date;
        var userId = _userInfoToken.Id;

        var stats = new DashboardStatsDto
        {
            TotalDocuments = await _documentRepository.All.CountAsync(cancellationToken),
            PendingReviews = await _workflowInstanceRepository.All.CountAsync(x => x.Status == WorkflowInstanceStatus.InProgress, cancellationToken),
            ExpiringSoon = await _documentRepository.All.CountAsync(x => x.RetentionDate != null && x.RetentionDate >= DateOnly.FromDateTime(today) && x.RetentionDate <= DateOnly.FromDateTime(today.AddDays(30)), cancellationToken),
            RecentUploads = await _documentRepository.All.CountAsync(x => x.CreatedDate >= today.AddDays(-7), cancellationToken),
            
            // New dynamic stats
            TotalArchived = await _documentRepository.All.CountAsync(d => d.IsArchive, cancellationToken),
            AssignedDocuments = await _documentUserPermissionRepository.All.CountAsync(p => p.UserId == userId, cancellationToken),
            AssignedWorkflows = await _workflowStepInstanceRepository.All.CountAsync(s => s.UserId == userId && s.Status == WorkflowStepInstanceStatus.InProgress, cancellationToken)
        };

        // Calculate Growth and Footers
        var lastMonth = today.AddMonths(-1);
        
        // Total Documents Growth
        var totalDocsLastMonth = await _documentRepository.All.CountAsync(x => x.CreatedDate < lastMonth, cancellationToken);
        var totalDocsThisMonth = stats.TotalDocuments - totalDocsLastMonth;
        stats.TotalDocumentsGrowth = $"{totalDocsThisMonth} Increased from last month";

        // Assigned Documents Growth
        var assignedLastMonth = await _documentUserPermissionRepository.All.CountAsync(p => p.UserId == userId && p.CreatedDate < lastMonth, cancellationToken);
        var assignedThisMonth = stats.AssignedDocuments - assignedLastMonth;
        stats.AssignedDocumentsGrowth = $"{assignedThisMonth} Increased from last month";

        // Pending Reviews Footer
        stats.PendingReviewsFooter = $"{stats.PendingReviews} Pending approval";

        // Assigned Tasks Footer
        var tasksCompleted = await _workflowStepInstanceRepository.All.CountAsync(s => s.UserId == userId && s.Status == WorkflowStepInstanceStatus.Completed, cancellationToken);
        stats.AssignedWorkflowsFooter = $"{tasksCompleted} Tasks completed";

        return stats;
    }
}

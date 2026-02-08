using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.MediatR.Handlers;

public class GetDocumentActivityQueryHandler(IDocumentRepository _documentRepository) : IRequestHandler<GetDocumentActivityQuery, List<DashboardChartDataDto>>
{
    public async Task<List<DashboardChartDataDto>> Handle(GetDocumentActivityQuery request, CancellationToken cancellationToken)
    {
        var startDate = DateTime.UtcNow.Date.AddDays(-6);
        var activityData = await _documentRepository.All
            .Where(d => d.CreatedDate >= startDate)
            .ToListAsync(cancellationToken);

        var groupedActivity = activityData
            .GroupBy(d => d.CreatedDate.Date)
            .Select(g => new
            {
                Label = g.Key.ToString("ddd"),
                Count = g.Count()
            })
            .ToList();

        // Ensure all last 7 days are represented
        var result = new List<DashboardChartDataDto>();
        for (int i = 0; i < 7; i++)
        {
            var date = startDate.AddDays(i);
            var label = date.ToString("ddd");
            var count = groupedActivity.FirstOrDefault(a => a.Label == label)?.Count ?? 0;
            result.Add(new DashboardChartDataDto { Label = label, Count = count });
        }

        return result;
    }
}

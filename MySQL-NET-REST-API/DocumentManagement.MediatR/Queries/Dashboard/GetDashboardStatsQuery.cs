using DocumentManagement.Data.Dto;
using MediatR;

namespace DocumentManagement.MediatR.Queries
{
    public class GetDashboardStatsQuery : IRequest<DashboardStatsDto>
    {
    }
}

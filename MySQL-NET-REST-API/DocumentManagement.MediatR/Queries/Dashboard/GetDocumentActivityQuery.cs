using DocumentManagement.Data.Dto;
using MediatR;
using System.Collections.Generic;

namespace DocumentManagement.MediatR.Queries
{
    public class GetDocumentActivityQuery : IRequest<List<DashboardChartDataDto>>
    {
    }
}

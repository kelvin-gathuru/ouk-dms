using DocumentManagement.Data.Dto;
using MediatR;
using System.Collections.Generic;

namespace DocumentManagement.MediatR.Queries
{
    public class GetDailyReminderQuery : IRequest<List<CalenderReminderDto>>
    {
        public int Month { get; set; }
        public int Year { get; set; }
    }
}

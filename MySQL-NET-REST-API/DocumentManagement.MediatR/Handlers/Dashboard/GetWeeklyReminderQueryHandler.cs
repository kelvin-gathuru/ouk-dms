using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetWeeklyReminderQueryHandler (IReminderRepository _reminderRepository, UserInfoToken _userInfo) : IRequestHandler<GetWeeklyReminderQuery, List<CalenderReminderDto>>
    {
        public async Task<List<CalenderReminderDto>> Handle(GetWeeklyReminderQuery request, CancellationToken cancellationToken)
        {
            var result = new List<CalenderReminderDto>();
            var startDate = new DateTime(request.Year, request.Month, 1, 0, 0, 1);
            var monthEndDate = startDate.AddMonths(1).AddDays(-1);
            var endDate = new DateTime(monthEndDate.Year, monthEndDate.Month, monthEndDate.Day, 23, 59, 59);
            var reminders = await _reminderRepository.All
                 .Include(c => c.ReminderUsers)
                 .Where(c => c.Frequency == Frequency.Weekly
                    && c.StartDate <= endDate && (!c.EndDate.HasValue || c.EndDate >= startDate)
                    && (c.CreatedBy == _userInfo.Id || c.ReminderUsers.Any(c => c.UserId == _userInfo.Id)))
                 .ToListAsync();

            reminders.ForEach(re =>
            {
                var reminderStartDate = startDate <= re.StartDate ? re.StartDate : startDate;
                var reminderEndDate = re.EndDate.HasValue && endDate >= re.EndDate ? re.EndDate.Value : endDate;
                var dailyReminders = Enumerable.Range(0, 1 + reminderEndDate.Subtract(reminderStartDate).Days)
                    .Where(d => reminderStartDate.AddDays(d).DayOfWeek == re.DayOfWeek)
                 .Select(offset => reminderStartDate.AddDays(offset))
                 .ToList();
                result.AddRange(dailyReminders.Select(d => new CalenderReminderDto
                {
                    RemiderId = re.Id,
                    Start = d,
                    End = d,
                    Title = re.Subject
                }));
            });
            return result;
        }
    }
}

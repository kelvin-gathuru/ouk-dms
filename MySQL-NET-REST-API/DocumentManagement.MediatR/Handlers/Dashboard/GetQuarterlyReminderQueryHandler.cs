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
    public class GetQuarterlyReminderQueryHandler (IReminderRepository _reminderRepository, UserInfoToken _userInfo) : IRequestHandler<GetQuarterlyReminderQuery, List<CalenderReminderDto>>
    {
        public async Task<List<CalenderReminderDto>> Handle(GetQuarterlyReminderQuery request, CancellationToken cancellationToken)
        {
            var startDate = new DateTime(request.Year, request.Month, 1, 0, 0, 1);
            var currentQuater = GetCurrentQuater(startDate);
            var monthEndDate = startDate.AddMonths(1).AddDays(-1);
            var endDate = new DateTime(monthEndDate.Year, monthEndDate.Month, monthEndDate.Day, 23, 59, 59);
            var lastDayOfMonth = endDate.Day;
            var reminders = await _reminderRepository.All
                 .Include(c => c.ReminderUsers)
                 .Include(c => c.QuarterlyReminders.Where(c => c.Quarter == currentQuater))
                 .Where(c => c.Frequency == Frequency.Quarterly
                     && c.QuarterlyReminders.Any(c => c.Month == request.Month)
                     && c.StartDate <= endDate && (!c.EndDate.HasValue || c.EndDate >= startDate)
                     && (c.CreatedBy == _userInfo.Id || c.ReminderUsers.Any(c => c.UserId == _userInfo.Id)))
                 .ToListAsync();

            var reminderDto = reminders.Select(c =>
            {
                var quater = c.QuarterlyReminders.FirstOrDefault();
                return new CalenderReminderDto
                {
                    RemiderId = c.Id,
                    Title = c.Subject,
                    Start = new DateTime(startDate.Year, startDate.Month, quater.Day),
                    End = new DateTime(startDate.Year, startDate.Month, quater.Day),
                };
            }).ToList();
            return reminderDto;
        }


        private QuarterEnum GetCurrentQuater(DateTime date)
        {
            if (date >= new DateTime(date.Year, 1, 1) && date <= new DateTime(date.Year, 3, 31))
            {
                return QuarterEnum.Quarter1;
            }
            else if (date >= new DateTime(date.Year, 4, 1) && date <= new DateTime(date.Year, 6, 30))
            {
                return QuarterEnum.Quarter2;
            }
            else if (date >= new DateTime(date.Year, 7, 1) && date <= new DateTime(date.Year, 9, 30))
            {
                return QuarterEnum.Quarter3;
            }
            else
            {
                return QuarterEnum.Quarter4;
            }
        }
    }

}

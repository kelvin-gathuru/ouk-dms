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
    public class GetOneTimeReminderQueryHandler (IReminderRepository _reminderRepository, UserInfoToken _userInfo) : IRequestHandler<GetOneTimeReminderQuery, List<CalenderReminderDto>>
    {
        public async Task<List<CalenderReminderDto>> Handle(GetOneTimeReminderQuery request, CancellationToken cancellationToken)
        {
            var startDate = new DateTime(request.Year, request.Month, 1, 0, 0, 1);
            var reminders = await _reminderRepository.All
                 .Include(c => c.ReminderUsers)
                 .Where(c => c.Frequency == Frequency.OneTime
                    && c.StartDate.Month == request.Month
                    && (c.CreatedBy == _userInfo.Id || c.ReminderUsers.Any(c => c.UserId == _userInfo.Id)))
                 .ToListAsync();

            var reminderDto = reminders.Select(c => new CalenderReminderDto
            {
                RemiderId = c.Id,
                Title = c.Subject,
                Start = c.StartDate,
                End = c.StartDate,
            }).ToList();

            return reminderDto;
        }
    }
}

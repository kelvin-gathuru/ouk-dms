using DocumentManagement.Data;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class WeeklyReminderServicesQueryHandler : IRequestHandler<WeeklyReminderServicesQuery, bool>
    {
        private readonly IReminderRepository _reminderRepository;
        private readonly IReminderSchedulerRepository _reminderSchedulerRepository;

        public WeeklyReminderServicesQueryHandler(IReminderRepository reminderRepository,
            IReminderSchedulerRepository reminderSchedulerRepository
            )
        {
            _reminderRepository = reminderRepository;
            _reminderSchedulerRepository = reminderSchedulerRepository;
        }
        public async Task<bool> Handle(WeeklyReminderServicesQuery request, CancellationToken cancellationToken)
        {
            var dayOfWeek = DateTime.UtcNow.DayOfWeek;
            var currentDate = DateTime.UtcNow;
            var tests= await _reminderRepository.All.ToListAsync();
            var reminders = await _reminderRepository.All
                 .Include(c => c.ReminderUsers)
                 .Where(c => c.Frequency == Frequency.Weekly && c.IsRepeated
               && c.DayOfWeek == dayOfWeek && c.StartDate <= currentDate && (!c.EndDate.HasValue || c.EndDate >= currentDate)
            )
            .ToListAsync();

            if (reminders.Count() > 0)
            {
                return await _reminderSchedulerRepository.AddMultiReminder(reminders);
            }
            return true;
        }
    }
}

using DocumentManagement.Data;
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
    public class MonthlyReminderServicesQueryHandler : IRequestHandler<MonthlyReminderServicesQuery, bool>
    {
        private readonly IReminderRepository _reminderRepository;
        private readonly IReminderSchedulerRepository _reminderSchedulerRepository;

        public MonthlyReminderServicesQueryHandler(IReminderRepository reminderRepository,
            IReminderSchedulerRepository reminderSchedulerRepository
           )
        {
            _reminderRepository = reminderRepository;
            _reminderSchedulerRepository = reminderSchedulerRepository;
        }
        public async Task<bool> Handle(MonthlyReminderServicesQuery request, CancellationToken cancellationToken)
        {
            List<Reminder> reminders = new();
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second).ToUniversalTime();

            var lastDayOfMonth = LastDayOfMonth(currentDate);
            if (lastDayOfMonth == 28)
            {
                reminders = await _reminderRepository.All
                       .Include(c => c.ReminderUsers)
                       .Where(c => c.Frequency == Frequency.Monthly
                && c.StartDate <= currentDate && (!c.EndDate.HasValue || c.EndDate >= currentDate)
                && c.StartDate.Day == currentDate.Day && c.StartDate.Day == 29 && c.StartDate.Day == 30 && c.StartDate.Day == 31)
              .ToListAsync();
            }
            else if (lastDayOfMonth == 29)
            {
                reminders = await _reminderRepository.All
                     .Include(c => c.ReminderUsers)
                     .Where(c => c.Frequency == Frequency.Monthly
                            && c.StartDate <= currentDate && (!c.EndDate.HasValue || c.EndDate >= currentDate)
                            && c.StartDate.Day == currentDate.Day && c.StartDate.Day == 30 && c.StartDate.Day == 31)
                            .ToListAsync();
            }
            else if (lastDayOfMonth == 30)
            {
                reminders = await _reminderRepository.All
                     .Include(c => c.ReminderUsers)
                     .Where(c => c.Frequency == Frequency.Monthly
                            && c.StartDate <= currentDate && (!c.EndDate.HasValue || c.EndDate >= currentDate)
                            && c.StartDate.Day == currentDate.Day && c.StartDate.Day == 31)
                            .ToListAsync();
            }
            else
            {
                reminders = await _reminderRepository.All
                     .Include(c => c.ReminderUsers)
                     .Where(c => c.Frequency == Frequency.Monthly
                            && c.StartDate <= currentDate && (!c.EndDate.HasValue || c.EndDate >= currentDate)
                            && c.StartDate.Day == currentDate.Day)
                            .ToListAsync();


            }

            if (reminders != null && reminders.Count() > 0)
            {
                return await _reminderSchedulerRepository.AddMultiReminder(reminders);
            }
            return true;
        }
        private int LastDayOfMonth(DateTime dt)
        {
            DateTime ss = new DateTime(dt.Year, dt.Month, 1);
            return ss.AddMonths(1).AddDays(-1).Day;
        }
    }
}

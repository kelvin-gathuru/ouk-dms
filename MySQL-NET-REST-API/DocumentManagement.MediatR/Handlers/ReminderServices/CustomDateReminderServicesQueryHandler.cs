using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Data;

namespace DocumentManagement.MediatR.Handlers
{
    public class CustomDateReminderServicesQueryHandler : IRequestHandler<CustomDateReminderServicesQuery, bool>
    {
        private readonly IReminderRepository _reminderRepository;
        private readonly IReminderSchedulerRepository _reminderSchedulerRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;

        public CustomDateReminderServicesQueryHandler(IReminderRepository reminderRepository,
            IReminderSchedulerRepository reminderSchedulerRepository,
            IUnitOfWork<DocumentContext> uow)
        {
            _reminderRepository = reminderRepository;
            _reminderSchedulerRepository = reminderSchedulerRepository;
            _uow = uow;
        }

        public async Task<bool> Handle(CustomDateReminderServicesQuery request, CancellationToken cancellationToken)
        {
            var toDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0).ToUniversalTime();
            var fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59).ToUniversalTime();

            var reminders = await _reminderRepository
                .All
                .Include(c => c.ReminderUsers)
                .Where(c => c.Frequency == Frequency.OneTime && !c.IsRepeated
                        && c.StartDate >= toDate
                        && c.StartDate <= fromDate)
                .ToListAsync();
            if (reminders.Count() > 0)
            {
                return await _reminderSchedulerRepository.AddMultiReminder(reminders);
            }
            return true;
        }
    }
}

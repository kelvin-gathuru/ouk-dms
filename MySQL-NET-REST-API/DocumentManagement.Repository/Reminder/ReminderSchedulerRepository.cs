using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Resources;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagement.Repository
{
    public class ReminderSchedulerRepository : GenericRepository<ReminderScheduler, DocumentContext>, IReminderSchedulerRepository
    {
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly UserInfoToken _userInfoToken;
        public ReminderSchedulerRepository(
            IUnitOfWork<DocumentContext> uow,
             IPropertyMappingService propertyMappingService,
               UserInfoToken userInfoToken
            ) : base(uow)
        {
            _uow = uow;
            _propertyMappingService = propertyMappingService;
            _userInfoToken = userInfoToken;
        }

        public async Task<bool> AddMultiReminder(List<Reminder> reminders)
        {
            if (reminders.Count() > 0)
            {
                var currentDate = DateTime.UtcNow;
                List<ReminderScheduler> lstReminderScheduler = new();
                foreach (var reminder in reminders)
                {
                    foreach (var reminderUser in reminder.ReminderUsers)
                    {
                        var reminderScheduler = new ReminderScheduler
                        {
                            Frequency = reminder.Frequency,
                            CreatedDate = DateTime.UtcNow,
                            IsActive = true,
                            Duration = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, reminder.StartDate.Hour, reminder.StartDate.Minute, reminder.StartDate.Second),
                            UserId = reminderUser.UserId,
                            IsEmailNotification = reminder.IsEmailNotification,
                            IsRead = false,
                            Subject = reminder.Subject,
                            Message = reminder.Message,
                            DocumentId = reminder.DocumentId
                        };
                        lstReminderScheduler.Add(reminderScheduler);
                    }
                }
                AddRange(lstReminderScheduler);
                if (await _uow.SaveAsync() <= -1)
                {
                    return false;
                }
            }
            return true;
        }


        public async Task<PagedList<ReminderScheduler>> GetReminders(ReminderResource reminderResource)
        {
            var collectionBeforePaging = All;
            if (reminderResource.OrderBy.ToLower() != "createddate desc" && reminderResource.OrderBy.ToLower() != "createddate asc")
            {
                collectionBeforePaging =
               collectionBeforePaging.ApplySort(reminderResource.OrderBy,
               _propertyMappingService.GetPropertyMapping<ReminderSchedulerDto, ReminderScheduler>());
            }
            else
            {
                collectionBeforePaging = collectionBeforePaging.OrderBy(c => c.CreatedDate)
                                         .ThenBy(c => c.IsRead);
            }

            collectionBeforePaging = collectionBeforePaging
                 .Where(c => c.UserId == _userInfoToken.Id);

            if (!string.IsNullOrWhiteSpace(reminderResource.Subject))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(c => EF.Functions.Like(c.Subject, $"%{reminderResource.Subject}%"));
            }

            if (!string.IsNullOrWhiteSpace(reminderResource.Message))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(c => EF.Functions.Like(c.Message, $"%{reminderResource.Message}%"));
            }

            return await PagedList<ReminderScheduler>.Create(
                collectionBeforePaging,
                reminderResource.Skip,
                reminderResource.PageSize
                );
        }
        public async Task<bool> MarkAsRead()
        {
            await _uow.Context.Database.ExecuteSqlInterpolatedAsync($"Update ReminderSchedulers SET IsRead=1 where UserId={_userInfoToken.Id};");
            return true;
        }
    }
}

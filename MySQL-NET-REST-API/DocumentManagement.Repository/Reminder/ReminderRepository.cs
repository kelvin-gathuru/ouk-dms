using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Resources;
using DocumentManagement.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagement.Repository
{
    public class ReminderRepository : GenericRepository<Reminder, DocumentContext>,
        IReminderRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly UserInfoToken _userInfo;

        public ReminderRepository(
            IUnitOfWork<DocumentContext> uow,
            IPropertyMappingService propertyMappingService,
            UserInfoToken userInfo
            ) : base(uow)
        {
            _propertyMappingService = propertyMappingService;
            _userInfo = userInfo;
        }

        public async Task<ReminderList> GetReminders(ReminderResource reminderResource)
        {
            var collectionBeforePaging = All;
            collectionBeforePaging =
               collectionBeforePaging.ApplySort(reminderResource.OrderBy,
               _propertyMappingService.GetPropertyMapping<ReminderDto, Reminder>());

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

            if (reminderResource.Frequency.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(c => c.Frequency == reminderResource.Frequency);
            }

            var reminders = new ReminderList();
            return await reminders.Create(
                collectionBeforePaging,
                reminderResource.Skip,
                reminderResource.PageSize
                );
        }

        public async Task<ReminderList> GetRemindersForLoginUser(ReminderResource reminderResource)
        {
            var collectionBeforePaging = All;
            collectionBeforePaging =
               collectionBeforePaging.ApplySort(reminderResource.OrderBy,
               _propertyMappingService.GetPropertyMapping<ReminderDto, Reminder>());

            collectionBeforePaging = collectionBeforePaging
                  .Where(c => c.ReminderUsers.Any(d => d.UserId == _userInfo.Id));

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

            if (reminderResource.Frequency.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(c => c.Frequency == reminderResource.Frequency);
            }

            var reminders = new ReminderList();
            return await reminders.Create(
                collectionBeforePaging,
                reminderResource.Skip,
                reminderResource.PageSize
                );
        }
    }
}

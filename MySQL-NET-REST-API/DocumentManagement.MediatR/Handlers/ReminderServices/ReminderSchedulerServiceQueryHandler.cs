using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class ReminderSchedulerServiceQueryHandler : IRequestHandler<ReminderSchedulerServiceQuery, bool>
    {
        private readonly IReminderSchedulerRepository _reminderSchedulerRepository;
        private readonly IEmailSMTPSettingRepository _emailSMTPSettingRepository;
        private readonly ILogger<ReminderSchedulerServiceQueryHandler> _logger;
        private readonly PathHelper _pathHelper;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly IUserNotificationRepository _userNotificationRepository;
        private readonly EmailHelper _emailHelper;


        public ReminderSchedulerServiceQueryHandler(IReminderRepository reminderRepository,
            IReminderSchedulerRepository reminderSchedulerRepository,
            IEmailSMTPSettingRepository emailSMTPSettingRepository,
            IUnitOfWork<DocumentContext> uow,
             ILogger<ReminderSchedulerServiceQueryHandler> logger,
             PathHelper pathHelper,
             IUserNotificationRepository userNotificationRepository,
             EmailHelper emailHelper
            )
        {
            _reminderSchedulerRepository = reminderSchedulerRepository;
            _emailSMTPSettingRepository = emailSMTPSettingRepository;
            _logger = logger;
            _pathHelper = pathHelper;
            _uow = uow;
            _userNotificationRepository = userNotificationRepository;
            _emailHelper = emailHelper;
        }

        public async Task<bool> Handle(ReminderSchedulerServiceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second).ToUniversalTime();

                var reminderSchedulers = await _reminderSchedulerRepository.AllIncluding(c => c.User)
                                                .AsNoTracking()
                                                .OrderBy(c => c.Duration)
                                                .Where(c => c.IsActive && c.Duration <= currentDate)
                                                .Take(10)
                                                .ToListAsync();

                if (reminderSchedulers.Count() > 0)
                {
                    var defaultSmtp = await _emailSMTPSettingRepository.FindBy(c => c.IsDefault).FirstOrDefaultAsync();

                    foreach (var reminderScheduler in reminderSchedulers)
                    {
                        var trackedUser = _uow.Context.Users.Local.FirstOrDefault(c => c.Id == reminderScheduler.User.Id);
                        if (trackedUser != null)
                        {
                            reminderScheduler.User = trackedUser;
                        }
                        else
                        {
                            _uow.Context.Attach(reminderScheduler.User);
                        }
                        _userNotificationRepository.AddUserNotificationByReminderScheduler(reminderScheduler);
                        if (reminderScheduler.IsEmailNotification && defaultSmtp != null)
                        {
                            await _emailHelper.SendEmail(new SendEmailSpecification
                            {
                                UserName = defaultSmtp.UserName,
                                Password = defaultSmtp.Password,
                                FromAddress = defaultSmtp.FromEmail,
                                ToAddress = reminderScheduler.User.Email,
                                Body = reminderScheduler.Message,
                                Host = defaultSmtp.Host,
                                Port = defaultSmtp.Port,
                                Subject = reminderScheduler.Subject,
                                CCAddress = "",
                                EncryptionType = defaultSmtp.EncryptionType,
                                FromName = defaultSmtp.FromName,
                                ToName = $"{reminderScheduler.User.FirstName} {reminderScheduler.User.LastName}",
                                Attechments = new List<FileInfo>()
                            });
                        }
                        reminderScheduler.IsActive = false;
                    }
                    _reminderSchedulerRepository.UpdateRange(reminderSchedulers);
                    if (await _uow.SaveAsync() <= -1)
                    {
                        return false;
                    }
                    var userIds = reminderSchedulers.Select(c => c.UserId).Distinct().ToList();
                    await _userNotificationRepository.SendNotification(userIds);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            return true;
        }
    }
}

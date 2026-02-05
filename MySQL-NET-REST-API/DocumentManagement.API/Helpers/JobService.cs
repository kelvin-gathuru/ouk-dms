using System;
using System.Threading.Tasks;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Handlers;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using Hangfire;
using MediatR;

namespace DocumentManagement.API.Helpers;

public class JobService
{
    public IMediator _mediator { get; set; }
    private readonly IConnectionMappingRepository _connectionMappingRepository;

    public JobService(IMediator mediator, IConnectionMappingRepository connectionMappingRepository)
    {
        _mediator = mediator;
        _connectionMappingRepository = connectionMappingRepository;
    }

    public void StartScheduler()
    {
        // * * * * *
        // 1 2 3 4 5

        // field #   meaning        allowed values
        // -------   ------------   --------------
        //    1      minute         0-59
        //    2      hour           0-23
        //    3      day of month   1-31
        //    4      month          1-12 (or use names)
        //    5      day of week    0-7 (0 or 7 is Sun, or use names)


        // Reminder Jobs
        RecurringJob.AddOrUpdate<JobService>(
            "DailyReminder",
            job => job.DailyReminder(),
            Cron.Daily(0, 5),
            new RecurringJobOptions { TimeZone = TimeZoneInfo.Local }
        );

        RecurringJob.AddOrUpdate<JobService>(
            "WeeklyReminder",
            job => job.WeeklyReminder(),
            Cron.Daily(0, 10),
            new RecurringJobOptions { TimeZone = TimeZoneInfo.Local }
        );

        RecurringJob.AddOrUpdate<JobService>(
            "MonthlyReminder",
            job => job.MonthyReminder(),
            Cron.Daily(0, 20),
            new RecurringJobOptions { TimeZone = TimeZoneInfo.Local }
        );

        RecurringJob.AddOrUpdate<JobService>(
            "QuarterlyReminder",
            job => job.QuarterlyReminder(),
            Cron.Daily(0, 30),
            new RecurringJobOptions { TimeZone = TimeZoneInfo.Local }
        );

        RecurringJob.AddOrUpdate<JobService>(
            "HalfYearlyReminder",
            job => job.HalfYearlyReminder(),
            Cron.Daily(0, 40),
            new RecurringJobOptions { TimeZone = TimeZoneInfo.Local }
        );

        RecurringJob.AddOrUpdate<JobService>(
            "YearlyReminder",
            job => job.YearlyReminder(),
            Cron.Daily(0, 50),
            new RecurringJobOptions { TimeZone = TimeZoneInfo.Local }
        );

        RecurringJob.AddOrUpdate<JobService>(
            "CustomDateReminder",
            job => job.CustomDateReminderSchedule(),
            Cron.Daily(0, 59),
            new RecurringJobOptions { TimeZone = TimeZoneInfo.Local }
        );

        // Frequent jobs
        RecurringJob.AddOrUpdate<JobService>(
            "ReminderSchedule",
            job => job.ReminderSchedule(),
            "*/10 * * * *", // every 10 minutes
            new RecurringJobOptions { TimeZone = TimeZoneInfo.Local }
        );

        RecurringJob.AddOrUpdate<JobService>(
            "SendEmailScheduler",
            job => job.SendEmailScheduler(),
            "*/15 * * * *", // every 15 minutes
            new RecurringJobOptions { TimeZone = TimeZoneInfo.Local }
        );

        RecurringJob.AddOrUpdate<JobService>(
            "DocumentIndexing",
            job => job.DocumentIndexing(),
            "*/15 * * * *", // every 15 minutes
            new RecurringJobOptions { TimeZone = TimeZoneInfo.Local }
        );

        RecurringJob.AddOrUpdate<JobService>(
            "SendEmailReminderForWorkflowTransition",
            job => job.SendEmailReminderForWorkflowTransition(),
            "*/3 * * * *", // every 3 minutes
            new RecurringJobOptions { TimeZone = TimeZoneInfo.Local }
        );

        // Cleanup / Archive
        RecurringJob.AddOrUpdate<JobService>(
            "PermissionCleanup",
            job => job.PermissionCleanupSchedule(),
            Cron.Daily(0, 59),
            new RecurringJobOptions { TimeZone = TimeZoneInfo.Local }
        );

        RecurringJob.AddOrUpdate<JobService>(
            "ArchiveRetentionDocumentToDelete",
            job => job.ArchiveRetentionDocumentToDelete(),
            Cron.Daily(1, 5),
            new RecurringJobOptions { TimeZone = TimeZoneInfo.Local }
        );

        RecurringJob.AddOrUpdate<JobService>(
            "ArchieveOrDeleteDocummentBaseOnConfigurationOnDocumentOnAction",
            job => job.ArchieveOrDeleteDocummentBaseOnConfigurationOnDocumentOnAction(),
            Cron.Daily(2, 5),
            new RecurringJobOptions { TimeZone = TimeZoneInfo.Local }
        );

        // Hangfire cleanup
        //RecurringJob.AddOrUpdate<HangfireCleanupService>(
        //    "hangfire-cleanup",
        //    x => x.CleanupOldJobs(),
        //    Cron.Daily,
        //    new RecurringJobOptions { TimeZone = TimeZoneInfo.Local }
        //);
    }

    [Queue("reminder")]
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 60, 300, 900 })]
    [DisableConcurrentExecution(3600)]
    public async Task<bool> DailyReminder()
    {
        return await _mediator.Send(new DailyReminderServicesQuery());
    }
    [Queue("reminder")]
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 60, 300, 900 })]
    [DisableConcurrentExecution(3600)]
    public async Task<bool> WeeklyReminder()
    {
        return await _mediator.Send(new WeeklyReminderServicesQuery());
    }
    [Queue("reminder")]
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 60, 300, 900 })]
    [DisableConcurrentExecution(3600)]
    public async Task<bool> MonthyReminder()
    {
        return await _mediator.Send(new MonthlyReminderServicesQuery());
    }
    [Queue("reminder")]
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 60, 300, 900 })]
    [DisableConcurrentExecution(3600)]
    public async Task<bool> QuarterlyReminder()
    {
        return await _mediator.Send(new QuarterlyReminderServiceQuery());
    }
    [Queue("reminder")]
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 60, 300, 900 })]
    [DisableConcurrentExecution(3600)]
    public async Task<bool> HalfYearlyReminder()
    {
        return await _mediator.Send(new HalfYearlyReminderServiceQuery());
    }
    [Queue("reminder")]
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 60, 300, 900 })]
    [DisableConcurrentExecution(3600)]
    public async Task<bool> YearlyReminder()
    {
        return await _mediator.Send(new YearlyReminderServicesQuery());
    }
    [Queue("reminder")]
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 60, 300, 900 })]
    [DisableConcurrentExecution(3600)]
    public async Task<bool> ReminderSchedule()
    {
        return await _mediator.Send(new ReminderSchedulerServiceQuery());
    }
    [Queue("reminder")]
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 60, 300, 900 })]
    [DisableConcurrentExecution(3600)]
    public async Task<bool> CustomDateReminderSchedule()
    {
        return await _mediator.Send(new CustomDateReminderServicesQuery());
    }
    [Queue("cleanup")]
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 60, 300, 900 })]
    [DisableConcurrentExecution(3600)]
    public async Task<bool> PermissionCleanupSchedule()
    {
        return await _mediator.Send(new CleanupExpiredPermissionsCommand());
    }
    [Queue("emailnotification")]
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 60, 300, 900 })]
    [DisableConcurrentExecution(3600)]
    public async Task<bool> SendEmailScheduler()
    {

        return await _mediator.Send(new SendEmailSchedulerCommand());

    }
    [Queue("workflow")]
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 60, 300, 900 })]
    [DisableConcurrentExecution(3600)]
    public async Task<bool> SendEmailReminderForWorkflowTransition()
    {
        return await _mediator.Send(new SendEmailReminderForWorkflowTransitionCommand());
    }
    [Queue("documentindex")]
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 60, 300, 900 })]
    [DisableConcurrentExecution(3600)]
    public async Task<bool> DocumentIndexing()
    {

        return await _mediator.Send(new AddDocumentIndexContentCommand());

    }
    [Queue("archievedocument")]
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 60, 300, 900 })]
    [DisableConcurrentExecution(3600)]
    public async Task<bool> ArchiveRetentionDocumentToDelete()
    {
        return await _mediator.Send(new ArchiveRetentionDocumentBackgroundServiceCommand());
    }
    [Queue("archievedocument")]
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 60, 300, 900 })]
    [DisableConcurrentExecution(3600)]
    public async Task<bool> ArchieveOrDeleteDocummentBaseOnConfigurationOnDocumentOnAction()
    {
        return await _mediator.Send(new RunArchiveRetensionOnDocumentActionConfigurationCommand());
    }


}

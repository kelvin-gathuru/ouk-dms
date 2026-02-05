using AutoMapper;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.CommandAndQuery;

namespace DocumentManagement.API.Helpers.Mapping
{
    public class ReminderProfile : Profile
    {
        public ReminderProfile()
        {
            CreateMap<Reminder, ReminderDto>().ReverseMap();
            CreateMap<AddReminderCommand, Reminder>();
            CreateMap<UpdateReminderCommand, Reminder>();
            CreateMap<ReminderNotification, ReminderNotificationDto>().ReverseMap();
            CreateMap<ReminderUser, ReminderUserDto>().ReverseMap();
            CreateMap<DailyReminder, DailyReminderDto>().ReverseMap();
            CreateMap<QuarterlyReminder, QuarterlyReminderDto>().ReverseMap();
            CreateMap<HalfYearlyReminder, HalfYearlyReminderDto>().ReverseMap();
            CreateMap<ReminderScheduler, ReminderSchedulerDto>().ReverseMap();
        }
    }
}

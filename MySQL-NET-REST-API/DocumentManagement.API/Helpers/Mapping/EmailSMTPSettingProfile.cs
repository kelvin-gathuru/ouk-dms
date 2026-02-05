using AutoMapper;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;
namespace DocumentManagement.API.Helpers.Mapping
{
    public class EmailSMTPSettingProfile : Profile
    {
        public EmailSMTPSettingProfile()
        {
            CreateMap<AddEmailSMTPSettingCommand, EmailSMTPSetting>();

            CreateMap<EmailSMTPSettingDto, EmailSMTPSetting>().ReverseMap();

            CreateMap<SendEmailCommand, SendEmail>();
            CreateMap<UpdateEmailSMTPSettingCommand, EmailSMTPSetting>();
        }
    }
}

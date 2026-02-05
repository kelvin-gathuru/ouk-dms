using AutoMapper;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;

namespace DocumentManagement.API.Helpers.Mapping
{
    public class UserNotificationProfile : Profile
    {
        public UserNotificationProfile()
        {
            CreateMap<UserNotification, UserNotificationDto>().ReverseMap();
        }
    }
}

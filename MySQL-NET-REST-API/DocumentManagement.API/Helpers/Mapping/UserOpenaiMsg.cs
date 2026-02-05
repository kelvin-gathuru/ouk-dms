using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;

namespace DocumentManagement.API.Helpers.Mapping;

public class UserOpenaiMsgProfile : Profile
{
    public UserOpenaiMsgProfile()
    {
        CreateMap<UserOpenaiMsg, UserOpenaiMsgDto>().ReverseMap();
    }
}

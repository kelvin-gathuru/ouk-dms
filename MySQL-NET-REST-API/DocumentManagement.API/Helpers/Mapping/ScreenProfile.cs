using AutoMapper;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;

namespace DocumentManagement.API.Helpers.Mapping
{
    public class ScreenProfile : Profile
    {
        public ScreenProfile()
        {
            CreateMap<Screen, ScreenDto>().ReverseMap();
            CreateMap<AddScreenCommand, Screen>();
            CreateMap<UpdateScreenCommand, Screen>();
        }
    }
}

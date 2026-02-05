using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data;
using DocumentManagement.MediatR.Commands;

namespace DocumentManagement.API.Helpers.Mapping
{
    public class PageHelperProfile : Profile
    {
        public PageHelperProfile()
        {
            CreateMap<PageHelperDto, PageHelper>().ReverseMap();
            CreateMap<CreatePageHelperCommand, PageHelper>();
            CreateMap<UpdatePageHelperCommand, PageHelper>();
        }
    }
}

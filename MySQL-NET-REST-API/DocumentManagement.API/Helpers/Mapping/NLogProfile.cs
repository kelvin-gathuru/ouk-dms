using AutoMapper;
using DocumentManagement.Data.Dto;

namespace DocumentManagement.API.Helpers.Mapping
{
    public class NLogProfile : Profile
    {
        public NLogProfile()
        {
            CreateMap<Data.Entities.NLog, NLogDto>().ReverseMap();
        }
    }
}

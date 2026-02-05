using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.MediatR.Commands;

namespace DocumentManagement.API.Helpers.Mapping
{
    public class MatTableSettingProfile: Profile
    {
        public MatTableSettingProfile()
        {
            CreateMap<MatTableSetting, MatTableSettingDto>().ReverseMap();
            CreateMap<AddOrUpdateTableSettingCommand, MatTableSetting>().ReverseMap();
            
        }
    }
}

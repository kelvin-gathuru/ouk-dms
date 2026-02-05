using AutoMapper;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;

namespace DocumentManagement.API.Helpers.Mapping
{
    public class StorageSettingProfile : Profile
    {
        public StorageSettingProfile()
        {
            CreateMap<StorageSetting, StorageSettingDto<string>>().ReverseMap();
            CreateMap<AddStorageSettingCommand, StorageSetting>();
            CreateMap<UpdateStorageSettingCommand, StorageSetting>();
        }
    }
}

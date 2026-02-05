using AutoMapper;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;

namespace DocumentManagement.API.Helpers.Mapping
{
    public class AllowFileExtensionProfile : Profile
    {
        public AllowFileExtensionProfile()
        {
            CreateMap<AllowFileExtension,AllowFileExtensionDto>().ReverseMap();
            CreateMap<AddAllowFileExtensionCommand, AllowFileExtension>();
            CreateMap<UpdateAllowFileExtensionCommand, AllowFileExtension>();
        }
    }
}

using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.MediatR.Commands;

namespace DocumentManagement.API.Helpers.Mapping
{
    public class CompanyProfileProfile : Profile
    {
        public CompanyProfileProfile()
        {
            CreateMap<CompanyProfile, CompanyProfileDto>().ReverseMap();
            CreateMap<UpdateCompanyProfileCommand, CompanyProfile>();
        }
    }
}

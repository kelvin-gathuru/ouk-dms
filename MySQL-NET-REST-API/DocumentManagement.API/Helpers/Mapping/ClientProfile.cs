using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.MediatR.Commands;

namespace DocumentManagement.API.Helpers.Mapping
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<Client, ClientDto>().ReverseMap();
            CreateMap<AddClientCommand, Client>().ReverseMap();
            CreateMap<UpdateClientCommand, Client>().ReverseMap();
        }
    }
}
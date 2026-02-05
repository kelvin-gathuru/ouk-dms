using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;
using AutoMapper;
using DocumentManagement.Data.Entities;

namespace DocumentManagement.API.Helpers.Mapping
{
    public class DocumentStatusProfile : Profile
    {
        public DocumentStatusProfile()
        {
            CreateMap<DocumentStatus, DocumentStatusDto>().ReverseMap();
            CreateMap<AddDocumentStatusCommand, DocumentStatus>();
            CreateMap<UpdateDocumentStatusCommand, DocumentStatus>();
        }
    }
}

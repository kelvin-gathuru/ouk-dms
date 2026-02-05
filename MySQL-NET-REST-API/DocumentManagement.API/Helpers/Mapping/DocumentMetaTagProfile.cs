using AutoMapper;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;

namespace DocumentManagement.API.Helpers.Mapping;

public class DocumentMetaTagProfile : Profile
{
    public DocumentMetaTagProfile()
    {
        CreateMap<DocumentMetaTag, DocumentMetaTagDto>().ReverseMap();
        CreateMap<AddDocumentMetaTagCommand, DocumentMetaTag>().ReverseMap();
        CreateMap<UpdateDocumentMetaTagCommand, DocumentMetaTag>().ReverseMap();
    }
}

using AutoMapper;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.MediatR.Commands;

namespace DocumentManagement.API.Helpers.Mapping;

public class DocumentProfile : Profile
{
    public DocumentProfile()
    {
        CreateMap<Document, DocumentDto>().ReverseMap();
        CreateMap<AddDocumentCommand, Document>();
        CreateMap<AddClientDocumentCommand, Document>();
        CreateMap<UpdateDocumentCommand, Document>();
        CreateMap<AddDocumentToMeCommand, Document>();
        CreateMap<DocumentComment, DocumentCommentDto>().ReverseMap();
        CreateMap<DocumentShareableLink, DocumentShareableLinkDto>().ReverseMap();
        CreateMap<CreateDocumentShareableLinkCommand, DocumentShareableLink>();
        CreateMap<AddDocumentCommentCommand, DocumentComment>();

        CreateMap<DocumentVersion, DocumentVersionDto>().ReverseMap();
        CreateMap<DocumentMetaData, DocumentMetaDataDto>().ReverseMap();
        CreateMap<UploadNewDocumentVersionCommand, DocumentVersion>();
        CreateMap<DocumentChunkDto, DocumentChunk>().ReverseMap();
        CreateMap<AddDocumentChunkCommand, Document>();
        CreateMap<DocumentMetaTagDto, DocumentMetaTag>().ReverseMap();
        CreateMap<AddDocumentChunkWindowSharedCommand, Document>();
        CreateMap<AddDocumentWindowSharedCommand, Document>();
        CreateMap<AddAIDocumentCreatedCommand, Document>();
    }
}

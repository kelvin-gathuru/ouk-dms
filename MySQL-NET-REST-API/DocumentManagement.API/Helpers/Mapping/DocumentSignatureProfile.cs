using AutoMapper;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;

namespace DocumentManagement.API.Helpers.Mapping
{
    public class DocumentSignatureProfile : Profile
    {
        public DocumentSignatureProfile()
        {
            CreateMap<DocumentSignature, DocumentSignatureDto>().ReverseMap();
            CreateMap<AddDocumentSignatureCommand, DocumentSignature>();
            CreateMap<AddDocumentSignatureWithPositionCommand, DocumentSignature>();
        }
    }
}

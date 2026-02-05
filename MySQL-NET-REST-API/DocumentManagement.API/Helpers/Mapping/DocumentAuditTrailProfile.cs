using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.MediatR.Commands;

namespace DocumentManagement.API.Helpers.Mapping
{
    public class DocumentAuditTrailProfile : Profile
    {
        public DocumentAuditTrailProfile()
        {
            CreateMap<DocumentAuditTrailDto, DocumentAuditTrail>().ReverseMap();
            CreateMap<AddDocumentAuditTrailCommand, DocumentAuditTrail>();
        }
    }
}

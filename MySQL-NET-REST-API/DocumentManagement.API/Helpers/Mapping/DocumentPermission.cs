using AutoMapper;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;

namespace DocumentManagement.API.Helpers.Mapping
{
    public class DocumentPermission : Profile
    {
        public DocumentPermission()
        {
            CreateMap<DocumentUserPermission, DocumentUserPermissionDto>().ReverseMap();
            CreateMap<DocumentRolePermission, DocumentRolePermissionDto>().ReverseMap();
            CreateMap<DocumentUserPermission, DocumentPermissionDto>().ReverseMap();
            CreateMap<DocumentRolePermission, DocumentPermissionDto>().ReverseMap();
        }
    }
}

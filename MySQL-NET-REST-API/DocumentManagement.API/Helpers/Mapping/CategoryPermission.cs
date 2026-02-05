using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;

namespace DocumentManagement.API.Helpers.Mapping
{
    public class CategoryPermission: Profile
    {
        public CategoryPermission()
        {
            CreateMap<CategoryUserPermission, CategoryUserPermissionDto>().ReverseMap();
            CreateMap<CategoryRolePermission, CategoryRolePermissionDto>().ReverseMap();
            CreateMap<CategoryUserPermission, CategoryPermissionDto>().ReverseMap();
            CreateMap<CategoryRolePermission, CategoryPermissionDto>().ReverseMap();
        }
    }
}

using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.MediatR.Commands;

namespace DocumentManagement.API.Helpers.Mapping;

public class PageActionProfile : Profile
{
    /// <summary>
    /// Action related entities and DTOs.
    /// </summary>
    public PageActionProfile()
    {
        CreateMap<PageAction, PageActionDto>().ReverseMap();
        CreateMap<AddPageActionCommand, PageAction>();
        CreateMap<UpdatePageActionCommand, PageAction>();
    }
}
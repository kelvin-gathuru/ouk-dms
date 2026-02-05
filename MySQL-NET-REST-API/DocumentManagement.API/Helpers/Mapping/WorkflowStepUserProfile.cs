using DocumentManagement.Data.Dto;
using DocumentManagement.Data;
using AutoMapper;

namespace DocumentManagement.API.Helpers.Mapping
{
    public class WorkflowStepUserProfile : Profile
    {
        public WorkflowStepUserProfile()
        {
            CreateMap<WorkflowStepUserDto, WorkflowStepUser>().ReverseMap();
        }
    }
}
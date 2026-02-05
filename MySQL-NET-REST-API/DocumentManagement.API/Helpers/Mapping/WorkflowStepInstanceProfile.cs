using AutoMapper;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;

namespace DocumentManagement.API.Helpers.Mapping
{
    public class WorkflowStepInstanceProfile : Profile
    {
        public WorkflowStepInstanceProfile()
        {
            CreateMap<WorkflowStepInstanceDto, WorkflowStepInstance>().ReverseMap();
        }
    }
}
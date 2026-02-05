using AutoMapper;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;

namespace DocumentManagement.API.Helpers.Mapping
{
    public class WorkflowInstanceProfile : Profile
    {
        public WorkflowInstanceProfile()
        {
            CreateMap<WorkflowInstanceDto, WorkflowInstance>().ReverseMap();
            CreateMap<AddWorkflowInstanceCommand, WorkflowInstance>();
            CreateMap<UpdateWorkflowInstanceCommand, WorkflowInstance>();
        }
    }
}
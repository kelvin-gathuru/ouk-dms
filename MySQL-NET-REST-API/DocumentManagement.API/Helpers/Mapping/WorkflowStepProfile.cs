using AutoMapper;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;

namespace DocumentManagement.API.Helpers.Mapping
{
    public class WorkflowStepProfile : Profile
    {
        public WorkflowStepProfile()
        {
            CreateMap<WorkflowStepDto, WorkflowStep>().ReverseMap();
            CreateMap<WorkflowStepDataDto, WorkflowStep>().ReverseMap();
            CreateMap<AddWorkflowStepCommand, WorkflowStep>();
            CreateMap<UpdateWorkflowStepCommand, WorkflowStep>();
        }
    }
}
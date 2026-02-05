using DocumentManagement.Data.Dto;
using DocumentManagement.Data;
using DocumentManagement.MediatR.Commands;
using AutoMapper;

namespace DocumentManagement.API.Helpers.Mapping
{
    public class WorkflowProfile : Profile
    {
        public WorkflowProfile()
        {
            CreateMap<WorkflowDto, Workflow>().ReverseMap();
            CreateMap<AddWorkflowCommand, Workflow>();
            CreateMap<UpdateWorkflowCommand, Workflow>();
        }
    }
}

using AutoMapper;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;

namespace DocumentManagement.API.Helpers.Mapping
{
    public class WorkflowStepRoleProfile : Profile
    {
        public WorkflowStepRoleProfile()
        {
            CreateMap<WorkflowStepRoleDto, WorkflowStepRole>().ReverseMap();
        }
    }
}
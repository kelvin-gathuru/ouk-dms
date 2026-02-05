using AutoMapper;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.MediatR.Commands;

namespace DocumentManagement.API.Helpers.Mapping;

public class WorkflowTransitionProfile : Profile
{
    public WorkflowTransitionProfile()
    {
        CreateMap<WorkflowTransitionDto, WorkflowTransition>().ReverseMap();
        CreateMap<WorkflowTransitionDataDto, WorkflowTransition>().ReverseMap();
        CreateMap<AddWorkflowTransitionCommand, WorkflowTransition>();
        CreateMap<UpdateWorkflowTransitionCommand, WorkflowTransition>();
        CreateMap<WorkflowTransitionRoleDto, WorkflowTransitionRole>().ReverseMap();
        CreateMap<WorkflowTransitionUserDto, WorkflowTransitionUser>().ReverseMap();
    }
}

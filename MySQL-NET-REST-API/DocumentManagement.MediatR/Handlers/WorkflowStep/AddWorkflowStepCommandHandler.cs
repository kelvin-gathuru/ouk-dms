using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.MediatR.Handlers;

public class AddWorkflowStepCommandHandler(
    IWorkflowStepRepository _workflowStepRepository,
    IWorkflowTransitionRepository _workflowTransitionRepository,
    IWorkflowRepository _workflowRepository,
    IUnitOfWork<DocumentContext> _uow,
    IMapper _mapper,
    IHubContext<UserHub, IHubClient> hubContext,
    IConnectionMappingRepository connectionMappingRepository,
    UserInfoToken _userInfo) : IRequestHandler<AddWorkflowStepCommand, ServiceResponse<List<WorkflowStepDto>>>
{
    public async Task<ServiceResponse<List<WorkflowStepDto>>> Handle(AddWorkflowStepCommand request, CancellationToken cancellationToken)
    {
        var firstStep = request.WorkflowSteps.FirstOrDefault();
        if (firstStep == null)
        {
            return ServiceResponse<List<WorkflowStepDto>>.Return404("WorkflowSteps list is empty.");
        }

        var workflowId = firstStep.WorkflowId;

        var workflowExist = await _workflowRepository.FindBy(c => c.Id == workflowId).FirstOrDefaultAsync();
        if (workflowExist == null)
        {
            return ServiceResponse<List<WorkflowStepDto>>.Return404($"Workflow with ID {workflowId} not found.");
        }

        var entities = _mapper.Map<List<WorkflowStep>>(request.WorkflowSteps);

        var workflowStepExist = _workflowStepRepository.All
            //.Include(s => s.WorkflowStepRoles)
            //.Include(s => s.WorkflowStepUsers)
            .Where(s => s.WorkflowId == workflowId).ToList();
        if (workflowStepExist.Count > 0)
        {
            _workflowStepRepository.RemoveRange(workflowStepExist);
        }
        var workflowTransitionExist = _workflowTransitionRepository.All
            .Where(s => s.WorkflowId == workflowId).ToList();
        if (workflowTransitionExist.Count > 0)
        {
            _workflowTransitionRepository.RemoveRange(workflowTransitionExist);
        }

        //foreach (var item in entities)
        //{
        //    foreach (var existItem in workflowStepExist)
        //    {

        //        if (item.Id == existItem.Id)
        //        {
        //            _workflowStepRepository.Remove(existItem);
        //        }
        //        else if(existItem)
        //        //if (workflowStepExist.WorkflowStepUsers.Count > 0)
        //        //{
        //        //    _workflowStepUserRepository.RemoveRange(workflowStepExist.WorkflowStepUsers);
        //        //}
        //        //if (workflowStepExist.WorkflowStepRoles.Count > 0)
        //        //{
        //        //    _workflowStepRoleRepository.RemoveRange(workflowStepExist.WorkflowStepRoles);
        //        //}
        //        //_workflowStepRepository.Remove(workflowStepExist);          
        //    } 

        //}
        //var workflowStepRoles = new List<WorkflowStepRole>();
        //var workflowStepUsers = new List<WorkflowStepUser>();
        for (var i = 0; i < entities.Count; i++)
        {
            var id = Guid.NewGuid();
            entities[i].Id = id;
            entities[i].CreatedAt = DateTime.UtcNow;
            entities[i].UpdatedAt = DateTime.UtcNow;
            //foreach (var item in request.WorkflowSteps[i].RoleIds)
            //{
            //    var workflowStepRole = new WorkflowStepRole();
            //    workflowStepRole.WorkflowStepId = id;
            //    workflowStepRole.RoleId = item;
            //    workflowStepRoles.Add(workflowStepRole);
            //}
            //foreach (var item in request.WorkflowSteps[i].UserIds)
            //{
            //    var workflowStepUser = new WorkflowStepUser();
            //    workflowStepUser.WorkflowStepId = id;
            //    workflowStepUser.UserId = item;
            //    workflowStepUsers.Add(workflowStepUser);
            //}
        }

        _workflowStepRepository.AddRange(entities);
        //_workflowStepUserRepository.AddRange(workflowStepUsers);
        //_workflowStepRoleRepository.AddRange(workflowStepRoles);

        if (await _uow.SaveAsync() <= 0)
        {
            return ServiceResponse<List<WorkflowStepDto>>.Return500();
        }
        try
        {
            var user = connectionMappingRepository.GetUserInfoById(_userInfo.Id);

            if (user != null)
            {
                await hubContext.Clients.AllExcept(new List<string> { user.ConnectionId }).RefreshWorkflowSettings();
            }
        }
        catch (Exception)
        {

        }
        var entityDto = _mapper.Map<List<WorkflowStepDto>>(entities);
        entityDto.OrderBy(c => c.OrderNo);
        return ServiceResponse<List<WorkflowStepDto>>.ReturnResultWith201(entityDto);
    }
}
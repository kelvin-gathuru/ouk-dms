using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.MediatR.Handlers;

public class UpdateWorkflowStepCommandHandler(
    IWorkflowStepRepository _workflowStepRepository,
    IUnitOfWork<DocumentContext> _uow,
    IMapper _mapper,
    IHubContext<UserHub, IHubClient> hubContext,
         IConnectionMappingRepository connectionMappingRepository,
         UserInfoToken _userInfo
    ) : IRequestHandler<UpdateWorkflowStepCommand, ServiceResponse<List<WorkflowStepDto>>>
{
    public async Task<ServiceResponse<List<WorkflowStepDto>>> Handle(UpdateWorkflowStepCommand request, CancellationToken cancellationToken)
    {
        var workflowId = request.WorkflowSteps.Select(c => c.WorkflowId).FirstOrDefault();
        var workflowSteps = await _workflowStepRepository.All
            //.Include(a => a.WorkflowStepRoles)
            //.Include(a => a.WorkflowStepUsers)
            .Where(c => c.WorkflowId == workflowId).ToListAsync();
        if (workflowSteps.Count() == 0)
        {
            return ServiceResponse<List<WorkflowStepDto>>.Return404();
        }
        workflowSteps.ForEach(e =>
        {
            e.StepName = request.WorkflowSteps.Find(c => c.Id == e.Id).StepName;
            e.OrderNo = request.WorkflowSteps.Find(c => c.Id == e.Id).OrderNo;
            //e.IsSignatureRequired = request.WorkflowSteps.Find(c => c.Id == e.Id).IsSignatureRequired;
            e.UpdatedAt = DateTime.UtcNow;
        });
        _workflowStepRepository.UpdateRange(workflowSteps);

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

        var entitiesDto = _mapper.Map<List<WorkflowStepDto>>(workflowSteps);
        entitiesDto.OrderBy(c => c.CreatedAt);
        return ServiceResponse<List<WorkflowStepDto>>.ReturnResultWith201(entitiesDto);
    }
}

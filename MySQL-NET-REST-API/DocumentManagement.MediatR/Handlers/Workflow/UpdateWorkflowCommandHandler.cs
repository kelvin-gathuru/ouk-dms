using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using System;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;

namespace DocumentManagement.MediatR.Handlers
{
    public class UpdateWorkflowCommandHandler (
        IWorkflowRepository _workflowRepository,
        IUnitOfWork<DocumentContext> _uow, 
        IMapper _mapper,
        IHubContext<UserHub, IHubClient> hubContext,
             IConnectionMappingRepository connectionMappingRepository, 
             UserInfoToken _userInfo) : IRequestHandler<UpdateWorkflowCommand, ServiceResponse<WorkflowDto>>
    {
        public async Task<ServiceResponse<WorkflowDto>> Handle(UpdateWorkflowCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _workflowRepository.All
                .FirstOrDefaultAsync(c => c.Name.ToUpper() == request.Name.ToUpper() && c.Id != request.Id);
            if (entityExist != null)
            {
                return ServiceResponse<WorkflowDto>.Return409("Workflow with same name already exists.");
            }
            entityExist = await _workflowRepository.All
                .FirstOrDefaultAsync(c => c.Id == request.Id);
            if (entityExist == null)
            {
                return ServiceResponse<WorkflowDto>.Return409("Workflow does not exists.");
            }
            entityExist.Name = request.Name;
            entityExist.Description = request.Description;
            _workflowRepository.Update(entityExist);

            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<WorkflowDto>.Return500();
            }
            try
            {
                var user = connectionMappingRepository.GetUserInfoById(_userInfo.Id);

                if (user != null)
                {
                    await hubContext.Clients.AllExcept(new List<string> { user.ConnectionId }).RefreshWorkflowSettings();
                }
            }
            catch (Exception ex)
            {

            }

            var entityDto = _mapper.Map<WorkflowDto>(entityExist);
            return ServiceResponse<WorkflowDto>.ReturnResultWith200(entityDto);
        }
    }
}

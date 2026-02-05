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
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class AddWorkflowCommandHandler(
        IWorkflowRepository _workflowRepository,
        IUnitOfWork<DocumentContext> _uow,
        IMapper _mapper,
        UserInfoToken _userInfo,
        IHubContext<UserHub, IHubClient> hubContext,
             IConnectionMappingRepository connectionMappingRepository) : IRequestHandler<AddWorkflowCommand, ServiceResponse<WorkflowDto>>
    {
        public async Task<ServiceResponse<WorkflowDto>> Handle(AddWorkflowCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _workflowRepository.FindBy(c => c.Name.ToUpper() == request.Name.ToUpper()).FirstOrDefaultAsync();
            if (entityExist != null)
            {
                return ServiceResponse<WorkflowDto>.Return409("work flow with same name already exists.");
            }

            var entity = _mapper.Map<Workflow>(request);
            entity.Id = Guid.NewGuid();
            entity.UserId = _userInfo.Id;
            entity.CreatedBy = _userInfo.Id;
            entity.CreatedDate = DateTime.UtcNow;
            entity.IsWorkflowSetup = false;
            _workflowRepository.Add(entity);
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

            var entityDto = _mapper.Map<WorkflowDto>(entity);
            return ServiceResponse<WorkflowDto>.ReturnResultWith201(entityDto);
        }
    }
}
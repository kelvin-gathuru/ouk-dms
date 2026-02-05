using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class DeleteWorkflowStepCommandHandler(IUnitOfWork<DocumentContext> _uow, 
        IWorkflowStepRepository _workflowStepRepository,
         IHubContext<UserHub, IHubClient> hubContext,
             IConnectionMappingRepository connectionMappingRepository,
             UserInfoToken _userInfo) : IRequestHandler<DeleteWorkflowStepCommand, ServiceResponse<bool>>
    {
        public async Task<ServiceResponse<bool>> Handle(DeleteWorkflowStepCommand request, CancellationToken cancellationToken)
        {
            var workflow = await _workflowStepRepository.FindAsync(request.Id);
            if (workflow == null)
            {
                return ServiceResponse<bool>.Return404();
            }
            _workflowStepRepository.Remove(workflow);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
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
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
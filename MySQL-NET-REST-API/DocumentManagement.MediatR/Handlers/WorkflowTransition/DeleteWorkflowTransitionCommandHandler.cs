using DocumentFormat.OpenXml.Spreadsheet;
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
    public class DeleteWorkflowTransitionCommandHandler(
        IUnitOfWork<DocumentContext> _uow, 
        IWorkflowTransitionRepository _workflowTransitionRepository,
         IHubContext<UserHub, IHubClient> hubContext,
             IConnectionMappingRepository connectionMappingRepository,
             UserInfoToken _userInfo) : IRequestHandler<DeleteWorkflowTransitionCommand, ServiceResponse<bool>>
    {
        public async Task<ServiceResponse<bool>> Handle(DeleteWorkflowTransitionCommand request, CancellationToken cancellationToken)
        {
            var workflow = await _workflowTransitionRepository.FindAsync(request.Id);
            if (workflow == null)
            {
                return ServiceResponse<bool>.Return404();
            }
            _workflowTransitionRepository.Remove(workflow);
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
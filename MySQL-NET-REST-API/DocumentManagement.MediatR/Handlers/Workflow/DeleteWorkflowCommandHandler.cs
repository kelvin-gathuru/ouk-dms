using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace DocumentManagement.MediatR.Handlers
{
    public class DeleteWorkflowCommandHandler(IDocumentRepository _documentRepository,IUnitOfWork<DocumentContext> _uow, IWorkflowRepository _workflowRepository, IHubContext<UserHub, IHubClient> hubContext,
             IConnectionMappingRepository connectionMappingRepository, UserInfoToken _userInfo) : IRequestHandler<DeleteWorkflowCommand, ServiceResponse<bool>>
    {
        public async Task<ServiceResponse<bool>> Handle(DeleteWorkflowCommand request, CancellationToken cancellationToken)
        {
            var workflow = await _workflowRepository.FindAsync(request.Id);
            if (workflow == null)
            {
                return ServiceResponse<bool>.Return404();
            }
            var isExistingDoc = _documentRepository.All.Any(c => !c.IsDeleted && c.WorkflowInstances.Any(w => w.WorkflowId == request.Id));

            if (isExistingDoc)
            {
                return new ServiceResponse<bool>
                {
                    StatusCode = 404,
                    Errors = new List<string> { "Workflow can not be deleted. Document is assigned to this workflow." }
                };
            }
            _workflowRepository.Delete(workflow);
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

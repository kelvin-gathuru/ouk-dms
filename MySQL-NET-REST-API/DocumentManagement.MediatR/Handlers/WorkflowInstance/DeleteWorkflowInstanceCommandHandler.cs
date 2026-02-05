using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class DeleteWorkflowInstanceCommandHandler(IUnitOfWork<DocumentContext> _uow, IWorkflowInstanceRepository _workflowInstanceRepository) : IRequestHandler<DeleteWorkflowInstanceCommand, ServiceResponse<bool>>
    {
        public async Task<ServiceResponse<bool>> Handle(DeleteWorkflowInstanceCommand request, CancellationToken cancellationToken)
        {
            var workflowInstance = await _workflowInstanceRepository.FindAsync(request.Id);
            if (workflowInstance == null)
            {
                return ServiceResponse<bool>.Return404();
            }
            _workflowInstanceRepository.Delete(workflowInstance);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
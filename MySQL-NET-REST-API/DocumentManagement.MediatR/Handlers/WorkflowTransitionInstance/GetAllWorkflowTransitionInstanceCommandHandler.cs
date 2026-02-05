using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers.WorkflowTransitionInstance
{
    public class GetAllWorkflowTransitionInstanceCommandHandler(IWorkflowTransitionInstanceRepository _workflowTransitionInstanceRepository) : IRequestHandler<GetAllWorkflowTransitionInstanceCommand, WorkflowLogList>
    {
        public async Task<WorkflowLogList> Handle(GetAllWorkflowTransitionInstanceCommand request, CancellationToken cancellationToken)
      {
            return await _workflowTransitionInstanceRepository.GetWorkflowTransitionInstance(request.workflowLogResource);
        }
    }
}

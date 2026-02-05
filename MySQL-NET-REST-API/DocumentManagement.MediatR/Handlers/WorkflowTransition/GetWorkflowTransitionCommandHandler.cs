using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Queries;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using DocumentManagement.Repository;
using AutoMapper;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetWorkflowTransitionCommandHandler(IWorkflowTransitionRepository _workflowTransitionRepository, IMapper _mapper) : IRequestHandler<GetWorkflowTransitionQuery, ServiceResponse<WorkflowTransitionDto>>
    {
        public async Task<ServiceResponse<WorkflowTransitionDto>> Handle(GetWorkflowTransitionQuery request, CancellationToken cancellationToken)
        {
            var entity = await _workflowTransitionRepository.FindAsync(request.Id);

            if (entity == null)
            {
                return ServiceResponse<WorkflowTransitionDto>.Return409("Not found");
            }
            var data = _mapper.Map<WorkflowTransitionDto>(entity);
            return ServiceResponse<WorkflowTransitionDto>.ReturnResultWith200(data);
        }
    }
}


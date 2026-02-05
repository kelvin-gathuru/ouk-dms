using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetWorkflowStepCommandHandler(IWorkflowStepRepository _workflowStepRepository, IMapper _mapper) : IRequestHandler<GetWorkflowStepQuery, ServiceResponse<WorkflowStepDto>>
    {
        public async Task<ServiceResponse<WorkflowStepDto>> Handle(GetWorkflowStepQuery request, CancellationToken cancellationToken)
        {
            var entity = await _workflowStepRepository
                .FindAsync(request.Id);

            if (entity == null)
            {
                return ServiceResponse<WorkflowStepDto>.Return409("Not found");
            }
            var result = _mapper.Map<WorkflowStepDto>(entity);
            return ServiceResponse<WorkflowStepDto>.ReturnResultWith200(result);
        }
    }
}

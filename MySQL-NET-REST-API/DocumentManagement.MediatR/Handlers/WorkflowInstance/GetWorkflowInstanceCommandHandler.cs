using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetWorkflowInstanceCommandHandler(IWorkflowInstanceRepository _workflowInstanceRepository, IMapper _mapper) : IRequestHandler<GetWorkflowInstanceQuery, ServiceResponse<WorkflowInstanceDto>>
    {
        public async Task<ServiceResponse<WorkflowInstanceDto>> Handle(GetWorkflowInstanceQuery request, CancellationToken cancellationToken)
        {
            var entity = await _workflowInstanceRepository.All.FirstOrDefaultAsync(w => w.DocumentId == request.Id);
            if (entity == null)
            {
                return ServiceResponse<WorkflowInstanceDto>.Return409("Not found");
            }
            var result = _mapper.Map<WorkflowInstanceDto>(entity);
            return ServiceResponse<WorkflowInstanceDto>.ReturnResultWith200(result);
        }
    }
}
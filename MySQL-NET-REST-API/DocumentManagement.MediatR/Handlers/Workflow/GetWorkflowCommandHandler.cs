using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.MediatR.Handlers;

public class GetWorkflowCommandHandler(IWorkflowRepository _workflowRepository, IMapper _mapper) : IRequestHandler<GetWorkflowQuery, ServiceResponse<WorkflowDto>>
{
    public async Task<ServiceResponse<WorkflowDto>> Handle(GetWorkflowQuery request, CancellationToken cancellationToken)
    {
        var entity = await _workflowRepository.All
            .Include(a => a.WorkflowSteps.OrderBy(ws => ws.CreatedAt))
            //.ThenInclude(c => c.WorkflowStepRoles)
            .Include(a => a.WorkflowSteps.OrderBy(ws => ws.CreatedAt))
            //.ThenInclude(c => c.WorkflowStepUsers)
            .Include(a => a.WorkflowTransitions.OrderBy(wt => wt.OrderNo))
                 .ThenInclude(c => c.WorkflowTransitionRoles)
            .Include(a => a.WorkflowTransitions)
                 .ThenInclude(c => c.WorkflowTransitionUsers)
            .Include(a => a.WorkflowInstances)
            .FirstOrDefaultAsync(w => w.Id == request.Id);

        if (entity == null)
        {
            return ServiceResponse<WorkflowDto>.Return409("Not found");
        }
        var result = _mapper.Map<WorkflowDto>(entity);

        return ServiceResponse<WorkflowDto>.ReturnResultWith200(result);
    }
}

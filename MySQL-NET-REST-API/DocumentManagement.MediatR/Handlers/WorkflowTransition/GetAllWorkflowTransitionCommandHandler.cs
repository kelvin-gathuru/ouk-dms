using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetAllWorkflowTransitionCommandHandler (IWorkflowTransitionRepository _workflowTransitionRepository,IMapper _mapper): IRequestHandler<GetAllWorkflowTransitionQuery, ServiceResponse<List<WorkflowTransitionDto>>>
    {

        public async Task<ServiceResponse<List<WorkflowTransitionDto>>> Handle(GetAllWorkflowTransitionQuery request, CancellationToken cancellationToken)
        {
            var entities = await _workflowTransitionRepository.All.ToListAsync(cancellationToken);
            var dtos = _mapper.Map<List<WorkflowTransitionDto>>(entities);
            dtos.OrderBy(c => c.CreatedAt);
            return ServiceResponse<List<WorkflowTransitionDto>>.ReturnResultWith200(dtos);
        }
    }
}
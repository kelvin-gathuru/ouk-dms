using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;

public class GetAllWorkflowStepCommandHandler(IWorkflowStepRepository _workflowStepRepository, IMapper _mapper) : IRequestHandler<GetAllWorkflowStepQuery, ServiceResponse<List<WorkflowStepDto>>>
{

    public async Task<ServiceResponse<List<WorkflowStepDto>>> Handle(GetAllWorkflowStepQuery request, CancellationToken cancellationToken)
    {
        var entities = await _workflowStepRepository.All.ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<WorkflowStepDto>>(entities);
        dtos.OrderBy(c => c.OrderNo);
        return ServiceResponse<List<WorkflowStepDto>>.ReturnResultWith200(dtos);
    }
}

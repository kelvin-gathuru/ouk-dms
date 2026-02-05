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
    public class GetAllWorkflowCommandHandler(IWorkflowRepository _workflowRepository, IMapper _mapper) : IRequestHandler<GetAllWorkflowQuery, ServiceResponse<List<WorkflowDto>>>
    {
        public async Task<ServiceResponse<List<WorkflowDto>>> Handle(GetAllWorkflowQuery request, CancellationToken cancellationToken)
        {
            var entities = await _workflowRepository.All.OrderByDescending(c => c.CreatedDate).ToListAsync(cancellationToken);
            var dtos = _mapper.Map<List<WorkflowDto>>(entities);
            return ServiceResponse<List<WorkflowDto>>.ReturnResultWith200(dtos);
        }
    }
}

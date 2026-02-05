using AutoMapper;
using DocumentManagement.Data.Dto;
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
    public class GetReqDocumentWorkflowsQueryHandler (IWorkflowRepository workflowRepository, IMapper _mapper) : IRequestHandler<GetReqDocumentWorkflowsQuery, List<WorkflowDto>>
    {
        public async Task<List<WorkflowDto>> Handle(GetReqDocumentWorkflowsQuery request, CancellationToken cancellationToken)
        {
           var workflows = await workflowRepository.All
                .Where(c=>c.WorkflowTransitions.Any(d => d.IsFirstTransaction && d.IsUploadDocumentVersion) && c.IsWorkflowSetup)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<WorkflowDto>>(workflows);
        }
    }
}

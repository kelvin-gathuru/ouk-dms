using AutoMapper;
using DocumentManagement.Data;
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
    public class GetAllWorkflowNotStartedQueryHandler(
        IWorkflowRepository workflowRepository, 
        IWorkflowInstanceRepository workflowInstanceRepository, IMapper _mapper) : IRequestHandler<GetAllWorkflowNotStartedQuery, ServiceResponse<List<WorkflowDto>>>
    {
        public async Task<ServiceResponse<List<WorkflowDto>>> Handle(GetAllWorkflowNotStartedQuery request, CancellationToken cancellationToken)
        {
            var activeWorkflowIds = await workflowInstanceRepository.All.Where(c => c.Status == Data.WorkflowInstanceStatus.Initiated && c.DocumentId== request.DocumentId)
                .Select(c => c.WorkflowId)
                .ToListAsync(cancellationToken);
            List<Workflow> lstWorkflow;
            if (activeWorkflowIds.Count == 0)
            {
                 lstWorkflow = await workflowRepository.All.Where(c=> c.IsWorkflowSetup).ToListAsync(cancellationToken);
            }
            else
            {
                lstWorkflow = await workflowRepository.All.Where(c => !activeWorkflowIds.Contains(c.Id) && c.IsWorkflowSetup).ToListAsync(cancellationToken);
            }
            var dtos = _mapper.Map<List<WorkflowDto>>(lstWorkflow);
            return ServiceResponse<List<WorkflowDto>>.ReturnResultWith200(dtos);

        }
    }
}


using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetAllWorkflowInstanceWorkflowsCommandHandler(IWorkflowInstanceRepository workflowInstanceRepository) : IRequestHandler<GetAllWorkflowInstanceWorkflowsCommand, List<WorkflowShort>>
    {
        public async Task<List<WorkflowShort>> Handle(GetAllWorkflowInstanceWorkflowsCommand request, CancellationToken cancellationToken)
        {
            return await workflowInstanceRepository.All.Include(x => x.Workflow)
               .Select(x => new WorkflowShort
               {
                   Id = x.WorkflowId,
                   Name = x.Workflow.Name
               })
               .Distinct()
               .ToListAsync();
        }
    }
}

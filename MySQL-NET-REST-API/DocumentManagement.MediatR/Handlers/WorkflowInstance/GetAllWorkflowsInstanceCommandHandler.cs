using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers.WorkflowInstance
{
    public class GetAllWorkflowsInstanceCommandHandler(IWorkflowInstanceRepository _workflowInstanceRepository)
        : IRequestHandler<GetAllWorkflowsInstanceQuery, AllWorkflowInstanceList>
    {

        public async Task<AllWorkflowInstanceList> Handle(GetAllWorkflowsInstanceQuery request, CancellationToken cancellationToken)
        {
            return await _workflowInstanceRepository.GetWorkflowInstances(request.allWorkflowInstanceResource);
        }
    }
}
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.MediatR.Handlers;
public class GetWorkflowInstanceByWorkflowIdCommandHandler(IWorkflowInstanceRepository _workflowInstanceRepository, IMapper _mapper) : IRequestHandler<GetWorkflowInstanceByWorkflowIdQuery, ServiceResponse<bool>>
{
    public async Task<ServiceResponse<bool>> Handle(GetWorkflowInstanceByWorkflowIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _workflowInstanceRepository.All.FirstOrDefaultAsync(w => w.WorkflowId == request.WorkflowId);
        if (entity == null)
        {
            return ServiceResponse<bool>.Return409("Not found");
        }
        return ServiceResponse<bool>.ReturnResultWith200(true);
    }
}

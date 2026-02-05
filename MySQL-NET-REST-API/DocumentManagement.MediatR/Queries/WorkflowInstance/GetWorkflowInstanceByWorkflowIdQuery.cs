using System;
using DocumentManagement.Helper;
using MediatR;

namespace DocumentManagement.MediatR.Queries;
public class GetWorkflowInstanceByWorkflowIdQuery : IRequest<ServiceResponse<bool>>
{
    public Guid WorkflowId { get; set; }
}

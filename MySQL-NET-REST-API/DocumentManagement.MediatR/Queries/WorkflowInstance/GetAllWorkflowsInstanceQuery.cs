using DocumentManagement.Data.Resources;
using DocumentManagement.Repository;
using MediatR;

namespace DocumentManagement.MediatR.Queries
{
    public class GetAllWorkflowsInstanceQuery : IRequest<AllWorkflowInstanceList>
    {
        public AllWorkflowInstanceResource allWorkflowInstanceResource { get; set; }
    }
}


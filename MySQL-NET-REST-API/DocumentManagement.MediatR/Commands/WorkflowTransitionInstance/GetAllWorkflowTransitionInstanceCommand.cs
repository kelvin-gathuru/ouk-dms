using DocumentManagement.Data.Resources;
using DocumentManagement.Repository;
using MediatR;

namespace DocumentManagement.MediatR.Commands
{
    public class GetAllWorkflowTransitionInstanceCommand : IRequest<WorkflowLogList>
    {
        public WorkflowLogResource workflowLogResource { get; set; }
    }
}

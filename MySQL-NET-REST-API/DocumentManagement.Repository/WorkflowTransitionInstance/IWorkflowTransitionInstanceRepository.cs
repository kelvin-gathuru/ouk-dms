using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Data;
using DocumentManagement.Data.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Repository
{
    public interface IWorkflowTransitionInstanceRepository : IGenericRepository<WorkflowTransitionInstance>
    {
        Task<WorkflowLogList> GetWorkflowTransitionInstance(WorkflowLogResource workflowLogResource);
    }
}

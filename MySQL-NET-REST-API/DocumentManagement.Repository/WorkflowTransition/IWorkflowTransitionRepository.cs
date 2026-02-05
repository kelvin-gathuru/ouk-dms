using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace DocumentManagement.Repository
{
    public interface IWorkflowTransitionRepository : IGenericRepository<WorkflowTransition>
    {
        Task<List<PendingTransition>> GetPendingTransitions(Guid currentStepId);
    }
}

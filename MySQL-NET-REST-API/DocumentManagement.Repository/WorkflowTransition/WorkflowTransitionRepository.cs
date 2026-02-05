using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.Repository
{
    public class WorkflowTransitionRepository : GenericRepository<WorkflowTransition, DocumentContext>, IWorkflowTransitionRepository
    {
        public WorkflowTransitionRepository(IUnitOfWork<DocumentContext> uow) : base(uow)
        {
        }


        public async Task<List<PendingTransition>> GetPendingTransitions(Guid currentStepId)
        {
            return await _uow.Context.PendingTransitions
                     .FromSqlInterpolated($"CALL GetPendingWorkflowTransitions({currentStepId})")
                     .ToListAsync();
        }
    }
}

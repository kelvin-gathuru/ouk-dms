using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Resources;
using DocumentManagement.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagement.Repository
{
    public class WorkflowTransitionInstanceRepository : GenericRepository<WorkflowTransitionInstance, DocumentContext>, IWorkflowTransitionInstanceRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;
        public WorkflowTransitionInstanceRepository(IUnitOfWork<DocumentContext> uow, IPropertyMappingService propertyMappingService) : base(uow)
        {
            _propertyMappingService = propertyMappingService;
        }

        public async Task<WorkflowLogList> GetWorkflowTransitionInstance(WorkflowLogResource workflowLogResource)
        {
            var collectionBeforePaging = All
                .Include(w => w.WorkflowInstance)
                    .ThenInclude(w => w.Workflow)
                .Include(w => w.WorkflowInstance)
                    .ThenInclude(w => w.Document)
                    .IgnoreQueryFilters()
                .Include(w => w.WorkflowInstance)
                    .ThenInclude(w => w.InitiatedBy)
                .Include(w => w.WorkflowTransition)
                    .ThenInclude(w => w.FromWorkflowStep)
                .Include(w => w.WorkflowTransition)
                    .ThenInclude(w => w.ToWorkflowStep)
                .Include(w => w.PerformBy).AsQueryable();


            collectionBeforePaging = collectionBeforePaging.ApplySort(workflowLogResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<WorkflowTransitionLogDto, WorkflowTransitionInstance>());

            if (!string.IsNullOrWhiteSpace(workflowLogResource.WorkflowId))
            {
                collectionBeforePaging = collectionBeforePaging.
                    Where(c => c.WorkflowInstance.Workflow.Id == Guid.Parse(workflowLogResource.WorkflowId));
            }
            if (!string.IsNullOrWhiteSpace(workflowLogResource.DocumentId))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(c => c.WorkflowInstance.Document.Id == Guid.Parse(workflowLogResource.DocumentId));

            }
            if (workflowLogResource.WorkflowInstanceStatus != WorkflowInstanceStatus.All)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(c => c.WorkflowInstance.Status == workflowLogResource.WorkflowInstanceStatus);
            }

            var workflowLogList = new WorkflowLogList();
            return await workflowLogList.Create(
                collectionBeforePaging,
                workflowLogResource.Skip,
                workflowLogResource.PageSize
                );
        }
    }
}
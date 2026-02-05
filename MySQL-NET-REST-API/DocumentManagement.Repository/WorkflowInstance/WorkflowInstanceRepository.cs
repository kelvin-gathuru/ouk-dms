using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Resources;
using DocumentManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.Repository;

public class WorkflowInstanceRepository : GenericRepository<WorkflowInstance, DocumentContext>, IWorkflowInstanceRepository
{
    private readonly IPropertyMappingService _propertyMappingService;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly UserInfoToken _userInfoToken;

    public WorkflowInstanceRepository(IUnitOfWork<DocumentContext> uow, IPropertyMappingService propertyMappingService, UserInfoToken userInfoToken, IUserRoleRepository userRoleRepository) : base(uow)
    {
        _propertyMappingService = propertyMappingService;
        _userInfoToken = userInfoToken;
        _userRoleRepository = userRoleRepository;
    }

    public async Task<AllWorkflowInstanceList> GetWorkflowInstances(AllWorkflowInstanceResource allWorkflowInstanceResource)
    {
        var roles = _userRoleRepository.All.Where(u => u.UserId == _userInfoToken.Id).Select(r => r.RoleId).ToList();
        var currentUserId = _userInfoToken.Id;
        var collectionBeforePaging = All
            .Include(W => W.InitiatedBy)
            .Include(w => w.Workflow)
               .ThenInclude(ws => ws.WorkflowSteps)
             .Include(w => w.Workflow)
               .ThenInclude(c => c.WorkflowTransitions)
                   .ThenInclude(c => c.WorkflowTransitionRoles)
             .Include(w => w.Workflow)
               .ThenInclude(c => c.WorkflowTransitions)
                   .ThenInclude(c => c.WorkflowTransitionUsers)
           .Include(w => w.Document)
           .IgnoreQueryFilters()
           .Include(c => c.WorkflowStepInstances)
               .ThenInclude(c => c.WorkflowStep)
           .Include(c => c.WorkflowTransitionInstances)
                .ThenInclude(c => c.PerformBy)
           .Include(c => c.WorkflowTransitionInstances)
                .ThenInclude(c => c.WorkflowTransition)
               .AsQueryable();

        collectionBeforePaging = collectionBeforePaging.ApplySort(allWorkflowInstanceResource.OrderBy,
            _propertyMappingService.GetPropertyMapping<CurrentWorkflowDataDto, WorkflowInstance>());

        if (!string.IsNullOrWhiteSpace(allWorkflowInstanceResource.WorkflowId))
        {
            collectionBeforePaging = collectionBeforePaging.
                Where(c => c.Workflow.Id == Guid.Parse(allWorkflowInstanceResource.WorkflowId));
        }
        if (!string.IsNullOrWhiteSpace(allWorkflowInstanceResource.DocumentId))
        {
            collectionBeforePaging = collectionBeforePaging
                .Where(c => c.Document.Id == Guid.Parse(allWorkflowInstanceResource.DocumentId));

        }
        if (allWorkflowInstanceResource.WorkflowInstanceStatus != WorkflowInstanceStatus.All)
        {
            collectionBeforePaging = collectionBeforePaging
                .Where(c => c.Status == allWorkflowInstanceResource.WorkflowInstanceStatus);
        }

        var allWorkflowInstanceList = new AllWorkflowInstanceList(roles, currentUserId);
        return await allWorkflowInstanceList.Create(
            collectionBeforePaging,
            allWorkflowInstanceResource.Skip,
            allWorkflowInstanceResource.PageSize
            );
    }

    public async Task CancelWorkflowInstancesAsync(Guid documentId)
    {
        try
        {
            var workflowInstance = await All
          .Where(wi => wi.DocumentId == documentId &&
                       (wi.Status == WorkflowInstanceStatus.Initiated ||
                        wi.Status == WorkflowInstanceStatus.InProgress))
           .ExecuteUpdateAsync(setters =>
               setters.SetProperty(wi => wi.Status, WorkflowInstanceStatus.Cancelled));
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while cancelling workflow instances.", ex);

        }
    }

    public async Task CancelWorkflowInstancesByCategoryAsync(List<Guid> categories)
    {
        try
        {
            var workflowInstance = await All.Include(c => c.Document)
          .Where(wi => categories.Contains(wi.Document.CategoryId) &&
                       (wi.Status == WorkflowInstanceStatus.Initiated ||
                        wi.Status == WorkflowInstanceStatus.InProgress))
           .ExecuteUpdateAsync(setters =>
               setters.SetProperty(wi => wi.Status, WorkflowInstanceStatus.Cancelled));
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while cancelling workflow instances.", ex);

        }
    }
}


using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Data;
using DocumentManagement.Data.Resources;

namespace DocumentManagement.Repository;

public interface IWorkflowInstanceRepository : IGenericRepository<WorkflowInstance>
{
    Task<AllWorkflowInstanceList> GetWorkflowInstances(AllWorkflowInstanceResource allWorkflowInstanceResource);
    Task CancelWorkflowInstancesAsync(Guid documentId);
    Task CancelWorkflowInstancesByCategoryAsync(List<Guid> categories);
}

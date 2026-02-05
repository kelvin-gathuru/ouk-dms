using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;
public class ArchiveCategoryCommandHandler(
        ICategoryRepository _categoryRepository,
        IWorkflowInstanceRepository _workflowInstanceRepository,
        IUnitOfWork<DocumentContext> _uow,
         IHubContext<UserHub, IHubClient> hubContext,
        IConnectionMappingRepository connectionMappingRepository,
        UserInfoToken _userInfoToken,
        IDocumentAuditTrailRepository documentAuditTrailRepository,
        ILogger<DeleteDocumentCommandHandler> _logger) : IRequestHandler<ArchiveCategoryCommand, ServiceResponse<CategoryDto>>
{
    public async Task<ServiceResponse<CategoryDto>> Handle(ArchiveCategoryCommand request, CancellationToken cancellationToken)
    {
        var entityExist = await _categoryRepository.FindAsync(request.CategoryId);
        if (entityExist == null)
        {
            return ServiceResponse<CategoryDto>.ReturnFailed(404, "Not Found");
        }
        var categories = new List<Guid>();
        categories.Add(entityExist.Id);
        entityExist.IsArchive = true;
        entityExist.ArchiveById = _userInfoToken.Id;
        entityExist.ArchiveParentId = null;
        _categoryRepository.Update(entityExist);

        var documentAudit = new DocumentAuditTrail()
        {
            CategoryId = entityExist.Id,
            CreatedBy = _userInfoToken.Id,
            CreatedDate = DateTime.UtcNow,
            OperationName = DocumentOperation.Archived_Folder
        };
        documentAuditTrailRepository.Add(documentAudit);

        var childs = _categoryRepository.GetAllChildCategoryIdsUsingRawSql(request.CategoryId);
        if (childs.Count > 0)
        {
            var lstChildCategory = new List<Data.Entities.Category>();
            foreach (var child in childs)
            {
                var childEntity = await _categoryRepository.FindAsync(child);
                if (childEntity != null)
                {
                    childEntity.IsArchive = true;
                    childEntity.ArchiveParentId = entityExist.Id;
                    lstChildCategory.Add(childEntity);
                    categories.Add(childEntity.Id);
                }

            }
            _categoryRepository.UpdateRange(lstChildCategory);
        }
        await _workflowInstanceRepository.CancelWorkflowInstancesByCategoryAsync(categories);
        if (await _uow.SaveAsync() <= -1)
        {
            return ServiceResponse<CategoryDto>.Return500();
        }
        try
        {
            var onlineUsers = connectionMappingRepository.GetAllUsersExceptThis(new SignlarUser { Id = _userInfoToken.Id.ToString() });
            if (onlineUsers.Count() > 0)
            {
                await hubContext.Clients.All.ArchieveRestoreFolder(entityExist.ParentId);
                await hubContext.Clients.All.AddEditFolder(entityExist.ParentId);
                await hubContext.Clients.All.DeleteFolder(entityExist.Id);
            }
            else
            {
                await hubContext.Clients.All.DeleteFolder(entityExist.Id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SignalR Error");
        }
        return ServiceResponse<CategoryDto>.ReturnSuccess();
    }
}

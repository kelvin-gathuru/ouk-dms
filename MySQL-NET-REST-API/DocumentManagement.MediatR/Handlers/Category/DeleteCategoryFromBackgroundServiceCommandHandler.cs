using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;
public class DeleteCategoryFromBackgroundServiceCommandHandler(
     ICategoryRepository _categoryRepository,
        IUnitOfWork<DocumentContext> _uow,
        UserInfoToken _userInfoToken,
        IDocumentAuditTrailRepository documentAuditTrailRepository
        ) : IRequestHandler<DeleteCategoryFromBackgroundServiceCommand, bool>
{
    public async Task<bool> Handle(DeleteCategoryFromBackgroundServiceCommand request, CancellationToken cancellationToken)
    {
        var entityExist = await _categoryRepository.FindAsync(request.CategoryId);
        if (entityExist == null)
        {
            return false;
        }
        var categories = new List<Guid>();
        entityExist.IsDeleted = true;
        entityExist.DeletedBy = _userInfoToken.Id;
        entityExist.DeletedDate = DateTime.UtcNow;
        //entityExist.ArchiveById = _userInfoToken.Id;
        //entityExist.ArchiveParentId = null;
        _categoryRepository.Update(entityExist);

        var documentAudit = new DocumentAuditTrail()
        {
            CategoryId = entityExist.Id,
            CreatedBy = _userInfoToken.Id,
            CreatedDate = DateTime.UtcNow,
            OperationName = DocumentOperation.Deleted_Folder
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
                    childEntity.IsDeleted = true;
                    childEntity.DeletedBy = _userInfoToken.Id;
                    childEntity.DeletedDate = DateTime.UtcNow;
                    lstChildCategory.Add(childEntity);
                    categories.Add(childEntity.Id);
                }

            }
            _categoryRepository.UpdateRange(lstChildCategory);
        }
        if (await _uow.SaveAsync() <= -1)
        {
            return false;
        }

        return true;
    }
}

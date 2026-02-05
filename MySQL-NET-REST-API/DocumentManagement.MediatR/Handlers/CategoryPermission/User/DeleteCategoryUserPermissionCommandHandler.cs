using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;

public class DeleteCategoryUserPermissionCommandHandler(
    ICategoryUserPermissionRepository categoryUserPermissionRepository,
    IDocumentRepository documentRepository,
    IMediator mediator,
    IUnitOfWork<DocumentContext> uow,
    UserInfoToken userInfo,
    ICategoryRepository categoryRepository,
    IDocumentAuditTrailRepository documentAuditTrailRepository
) : IRequestHandler<DeleteCategoryUserPermissionCommand, CategoryUserPermissionDto>
{
    public async Task<CategoryUserPermissionDto> Handle(DeleteCategoryUserPermissionCommand request, CancellationToken cancellationToken)
    {
        var entity = await categoryUserPermissionRepository.FindAsync(request.Id);
        if (entity == null)
        {
            return new CategoryUserPermissionDto
            {
                StatusCode = 404,
                Messages = new List<string> { "Not Found" }
            };
        }
        var category = await categoryRepository.FindAsync(entity.CategoryId);
        if (category == null)
        {
            return new CategoryUserPermissionDto
            {
                StatusCode = 404,
                Messages = new List<string> { "Folder Not Found" }
            };
        }
        if (category.ParentId != null)
        {
            var command = new CheckShareUserByCategoryCommand()
            {
                CategoryId = (Guid)category.ParentId
            };

            var result = await mediator.Send(command);
            if (result.Data)
            {
                return new CategoryUserPermissionDto
                {
                    StatusCode = 422,
                    Messages = new List<string> { "Parent Folder is shared, permission cannot be deleted." }
                };
            }
        }

        var affectedCategoryIds = categoryRepository.GetAllChildCategoryIdsUsingRawSql(entity.CategoryId);
        affectedCategoryIds.Add(entity.CategoryId);

        var permissionsToRemove = categoryUserPermissionRepository.All
            .Where(p => affectedCategoryIds.Contains(p.CategoryId) && p.UserId == entity.UserId)
            .ToList();

        foreach (var perm in permissionsToRemove)
        {
            documentAuditTrailRepository.Add(new DocumentAuditTrail
            {
                CategoryId = perm.CategoryId,
                CreatedBy = userInfo.Id,
                CreatedDate = DateTime.UtcNow,
                OperationName = DocumentOperation.Removed_Folder_Permission,
                AssignToUserId = perm.UserId
            });
        }
        categoryUserPermissionRepository.RemoveRange(permissionsToRemove);
        if (await uow.SaveAsync() <= -1)
        {
            return new CategoryUserPermissionDto
            {
                StatusCode = 500,
                Messages = new List<string> { "An unexpected fault happened. Try again later." }
            };
        }
        await documentRepository.UpdateDocumentSharingFlagAsync(affectedCategoryIds);

        return new CategoryUserPermissionDto
        {
            StatusCode = 200,
            Messages = new List<string> { "Permission Deleted Successfully." }
        };
    }
}

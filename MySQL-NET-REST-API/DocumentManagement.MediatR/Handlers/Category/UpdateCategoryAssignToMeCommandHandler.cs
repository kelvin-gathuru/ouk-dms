using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.MediatR.Handlers;

public class UpdateCategoryAssignToMeCommandHandler(
    ICategoryRepository _categoryRepository,
    IUnitOfWork<DocumentContext> _uow,
    UserInfoToken userInfo,
    ICategoryUserPermissionRepository categoryUserPermissionRepository,
    IDocumentAuditTrailRepository documentAuditTrailRepository,
    IMapper _mapper) : IRequestHandler<UpdateCategoryAssignToMeCommand, CategoryDto>
{
    public async Task<CategoryDto> Handle(UpdateCategoryAssignToMeCommand request, CancellationToken cancellationToken)
    {
        var entityExist = await _categoryRepository
            .FindBy(c => c.Name == request.Name && c.Id != request.Id && c.ParentId == request.ParentId).FirstOrDefaultAsync();
        if (entityExist != null)
        {
            var errorDto = new CategoryDto
            {
                StatusCode = 409,
                Messages = new List<string> { "Category Name already exist for another category." }
            };
            return errorDto;
        }
        var categoriesChilds = _categoryRepository.GetAllChildrenAsync(request.Id);
        if (request.Id == request.ParentId || categoriesChilds.Any(c => c.Id == request.ParentId))
        {
            var errorDto = new CategoryDto
            {
                StatusCode = 409,
                Messages = new List<string> { "Parent folder cannot be the folder itself or any of its subfolders." }
            };
            return errorDto;
        }
        var entity = _mapper.Map<Data.Entities.Category>(request);
        _categoryRepository.Update(entity);
        var documentAudit = new DocumentAuditTrail()
        {
            CategoryId = entity.Id,
            CreatedBy = userInfo.Id,
            CreatedDate = DateTime.UtcNow,
            OperationName = DocumentOperation.Edited_Folder
        };
        documentAuditTrailRepository.Add(documentAudit);

        var categoryUserPermissionAssign = categoryUserPermissionRepository.All.Where(c => c.UserId == userInfo.Id && c.CategoryId == entity.Id).FirstOrDefault();
        if (categoryUserPermissionAssign == null)
        {
            var categoryUserPermission = new CategoryUserPermission
            {
                Id = Guid.NewGuid(),
                CategoryId = entity.Id,
                UserId = userInfo.Id,
                ParentId = null,
                StartDate = null,
                EndDate = null,
                IsTimeBound = false,
                IsAllowDownload = false
            };
            categoryUserPermissionRepository.Add(categoryUserPermission);
            var documentAuditPermission = new DocumentAuditTrail()
            {
                CategoryId = entity.Id,
                CreatedBy = userInfo.Id,
                CreatedDate = DateTime.UtcNow,
                OperationName = DocumentOperation.Added_Folder_Permission,
                AssignToUserId = userInfo.Id
            };
            documentAuditTrailRepository.Add(documentAuditPermission);
        }


        if (await _uow.SaveAsync() <= -1)
        {
            var errorDto = new CategoryDto
            {
                StatusCode = 500,
                Messages = new List<string> { "An unexpected fault happened. Try again later." }
            };
            return errorDto;
        }
        var entityDto = _mapper.Map<CategoryDto>(entity);
        return entityDto;
    }
}

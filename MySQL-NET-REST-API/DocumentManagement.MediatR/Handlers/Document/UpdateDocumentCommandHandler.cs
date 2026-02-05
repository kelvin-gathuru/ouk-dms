using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;

public class UpdateDocumentCommandHandler(
    IDocumentRepository _documentRepository,
    IUnitOfWork<DocumentContext> _uow,
    IMapper _mapper,
    ICategoryRepository categoryRepository,
    IConnectionMappingRepository connectionMappingRepository,
    UserInfoToken userInfoToken,
    ILogger<UpdateDocumentCommandHandler> _logger,
    IHubContext<UserHub, IHubClient> hubContext,
    IDocumentMetaDataRepository documentMetaDataRepository) : IRequestHandler<UpdateDocumentCommand, DocumentDto>
{
    public async Task<DocumentDto> Handle(UpdateDocumentCommand request, CancellationToken cancellationToken)
    {
        var entityExist = await _documentRepository.FindBy(c => c.Name == request.Name && c.Id != request.Id && c.CategoryId == request.CategoryId).FirstOrDefaultAsync();

        if (entityExist != null)
        {
            var errorDto = new DocumentDto
            {
                StatusCode = 409,
                Messages = new List<string> { "Document already exist." }
            };
            return errorDto;
        }
        var entity = await _documentRepository
                     .FindByInclude(c => c.Id == request.Id)
                     .FirstOrDefaultAsync();

        var metaData = await documentMetaDataRepository
            .FindBy(c => c.DocumentId == request.Id)
            .ToListAsync();
        if (entity != null && metaData.Count > 0)
        {
            documentMetaDataRepository.RemoveRange(metaData);
        }
        if (request.DocumentMetaDatas != null && request.DocumentMetaDatas.Count > 0)
        {
            var metaDataFilter = request.DocumentMetaDatas.Where(c => c.DocumentMetaTagId != null && (c.Metatag != "" || c.MetaTagDate != null)).ToList();
            if (metaDataFilter.Count > 0)
            {
                var metaDataEntities = _mapper.Map<List<Data.DocumentMetaData>>(metaDataFilter);
                metaDataEntities.ForEach(c =>
                {
                    c.DocumentId = request.Id;
                });
                documentMetaDataRepository.AddRange(metaDataEntities);
            }
        }

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.CategoryId = request.CategoryId;
        entity.DocumentStatusId = request.DocumentStatusId;
        entity.ClientId = request.ClientId;
        entity.RetentionPeriodInDays = request.RetentionPeriodInDays;
        entity.OnExpiryAction = request.OnExpiryAction;

        if (entity.OnExpiryAction != null && entity.RetentionPeriodInDays != null && entity.RetentionPeriodInDays > 0)
        {
            entity.RetentionDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(entity.RetentionPeriodInDays.Value));
        }
        else
        {
            entity.RetentionDate = null;
        }

        _documentRepository.Update(entity);
        if (await _uow.SaveAsync() <= 0)
        {
            var errorDto = new DocumentDto
            {
                StatusCode = 500,
                Messages = new List<string> { "An unexpected fault happened. Try again later." }
            };
            return errorDto;
        }
        var category = categoryRepository.All.Where(c => c.Id == entity.CategoryId).FirstOrDefault();
        try
        {
            var onlineUsers = connectionMappingRepository.GetAllUsersExceptThis(new SignlarUser { Id = userInfoToken.Id.ToString() });
            if (onlineUsers.Count() > 0)
            {
                var user = connectionMappingRepository.GetUserInfoById(userInfoToken.Id);
                if (user != null)
                {
                    await hubContext.Clients.AllExcept(new List<string> { user.ConnectionId }).SendNotificationFolderChange(category.ParentId);
                }
                else
                {
                    await hubContext.Clients.All.SendNotificationFolderChange(category.ParentId);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SignalR Error");
        }
        var entityDto = _mapper.Map<DocumentDto>(entity);
        return entityDto;
    }
}

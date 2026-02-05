using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Handlers.StorageStategies;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace DocumentManagement.MediatR.Handlers;

public class CreateReqDocumentWorkflowQueryHandler(
    IStorageSettingRepository storageSettingRepository,
    StorageServiceFactory storeageServiceFactory,
    UserInfoToken userInfo,
    IUnitOfWork<DocumentContext> uow,
    IDocumentRepository documentRepository,
    IUserRepository userRepository,
    IDocumentVersionRepository documentVersionRepository,
      IHubContext<UserHub, IHubClient> _hubContext,
    IMediator mediator,
    IConnectionMappingRepository connectionMappingRepository) : IRequestHandler<CreateReqDocumentWorkflowQuery, ServiceResponse<bool>>
{
    public async Task<ServiceResponse<bool>> Handle(CreateReqDocumentWorkflowQuery request, CancellationToken cancellationToken)
    {

        var storeageSetting = await storageSettingRepository.GetStorageSettingByIdOrLocal(request.StorageSettingId);

        var storageService = storeageServiceFactory.GetStorageService(storeageSetting.StorageType);

        var documentFile = CreateNoteDocument(request.Comment, request.Name);

        var fileNameKeyValut = await storageService.UploadFileAsync(documentFile, storeageSetting, "txt");

        //var url = UploadFile(request.Files[0]);
        var entity = new Document();
        entity.Id = Guid.NewGuid();
        entity.DocumentNumber = await documentRepository.GenerateDocumentNumberAsync();
        entity.Name = request.Name;
        entity.CategoryId = request.CategoryId;
        entity.CreatedBy = userInfo.Id;
        entity.CreatedDate = DateTime.UtcNow;
        entity.Url = fileNameKeyValut.FileName;
        entity.Key = fileNameKeyValut.Key;
        entity.IV = fileNameKeyValut.IV;
        entity.StorageType = storeageSetting.StorageType;
        entity.StorageSettingId = storeageSetting.Id;
        entity.IsAddedPageIndxing = true;
        entity.IsChunk = false;
        entity.IsAllChunkUploaded = true;
        entity.Extension = "txt";
        documentRepository.Add(entity);

        var version = new DocumentVersion
        {
            DocumentId = entity.Id,
            Url = fileNameKeyValut.FileName,
            Key = fileNameKeyValut.Key,
            IV = fileNameKeyValut.IV,
            IsCurrentVersion = true,
            VersionNumber = 1,
            CreatedBy = userInfo.Id,
            CreatedDate = DateTime.UtcNow,
            ModifiedBy = userInfo.Id,
            ModifiedDate = DateTime.UtcNow,
            SignById = entity.SignById,
            SignDate = entity.SignDate,
            Comment = entity.Comment,
            Extension = entity.Extension,
            IsChunk = false,
            IsAllChunkUploaded = true
        };
        documentVersionRepository.Add(version);

        if (await uow.SaveAsync() <= 0)
        {
            return ServiceResponse<bool>.ReturnFailed(500, "Error While Added Document");
        }
        var command = new AddWorkflowInstanceCommand
        {
            DocumentId = entity.Id,
            WorkflowId = request.WorkflowId
        };
        var result = await mediator.Send(command);

        if (result.StatusCode != 201)
        {
            return ServiceResponse<bool>.ReturnFailed(500, "Error While Added Workflow Instance");
        }
        try
        {

            var user = connectionMappingRepository.GetUserInfoById(userInfo.Id);
            if (user != null)
            {
                await _hubContext.Clients.AllExcept(new List<string> { user.ConnectionId }).RefreshDocuments(entity.CategoryId);
            }
        }
        catch (Exception)
        {

        }
        return ServiceResponse<bool>.ReturnResultWith200(true);

    }

    private IFormFile CreateNoteDocument(string comment, string name)
    {
        //Create txt filr and convert to IFormFile
        var requestBy = userRepository.FindBy(c => c.Id == userInfo.Id).Select(c => c.LastName + ' ' + c.FirstName).FirstOrDefault();
        var content = "Please upload file request by " + requestBy + "\n\n";
        if (!string.IsNullOrEmpty(comment))
        {
            content = content + ' ' + comment;
        }
        var bytes = System.Text.Encoding.UTF8.GetBytes(content);
        var stream = new System.IO.MemoryStream(bytes);
        var file = new FormFile(stream, 0, bytes.Length, name, "RequestDocumentThroughWorkflow.txt");
        return file;
    }
}

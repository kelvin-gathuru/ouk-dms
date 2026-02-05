using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.Repository;

public class DocumentList : List<DocumentDto>
{
    public DocumentList()
    {
    }
    public int Skip { get; private set; }
    public int TotalPages { get; private set; }
    public int PageSize { get; private set; }
    public int TotalCount { get; private set; }

    public DocumentList(List<DocumentDto> items, int count, int skip, int pageSize)
    {
        TotalCount = count;
        PageSize = pageSize;
        Skip = skip;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        AddRange(items);
    }

    public async Task<DocumentList> Create(IQueryable<Document> source, int skip, int pageSize)
    {
        var count = await GetCount(source);
        var dtoList = await GetDtos(source, skip, pageSize);
        var dtoPageList = new DocumentList(dtoList, count, skip, pageSize);
        return dtoPageList;
    }

    public async Task<DocumentList> CreateDocumentLibrary(IQueryable<Document> source, int skip, int pageSize)
    {
        var count = await GetCount(source);
        var dtoList = await GetDocumentLibraryDtos(source, skip, pageSize);
        var dtoPageList = new DocumentList(dtoList, count, skip, pageSize);
        return dtoPageList;
    }

    public async Task<int> GetCount(IQueryable<Document> source)
    {
        return await source.AsNoTracking().CountAsync();
    }

    public async Task<List<DocumentDto>> GetDtos(IQueryable<Document> source, int skip, int pageSize)
    {
        var entities = await source
            .Skip(skip)
            .Take(pageSize)
            .AsNoTracking()
            .AsSplitQuery()
            .Select(c => new DocumentDto
            {
                Id = c.Id,
                Name = c.Name,
                CreatedDate = c.CreatedDate,
                ModifiedDate = c.ModifiedDate == DateTime.MinValue ? null : c.ModifiedDate,
                CategoryId = c.CategoryId,
                Description = c.Description,
                CategoryName = c.Category.Name,
                Url = c.Url,
                CreatedBy = c.User != null ? $"{c.User.FirstName} {c.User.LastName}" : "",
                IsAllowDownload = true,
                DocumentStatusId = c.DocumentStatusId,
                DocumentStatus = c.DocumentStatus,
                ClientId = c.ClientId,
                Client = c.Client,
                StorageSettingId = c.StorageSettingId,
                StorageSettingName = c.StorageSetting.Name,
                StorageType = c.StorageType,
                IsAddedPageIndxing = c.IsAddedPageIndxing,
                IsSignatureExists = c.IsSignatureExists,
                SignBy = c.SignBy != null ? $"{c.SignBy.FirstName} {c.SignBy.LastName}" : "",
                SignByDate = c.SignDate,
                //DocumentNumber = c.DocumentNumber,
                IsChunk = c.IsChunk,
                IsShared = c.IsShared,
                Extension = c.Extension,
                DocumentNumber = c.DocumentNumber,
                CommentCount = c.DocumentComments.Count,
                RetentionPeriodInDays = c.RetentionPeriodInDays,
                OnExpiryAction = c.OnExpiryAction,
                ArchiveById = c.ArchiveById,
                ArchiveName = c.ArchiveBy != null ? $"{c.ArchiveBy.FirstName} {c.ArchiveBy.LastName}" : "",
                WorkflowsDetail = c.WorkflowInstances.Select(c => new WorkflowShortDetail
                {
                    WorkflowId = c.WorkflowId,
                    WorkflowName = c.Workflow.Name,
                    WorkflowInstaceStatus = c.Status,
                    WorkflowInstanceId = c.Id,
                }).ToList()
            })

            .ToListAsync();

        return entities;
    }

    public async Task<List<DocumentDto>> GetDocumentLibraryDtos(IQueryable<Document> source, int skip, int pageSize)
    {
        var entities = await source
            .Skip(skip)
            .Take(pageSize)
            .AsNoTracking()
            .Select(c => new DocumentDto
            {
                Id = c.Id,
                Name = c.Name,
                CreatedDate = c.CreatedDate,
                ModifiedDate = c.ModifiedDate == DateTime.MinValue ? null : c.ModifiedDate,
                CategoryId = c.CategoryId,
                Description = c.Description,
                CategoryName = c.Category.Name,
                CreatedBy = c.User != null ? $"{c.User.FirstName} {c.User.LastName}" : "",
                ExpiredDate = c.GetDoucmentExpiredDate(),
                Url = c.Url,
                DocumentStatusId = c.DocumentStatusId,
                DocumentStatus = c.DocumentStatus,
                ClientId = c.ClientId,
                Client = c.Client,
                StorageSettingId = c.StorageSettingId,
                StorageSettingName = c.StorageSetting.Name,
                StorageType = c.StorageType,
                IsSignatureExists = c.IsSignatureExists,
                SignBy = c.SignBy != null ? $"{c.SignBy.FirstName} {c.SignBy.LastName}" : "",
                SignByDate = c.SignDate,
                DocumentNumber = c.DocumentNumber,
                Extension = c.Extension,
                IsChunk = c.IsChunk,
                IsShared = c.IsShared,
                CommentCount = c.DocumentComments.Count,
                RetentionPeriodInDays = c.RetentionPeriodInDays,
                OnExpiryAction = c.OnExpiryAction,
                IsAllowDownload =
                (c.DocumentUserPermissions != null && c.DocumentUserPermissions.Any(c => c.IsAllowDownload)) ||
                (c.DocumentRolePermissions != null && c.DocumentRolePermissions.Any(c => c.IsAllowDownload)),
                WorkflowsDetail = c.WorkflowInstances.Select(c => new WorkflowShortDetail
                {
                    WorkflowId = c.WorkflowId,
                    WorkflowName = c.Workflow.Name,
                    WorkflowInstaceStatus = c.Status,
                    WorkflowInstanceId = c.Id,
                }).ToList()
            })
            .ToListAsync();
        return entities;
    }
}

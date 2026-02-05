using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.Repository;
public class RecentDocumentList : List<RecentDocumentDto>
{
    public RecentDocumentList()
    {
    }

    public int Skip { get; private set; }
    public int TotalPages { get; private set; }
    public int PageSize { get; private set; }
    public int TotalCount { get; private set; }

    public RecentDocumentList(List<RecentDocumentDto> items, int count, int skip, int pageSize)
    {
        TotalCount = count;
        PageSize = pageSize;
        Skip = skip;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        AddRange(items);
    }

    public async Task<RecentDocumentList> Create(IQueryable<DocumentAuditTrail> source, int skip, int pageSize)
    {
        var count = await GetCount(source);
        var dtoList = await GetDtos(source, skip, pageSize);
        var dtoPageList = new RecentDocumentList(dtoList, count, skip, pageSize);
        return dtoPageList;
    }

    public async Task<int> GetCount(IQueryable<DocumentAuditTrail> source)
    {
        return await source.AsNoTracking().CountAsync();
    }

    public async Task<List<RecentDocumentDto>> GetDtos(IQueryable<DocumentAuditTrail> source, int skip, int pageSize)
    {
        var entities = await source
            .Skip(skip)
            .Take(pageSize)
            .AsNoTracking()
            .Select(c => new RecentDocumentDto
            {
                Id = c.Id,
                DocumentId = c.DocumentId,
                CategoryId = c.CategoryId,
                CreatedDate = c.CreatedDate,
                DocumentName = c.Document.Name,
                Url = c.Document.Url,
                CategoryName = c.DocumentId == null ? c.Category.Name : c.Document.Category.Name,
                IsDocumentDeleted = c.DocumentId == null ? c.Category.IsDeleted : c.Document.IsDeleted,
                OperationName = c.OperationName.ToString(),
                DocumentNumber = c.DocumentId == null ? "" : c.Document.DocumentNumber
            })
            .ToListAsync();
        return entities;
    }
}

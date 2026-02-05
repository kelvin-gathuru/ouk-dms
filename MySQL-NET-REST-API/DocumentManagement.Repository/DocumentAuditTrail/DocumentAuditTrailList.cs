using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagement.Repository
{
    public class DocumentAuditTrailList : List<DocumentAuditTrailDto>
    {
        public DocumentAuditTrailList()
        {
        }

        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public DocumentAuditTrailList(List<DocumentAuditTrailDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<DocumentAuditTrailList> Create(IQueryable<DocumentAuditTrail> source, int skip, int pageSize)
        {
            var count = await GetCount(source);
            var dtoList = await GetDtos(source, skip, pageSize);
            var dtoPageList = new DocumentAuditTrailList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<DocumentAuditTrail> source)
        {
            return await source.AsNoTracking().CountAsync();
        }

        public async Task<List<DocumentAuditTrailDto>> GetDtos(IQueryable<DocumentAuditTrail> source, int skip, int pageSize)
        {
            var entities = await source
                .Skip(skip)
                .Take(pageSize)
                .AsNoTracking()
                .Select(c => new DocumentAuditTrailDto
                {
                    Id = c.Id,
                    DocumentId = c.DocumentId,
                    CategoryId = c.CategoryId,
                    CreatedDate = c.CreatedDate,
                    DocumentName = c.Document.Name,
                    Url = c.Document.Url,
                    CategoryName = c.DocumentId==null? c.Category.Name: c.Document.Category.Name,
                    IsDocumentDeleted = c.DocumentId == null ? c.Category.IsDeleted: c.Document.IsDeleted,
                    CreatedBy = c.CreatedByUser != null ? $"{c.CreatedByUser.FirstName} {c.CreatedByUser.LastName}" : "",
                    OperationName = c.OperationName.ToString(),
                    PermissionUser = c.AssignToUserId != null ? $"{c.AssignToUser.FirstName} {c.AssignToUser.LastName}" : "",
                    PermissionRole = c.AssignToRoleId != null ? $"{c.AssignToRole.Name}" : "",
                    Comment = c.Comment,
                    IsChunk = c.DocumentId == null ?false: c.Document.IsChunk,
                    DocumentNumber = c.DocumentId == null ? "":c.Document.DocumentNumber
                })
                .ToListAsync();
            return entities;
        }
    }
}

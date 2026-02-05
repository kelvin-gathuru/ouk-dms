using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Handlers.LuceneHandler;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;

public class DeepSearchCommandHandler(
    IWebHostEnvironment webHostEnvironment,
    Helper.PathHelper pathHelper,
    IDocumentVersionRepository documentVersionRepository,
    ILogger<DeepSearchCommandHandler> logger) : IRequestHandler<DeepSearchCommand, ServiceResponse<List<DocumentDto>>>
{
    public async Task<ServiceResponse<List<DocumentDto>>> Handle(DeepSearchCommand request, CancellationToken cancellationToken)
    {
        try
        {


            if (string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                return ServiceResponse<List<DocumentDto>>.ReturnFailed(404, "Please enter search text.");
            }
            string searchIndexPath = System.IO.Path.Combine(webHostEnvironment.WebRootPath, pathHelper.SearchIndexPath);
            var indexSearcherManager = new IndexSearcherManager(searchIndexPath);
            indexSearcherManager.CreateSearcher();
            var lstIds = indexSearcherManager.Search(request.SearchQuery);
            indexSearcherManager.Dispose();
            if (lstIds.Count > 0)
            {
                var documents = await documentVersionRepository.All
                    .Include(c => c.SignBy)
                    .Include(c => c.CreatedByUser)
                        .Include(c => c.Document)
                            .ThenInclude(c => c.Category)
                        .Include(c => c.Document)
                            .ThenInclude(c => c.DocumentStatus)
                        .Include(c => c.Document)
                            .ThenInclude(c => c.StorageSetting)
                        .Where(c => lstIds.Contains(c.Id) && !c.Document.IsArchive)
                                       .AsNoTracking()
                                       .Select(c => new DocumentDto
                                       {
                                           Id = c.Document.Id,
                                           DocumentVersionId = c.Id,
                                           Name = c.Document.Name,
                                           CreatedDate = c.CreatedDate,
                                           CategoryId = c.Document.CategoryId,
                                           Description = c.Document.Description,
                                           CategoryName = c.Document.Category.Name,
                                           Url = c.Url,
                                           CreatedBy = c.CreatedByUser != null ? $"{c.CreatedByUser.FirstName} {c.CreatedByUser.LastName}" : "",
                                           IsAllowDownload = false,
                                           DocumentStatusId = c.Document.DocumentStatusId,
                                           DocumentStatus = c.Document.DocumentStatus,
                                           StorageSettingId = c.Document.StorageSettingId,
                                           StorageSettingName = c.Document.StorageSetting.Name,
                                           StorageType = c.Document.StorageSetting.StorageType,
                                           IsAddedPageIndxing = c.Document.IsAddedPageIndxing,
                                           IsSignatureExists = c.SignById != null && c.SignById != System.Guid.Empty,
                                           SignBy = c.SignBy != null && c.SignById != System.Guid.Empty ? $"{c.SignBy.FirstName} {c.SignBy.LastName}" : "",
                                           SignByDate = c.SignDate,
                                           DocumentNumber = c.Document.DocumentNumber,
                                           VersionNumber = c.VersionNumber,
                                           Extension = c.Extension,
                                           IsChunk = c.IsChunk,
                                           IsShared = c.Document.IsShared,
                                           Comment = c.Document.Comment,
                                           CommentCount = c.Document.DocumentComments.Count,

                                       })
                                       .AsSplitQuery()
                                       .ToListAsync();
                return ServiceResponse<List<DocumentDto>>.ReturnResultWith200(documents);
            }

            return ServiceResponse<List<DocumentDto>>.ReturnFailed(404, "No document found");
        }
        catch (System.Exception ex)
        {
            logger.LogError(ex, "Error occurred while searching documents.");
            return ServiceResponse<List<DocumentDto>>.ReturnFailed(500, ex.Message);
        }
    }
}

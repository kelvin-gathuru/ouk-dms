using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.MediatR.Handlers.LuceneHandler;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class RemovePageIndexingCommandHandler(
        IDocumentRepository _documentRepository,
        Helper.PathHelper pathHelper,
            IWebHostEnvironment webHostEnvironment,
            ILogger<RemovePageIndexingCommandHandler> _logger,
            IUnitOfWork<DocumentContext> uow,
           IDocumentVersionRepository _documentVersionRepository) : IRequestHandler<RemovePageIndexingCommand, ServiceResponse<bool>>
    {
        public async Task<ServiceResponse<bool>> Handle(RemovePageIndexingCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _documentRepository.All.Where(c => c.Id == request.DocumentId).FirstOrDefaultAsync();
            if (entityExist == null)
            {
                return ServiceResponse<bool>.ReturnFailed(404, "Document is not found");
            }
            try
            {
                var documentVersion = await _documentVersionRepository.All.Where(c => c.DocumentId == entityExist.Id && c.IsCurrentVersion).FirstOrDefaultAsync();
                if (documentVersion != null)
                {
                    string searchIndexPath = System.IO.Path.Combine(webHostEnvironment.WebRootPath, pathHelper.SearchIndexPath);
                    var indexService = new IndexDeleteManager(searchIndexPath);
                    indexService.DeleteDocumentById(documentVersion.Id.ToString());
                    indexService.Dispose();
                }
                entityExist.IsAddedPageIndxing = false;
                _documentRepository.Update(entityExist);
                if (await uow.SaveAsync() <= 0)
                {
                    return ServiceResponse<bool>.ReturnFailed(500, "Error while removing document from index");
                }
                return ServiceResponse<bool>.ReturnResultWith200(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while removing document from index");
                return ServiceResponse<bool>.ReturnFailed(404, "Error while removing document from index");
            }
        }
    }
}

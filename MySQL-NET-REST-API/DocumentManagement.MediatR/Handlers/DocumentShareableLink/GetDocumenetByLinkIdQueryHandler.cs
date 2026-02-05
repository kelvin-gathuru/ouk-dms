using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetDocumenetByLinkIdQueryHandler : IRequestHandler<GetDocumenetByLinkIdQuery, ServiceResponse<DocumentShareableLinkDto>>
    {
        private readonly IDocumentShareableLinkRepository _documentShareableLinkRepository;

        public GetDocumenetByLinkIdQueryHandler(IDocumentShareableLinkRepository documentShareableLinkRepository)
        {
            _documentShareableLinkRepository = documentShareableLinkRepository;
        }
        public async Task<ServiceResponse<DocumentShareableLinkDto>> Handle(GetDocumenetByLinkIdQuery request, CancellationToken cancellationToken)
        {
            var doc = await _documentShareableLinkRepository
                    .AllIncluding(c => c.Document)
                    .FirstOrDefaultAsync(cs => cs.Id == request.Id);
            if (doc == null)
            {
                return ServiceResponse<DocumentShareableLinkDto>.Return404();
            }

            var result = new DocumentShareableLinkDto
            {
                DocumentId = doc.DocumentId,
                DocumentName = doc.Document.Name,
                IsAllowDownload = doc.IsAllowDownload
            };
            return ServiceResponse<DocumentShareableLinkDto>.ReturnResultWith200(result);
        }
    }
}

using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class CheckDocumentStoreAsChunkCommandHandler(
        IDocumentVersionRepository documentVersionRepository) : IRequestHandler<CheckDocumentStoreAsChunkCommand, ServiceResponse<bool>>
    {
        public async Task<ServiceResponse<bool>> Handle(CheckDocumentStoreAsChunkCommand request, CancellationToken cancellationToken)
        {
            var documentVersion = await documentVersionRepository.All.Where(c => (c.DocumentId == request.DocumentId && c.IsCurrentVersion) || c.Id == request.DocumentId).FirstOrDefaultAsync();
            if (documentVersion == null)
            {

                return ServiceResponse<bool>.ReturnFailed(404, "Document version is not found");
            }
            return ServiceResponse<bool>.ReturnResultWith200(documentVersion.IsChunk);
        }
    }
}

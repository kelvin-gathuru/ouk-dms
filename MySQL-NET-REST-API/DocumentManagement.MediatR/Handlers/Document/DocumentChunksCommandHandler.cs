using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class DocumentChunksCommandHandler(
        IDocumentChunkRepository documentChunkRepository,
        IDocumentVersionRepository documentVersionRepository,
        IMapper mapper) : IRequestHandler<DocumentChunksCommand, ServiceResponse<List<DocumentChunkDto>>>
    {
        public async Task<ServiceResponse<List<DocumentChunkDto>>> Handle(DocumentChunksCommand request, CancellationToken cancellationToken)
        {
            var documentVersion = await documentVersionRepository.All.Where(c => c.Id == request.DocumentId || (c.DocumentId == request.DocumentId && c.IsCurrentVersion)).FirstOrDefaultAsync();
            if (documentVersion == null)
            {
                return ServiceResponse<List<DocumentChunkDto>>.ReturnFailed(404, "Document version is not found");
            }
            var documentChunks = await documentChunkRepository.All.Where(c => c.DocumentVersionId == documentVersion.Id).OrderBy(c => c.ChunkIndex).ToListAsync();

            var documentChunksdto = mapper.Map<List<DocumentChunkDto>>(documentChunks);

            return ServiceResponse<List<DocumentChunkDto>>.ReturnResultWith200(documentChunksdto);

        }
    }
}

using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;
public class GetDocumentDetailByIdHandler(
    IDocumentRepository _documentRepository,
    IDocumentVersionRepository documentVersionRepository,
    IMapper mapper) : IRequestHandler<GetDocumentDetailById, DocumentDto>
{
    public async Task<DocumentDto> Handle(GetDocumentDetailById request, CancellationToken cancellationToken)
    {
        var documentVersion = await documentVersionRepository.All.Where(c => c.DocumentId == request.Id && c.IsCurrentVersion || c.Id == request.Id).FirstOrDefaultAsync();

        var document = await _documentRepository.All
            .FirstOrDefaultAsync(c => c.Id == documentVersion.DocumentId);

        return mapper.Map<DocumentDto>(document);
    }
}

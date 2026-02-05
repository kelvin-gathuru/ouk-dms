using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;

public class GetDocumentLibraryQueryHandler : IRequestHandler<GetDocumentLibraryQuery, DocumentList>
{
    private readonly IDocumentRepository _documentRepository;
    public GetDocumentLibraryQueryHandler(
       IDocumentRepository documentRepository
        )
    {
        _documentRepository = documentRepository;

    }
    public async Task<DocumentList> Handle(GetDocumentLibraryQuery request, CancellationToken cancellationToken)
    {
        return await _documentRepository.GetDocumentsLibrary(request.Email, request.DocumentResource);
    }
}

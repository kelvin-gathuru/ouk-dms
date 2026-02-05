using System.Threading;
using System.Threading.Tasks;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;

namespace DocumentManagement.MediatR.Handlers;
public class GetAllArchiveDocumentQueryHandler(IDocumentRepository _documentRepository) : IRequestHandler<GetAllArchiveDocumentQuery, DocumentList>
{
    public async Task<DocumentList> Handle(GetAllArchiveDocumentQuery request, CancellationToken cancellationToken)
    {
        return await _documentRepository.GetArchiveDocuments(request.DocumentResource);
    }
}

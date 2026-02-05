using System.Threading;
using System.Threading.Tasks;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;

namespace DocumentManagement.MediatR.Handlers;
public class GetAllRecentDocumentQueryHandler(IRecentDocumentRepository _recentDocumentRepository) : IRequestHandler<GetAllRecentDocumentQuery, RecentDocumentList>
{
    public async Task<RecentDocumentList> Handle(GetAllRecentDocumentQuery request, CancellationToken cancellationToken)
    {
        return await _recentDocumentRepository.GetRecentDocuments(request.DocumentResource);
    }
}

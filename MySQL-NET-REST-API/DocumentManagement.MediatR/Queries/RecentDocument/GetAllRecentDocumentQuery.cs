using DocumentManagement.Data.Resources;
using DocumentManagement.Repository;
using MediatR;

namespace DocumentManagement.MediatR.Queries;
public class GetAllRecentDocumentQuery : IRequest<RecentDocumentList>
{
    public DocumentResource DocumentResource { get; set; }
}

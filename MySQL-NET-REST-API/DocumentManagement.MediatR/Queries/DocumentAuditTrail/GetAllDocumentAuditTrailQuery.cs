using DocumentManagement.Data.Resources;
using DocumentManagement.Repository;
using MediatR;

namespace DocumentManagement.MediatR.Queries
{
    public class GetAllDocumentAuditTrailQuery : IRequest<DocumentAuditTrailList>
    {
        public DocumentResource DocumentResource { get; set; }
    }
}

using DocumentManagement.Data.Resources;
using DocumentManagement.Repository;
using MediatR;

namespace DocumentManagement.MediatR.Queries
{
    public class GetAllDocumentQuery : IRequest<DocumentList>
    {
        public DocumentResource DocumentResource { get; set; }
    }
}

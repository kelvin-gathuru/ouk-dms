using DocumentManagement.Data.Resources;
using DocumentManagement.Repository;
using MediatR;

namespace DocumentManagement.MediatR.Queries
{
    public class GetAllLoginAuditQuery : IRequest<LoginAuditList>
    {
        public LoginAuditResource LoginAuditResource { get; set; }
    }
}

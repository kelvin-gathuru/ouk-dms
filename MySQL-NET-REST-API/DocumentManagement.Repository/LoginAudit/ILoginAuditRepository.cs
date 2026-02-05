using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Data.Resources;
using System.Threading.Tasks;

namespace DocumentManagement.Repository
{
    public interface ILoginAuditRepository : IGenericRepository<LoginAudit>
    {
        Task<LoginAuditList> GetLoginAudits(LoginAuditResource loginAuditResrouce);
        Task LoginAudit(LoginAuditDto loginAudit);
    }
}

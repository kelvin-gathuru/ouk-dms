using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Data.Entities;
using DocumentManagement.Data.Resources;
using System.Threading.Tasks;

namespace DocumentManagement.Repository
{
    public interface INLogRepository : IGenericRepository<NLog>
    {
        Task<NLogList> GetNLogsAsync(NLogResource nLogResource);
    }
}

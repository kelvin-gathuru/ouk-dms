using System.Threading.Tasks;
using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Data.Entities;
using DocumentManagement.Data.Resources;

namespace DocumentManagement.Repository;
public interface IUserOpenaiMsgRepository : IGenericRepository<UserOpenaiMsg>
{
    Task<UserOpenaiMsgList> GetUserOpenaiMsgAsync(UserOpenaiMsgResource userOpenaiMsgResource);
}

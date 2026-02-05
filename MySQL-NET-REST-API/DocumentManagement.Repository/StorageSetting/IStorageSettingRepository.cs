using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Data;
using System.Threading.Tasks;
using System;

namespace DocumentManagement.Repository
{
    public interface IStorageSettingRepository : IGenericRepository<StorageSetting>
    {
        Task<StorageSetting> GetStorageSettingByIdOrLocal(Guid? StorageId);
    }
}

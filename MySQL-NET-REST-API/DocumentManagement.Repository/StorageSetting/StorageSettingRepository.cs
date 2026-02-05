using DocumentManagement.Common.GenericRepository;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace DocumentManagement.Repository;

public class StorageSettingRepository : GenericRepository<StorageSetting, DocumentContext>, IStorageSettingRepository
{
    public StorageSettingRepository(IUnitOfWork<DocumentContext> uow) : base(uow)
    {
    }

    public async Task<StorageSetting> GetStorageSettingByIdOrLocal(Guid? StorageId)
    {
        if (StorageId == Guid.Empty || StorageId == null)
        {
            return await All.Where(s => s.IsDefault || s.StorageType == StorageType.LOCAL_STORAGE).FirstOrDefaultAsync();
        }
        return await FindBy(s => s.Id == StorageId).FirstOrDefaultAsync();
    }
}

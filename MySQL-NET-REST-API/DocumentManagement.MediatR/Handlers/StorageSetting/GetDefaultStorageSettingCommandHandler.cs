using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers;
public class GetDefaultStorageSettingCommandHandler(IStorageSettingRepository storageSettingRepository) : IRequestHandler<GetDefaultStorageSettingCommand, StorageSettingDto<object>>
{
    public async Task<StorageSettingDto<object>> Handle(GetDefaultStorageSettingCommand request, CancellationToken cancellationToken)
    {
        var entity = await storageSettingRepository
             .All
             .AsNoTracking()
             .FirstOrDefaultAsync(x => x.IsDefault, cancellationToken);

        return new StorageSettingDto<object>
        {
            StorageType = entity.StorageType,
            Id = entity.Id,
            Name = entity.Name,
            JsonValue = null
        };
    }
}

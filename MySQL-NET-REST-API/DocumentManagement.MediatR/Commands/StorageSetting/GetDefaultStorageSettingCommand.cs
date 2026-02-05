using DocumentManagement.Data.Dto;
using MediatR;

namespace DocumentManagement.MediatR.Commands;
public class GetDefaultStorageSettingCommand : IRequest<StorageSettingDto<object>>
{
}

using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;

namespace DocumentManagement.MediatR.Commands
{
    public class AddStorageSettingCommand: IRequest<ServiceResponse<StorageSettingDto<string>>>
    {
        public string Name { get; set; }
        public string JsonValue { get; set; }
        public bool IsDefault { get; set; }
        public bool EnableEncryption { get; set; }
        public StorageType StorageType { get; set; }
    }
}

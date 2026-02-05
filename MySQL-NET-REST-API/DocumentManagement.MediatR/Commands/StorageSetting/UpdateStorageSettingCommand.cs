using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Helper;
using MediatR;
using System;

namespace DocumentManagement.MediatR.Commands
{
    public class UpdateStorageSettingCommand : IRequest<ServiceResponse<StorageSettingDto<string>>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string JsonValue { get; set; }
        public bool IsDefault { get; set; }
        public bool EnableEncryption { get; set; }
        public StorageType StorageType { get; set; }
    }
}

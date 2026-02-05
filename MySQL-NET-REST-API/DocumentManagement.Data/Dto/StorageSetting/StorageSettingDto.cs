using System;

namespace DocumentManagement.Data.Dto
{
    public class StorageSettingDto<T> where T:class
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public T JsonValue { get; set; } = null;
        public bool IsDefault { get; set; }
        public bool EnableEncryption { get; set; }
        public StorageType StorageType { get; set; }
    }
}

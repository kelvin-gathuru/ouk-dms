using System;

namespace DocumentManagement.Data
{
    public class StorageSetting : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string JsonValue { get; set; }
        public bool IsDefault { get; set; }
        public bool EnableEncryption { get; set; }
        public StorageType StorageType { get; set; }
    }
}

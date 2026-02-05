using System;

namespace DocumentManagement.Data
{
    public class Screen : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int? OrderNo { get; set; }
    }
}

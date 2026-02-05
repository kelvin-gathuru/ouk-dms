using System;

namespace DocumentManagement.Data
{
    public class PageHelper : BaseEntity
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }
}

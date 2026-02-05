using System;

namespace DocumentManagement.Data.Entities
{
    public class DocumentStatus
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ColorCode { get; set; }
    }
}

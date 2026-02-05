using System;

namespace DocumentManagement.Data.Dto
{
    public class DocumentStatusDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ColorCode { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace DocumentManagement.Data.Dto
{
    public class AllowFileExtensionDto
    {
        public Guid Id { get; set; }
        public FileType FileType { get; set; }
        public string Extension { get; set; }
        public List<string> Extensions { get; set; } = new List<string>();
    }
}

using System;

namespace DocumentManagement.Data
{
    public class AllowFileExtension
    {
        public Guid Id { get; set; }
        public FileType FileType { get; set; }
        public string Extension { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace DocumentManagement.Data.Dto
{
    public class DocumentSignatureDataDto
    {
        public string SignatureUser { get; set; }
        public string SignatureUrl { get; set; }
        public byte[] Data { get; set; }
        public DateTime? SignatureDate { get; set; }
    }
}

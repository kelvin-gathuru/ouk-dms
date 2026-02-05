using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Data.Dto
{
    public class UploadFileResponse
    {
        public string FileName { get; set; }
        public byte[] Key { get; set; }
        public byte[] IV { get; set; }
        public string Extension { get; set; }
        public string ErrorMessage { get; set; }
    }
}

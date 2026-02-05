using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Data.Dto.Document
{
    public class DownloadFileResponse
    {
        public byte[] FileBytes { get; set; }
        public string ErrorMessage { get; set; }
    }
}

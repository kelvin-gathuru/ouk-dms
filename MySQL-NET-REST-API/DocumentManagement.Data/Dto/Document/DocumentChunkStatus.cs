using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Data.Dto
{
    public class DocumentChunkStatus
    {
        public Guid DocumentId { get; set; }
        public bool Status { get; set; }
    }
}

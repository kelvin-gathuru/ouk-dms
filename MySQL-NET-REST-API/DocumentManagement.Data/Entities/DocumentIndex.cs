using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Data.Entities
{
    public class DocumentIndex
    {
        public Guid Id { get; set; }
        public Guid DocumentId { get; set; }
        [Column(TypeName = "datetime")]

        public DateTime CreatedDate { get; set; }
        public Guid DocumentVersionId { get; set; }
        [ForeignKey("DocumentVersionId")]
        public DocumentVersion DocumentVersion { get; set; }
    }
}

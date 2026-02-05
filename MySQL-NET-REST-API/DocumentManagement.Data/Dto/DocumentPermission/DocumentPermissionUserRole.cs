using System.Collections.Generic;

namespace DocumentManagement.Data.Dto
{
    public class DocumentPermissionUserRole
    {
        public List<string> Roles { get; set; }
        public List<string> Users { get; set; }
        public List<string> Documents { get; set; }
    }
}

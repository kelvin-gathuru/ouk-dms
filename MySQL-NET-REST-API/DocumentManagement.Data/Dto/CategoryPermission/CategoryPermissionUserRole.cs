using System.Collections.Generic;


namespace DocumentManagement.Data.Dto
{
    public class CategoryPermissionUserRole
    {
        public List<string> Roles { get; set; }
        public List<string> Users { get; set; }
        public List<string> Categories { get; set; }
    }
}

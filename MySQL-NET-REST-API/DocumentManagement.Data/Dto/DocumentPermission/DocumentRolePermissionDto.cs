using System;

namespace DocumentManagement.Data.Dto
{
    public class DocumentRolePermissionDto : ErrorStatusCode
    {
        public Guid? Id { get; set; }
        public Guid? DocumentId { get; set; }
        public Guid RoleId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsTimeBound { get; set; }
        public bool IsAllowDownload { get; set; }
        public RoleDto Role { get; set; }
        public bool IsAllowEmailNotification { get; set; }
    }
}

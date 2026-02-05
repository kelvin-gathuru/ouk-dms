using System;

namespace DocumentManagement.Data.Dto
{
    public class DocumentUserPermissionDto : ErrorStatusCode
    {
        public Guid? Id { get; set; }
        public Guid? DocumentId { get; set; }
        public Guid UserId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsTimeBound { get; set; }
        public bool IsAllowDownload { get; set; }
        public UserDto User { get; set; }
        public bool IsAllowEmailNotification { get; set; }
    }
}

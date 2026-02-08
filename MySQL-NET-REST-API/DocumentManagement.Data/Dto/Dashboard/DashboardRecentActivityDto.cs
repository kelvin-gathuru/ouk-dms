using System;

namespace DocumentManagement.Data.Dto
{
    public class DashboardRecentActivityDto
    {
        public Guid Id { get; set; }
        public DateTime ActivityTime { get; set; }
        public string Description { get; set; }
        public string ActivityType { get; set; } // "Login" or "Document"
        public string UserName { get; set; }
    }
}

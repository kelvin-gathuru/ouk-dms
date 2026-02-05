using System;
using System.Collections.Generic;
using DocumentManagement.Data.Dto;

namespace DocumentManagement.Data.Dto
{
    public class ClientDashboardDto
    {
        public int TotalPetitions { get; set; }
        public int PendingCount { get; set; }
        public int ApprovedCount { get; set; }
        public int RejectedCount { get; set; }
        public List<DocumentDto> RecentDocuments { get; set; } = new List<DocumentDto>();
        public List<DashboardActivityDto> RecentActivities { get; set; } = new List<DashboardActivityDto>();
    }

    public class DashboardActivityDto
    {
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
        public string Type { get; set; } // e.g., "SUBMISSION", "UPDATE", "STATUS_CHANGE"
        public Guid? DocumentId { get; set; }
    }
}

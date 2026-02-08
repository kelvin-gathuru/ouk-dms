using System;

namespace DocumentManagement.Data.Dto
{
    public class DashboardStatsDto
    {
        public int TotalDocuments { get; set; }
        public int PendingReviews { get; set; }
        public int ExpiringSoon { get; set; }
        public int RecentUploads { get; set; }
        public int TotalArchived { get; set; }
        public int AssignedDocuments { get; set; }
        public int AssignedWorkflows { get; set; }
        public string TotalDocumentsGrowth { get; set; }
        public string AssignedDocumentsGrowth { get; set; }
        public string PendingReviewsFooter { get; set; }
        public string AssignedWorkflowsFooter { get; set; }
    }
}

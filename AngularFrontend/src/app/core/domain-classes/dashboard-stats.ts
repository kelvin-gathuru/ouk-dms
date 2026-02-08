export interface DashboardStats {
    totalDocuments: number;
    pendingReviews: number;
    expiringSoon: number;
    recentUploads: number;
    totalArchived: number;
    assignedDocuments: number;
    assignedWorkflows: number;
    totalDocumentsGrowth: string;
    assignedDocumentsGrowth: string;
    pendingReviewsFooter: string;
    assignedWorkflowsFooter: string;
}

export interface NextTransition {
  transitionId: string;
  workflowInstanceId: string;
  workflowStepInstanceId: string;
  comment: string;
  isSignatureRequired?: boolean;
  isUserSignRequired?: boolean;
  isUploadDocumentVersion?: boolean;
  transitionName?: string;
  documentId?: string;
}

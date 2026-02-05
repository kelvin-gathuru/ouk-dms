export interface PerformTransition {
  workflowInstanceId: string;
  transitionId: string;
  workflowStepInstanceId: string;
  isUploadDocumentVersion: boolean;
  isSignatureRequired: boolean;
  comment: string;
  url: string;
  extension: string;
  signature: string;
  documentVersionId: string;
  documentId: string;
}

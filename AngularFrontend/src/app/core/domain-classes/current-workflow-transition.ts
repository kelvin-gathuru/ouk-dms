export class CurrentWorkflowTransition {
  id: string;
  name: string;
  comment: string;
  allowRoleToPerformTransition: boolean;
  allowUserToPerformTransition: boolean;
  isSignatureRequired: boolean;
  signatureBy: string;
  isUserSignRequired: boolean;
  isUploadDocumentVersion: boolean;
  color: string;
  orderNo: number;
  fromToStepName?: string;

}

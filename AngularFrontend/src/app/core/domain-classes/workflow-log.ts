import { WorkflowInstanceStatus } from "./workflow-instance-status.enum";
import { WorkflowTransitionInstanceStatus } from './workflow-transition-instance-status.enum';

export class WorkflowLog {
  workflowInstanceId: string;
  workflowId: string;
  workflowName: string;
  documentId: string;
  documentName: string;
  documentNumber: string
  isDocumentDeleted?: boolean;
  documentUrl: string;
  workflowInstanceStatus: WorkflowInstanceStatus;
  transitionName: string;
  initiatedBy: string;
  steps: string;
  workflowTransitionInstanceStatus: WorkflowTransitionInstanceStatus;
  initiatedAt: Date;
  transitionDate: Date;
  performBy: string;
  comment: string;
}

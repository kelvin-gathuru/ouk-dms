import { WorkflowInstanceStatus } from "./workflow-instance-status.enum";
import { CurrentWorkflowTransition } from "./current-workflow-transition";
import { WorkflowStepInstanceStatus } from './workflow-step-instance-status.enum';

export class WorkflowInstanceData {
  workflowId: string;
  workflowName: string;
  workflowInstanceStatus: WorkflowInstanceStatus;
  documentId: string;
  documentName: string;
  documentUrl: string;
  isDocumentDeleted?: boolean;
  workflowStepId: string;
  workflowStepName: string;
  workflowTransitions: CurrentWorkflowTransition[];
  workflowStepInstanceStatus: WorkflowStepInstanceStatus;
  workflowInstanceId: string;
  workflowStepInstanceId: string;
  documentNumber: string;
  lastTransition: string;
  lastTransitionSteps: string;
  workflowInitiatedDate: Date;
  initiatedUser?: string;
  updatedAt?: Date;
  performBy?: string;
  lastTransitionComment?: string;

}

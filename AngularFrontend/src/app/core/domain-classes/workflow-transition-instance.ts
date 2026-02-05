import { WorkflowTransitionStatus } from "./workflow-transition-status";

export interface WorkflowTransitionInstance {
  id: string;
  status: WorkflowTransitionStatus;
  workflowTransitionId: string;
  createdAt: Date;
  updatedAt: Date;
  comment: string;
  workflowInstanceId: string;
  transitionName: string;
}

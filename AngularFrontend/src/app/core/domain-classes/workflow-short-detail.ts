import { WorkflowInstanceStatus } from "./workflow-instance-status.enum";

export interface WorkflowShortDetail {
  workflowId: string;
  workflowInstanceId: string;
  workflowName: string;
  workflowInstaceStatus: WorkflowInstanceStatus
}

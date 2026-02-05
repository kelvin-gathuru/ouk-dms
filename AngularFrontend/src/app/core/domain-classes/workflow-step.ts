import { WorkflowStepStatus } from "./workflow-step-status";

export class WorkflowStep {
  id?: string;
  workflowId: string;
  stepName: string;
  status?: WorkflowStepStatus;
}

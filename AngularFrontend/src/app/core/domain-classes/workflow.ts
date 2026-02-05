import { WorkflowInstance } from './workflow-instance';
import { WorkflowStep } from './workflow-step';
import { WorkflowTransition } from './workflow-transition';

export class Workflow {
  id?: string;
  name: string;
  description: string;
  isWorkflowSetup: boolean;
  workflowSteps: WorkflowStep[];
  workflowTransitions: WorkflowTransition[];
  isWorkflowRedirectNextStep?: boolean;
  isWorkflowStepRedirectNextStep?: boolean;
  updatedAt?: Date;
  workflowInstances: WorkflowInstance[];
}

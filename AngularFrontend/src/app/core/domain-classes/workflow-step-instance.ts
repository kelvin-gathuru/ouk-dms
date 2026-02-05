import { User } from "./user";

export interface WorkflowStepInstance {
  id: string;
  workflowInstanceId: string;
  userId: string;
  user?: User;
  stepId: string;
  completedAt?: Date;
  createdAt: Date;
  updatedAt: Date;
  stepName: string;
}

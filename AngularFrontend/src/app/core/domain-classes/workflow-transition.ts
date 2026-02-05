import { User } from "./user";
import { WorkflowTransitionStatus } from "./workflow-transition-status";
import { WorkflowTransitionRole } from "./workflow-transition-role";
import { WorkflowTransitionUser } from "./workflow-transition-user";

export class WorkflowTransition {
  id?: string;
  workflowId: string;
  name: string;
  fromStepId: string;
  toStepId: string;
  condition: string;
  isFirstTransaction: boolean;
  status?: WorkflowTransitionStatus;
  fromStepName?: string;
  toStepName?: string;
  assignRoles?: string;
  assignUsers?: string;
  completedAt?: Date;
  createdAt?: Date;
  user?: User;
  minutes?: number;
  days?: number;
  hours?: number;
  isUploadDocumentVersion?: boolean;
  isSignatureRequired?: boolean;
  workflowTransitionRoles?: WorkflowTransitionRole[];
  workflowTransitionUsers?: WorkflowTransitionUser[];
  color?: string;
  roleIds?: string[];
  userIds?: string[];
  orderNo?: number;
  comment?: string;
}

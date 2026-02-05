import { CustomColor } from "./custom-color";
import { Link } from "./link";
import { WorkflowTransition } from "./workflow-transition";
import { Node } from '@swimlane/ngx-graph';
import { WorkflowInstanceStatus } from "./workflow-instance-status.enum";

export interface VisualWorkflowInstance {
  workflowId: string;
  workflowName: string;
  workflowInstanceStatus: WorkflowInstanceStatus;
  initiatedBy: string;
  pendingWorkflowTransitions?: WorkflowTransition[];
  workflowDescription?: string;
  documentId: string;
  documentName: string;
  documentNumber: string;
  createdAt: Date;
  updatedAt: Date;
  completedWorkflowTransitionInstances?: WorkflowTransition[]
  nodes?: Node[];
  links?: Link[];
  customColors?: CustomColor[];
}

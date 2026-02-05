import { ResourceParameter } from './resource-parameter';
import { WorkflowInstanceStatus } from './workflow-instance-status.enum';

export class WorkflowLogResource extends ResourceParameter {
  workflowId: string = '';
  documentId: string = '';
  workflowInstanceStatus: WorkflowInstanceStatus = WorkflowInstanceStatus.All;
}

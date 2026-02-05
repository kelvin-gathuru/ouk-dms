import { Pipe, PipeTransform } from '@angular/core';
import { WorkflowInstanceStatus } from '../../core/domain-classes/workflow-instance-status.enum';

@Pipe({
  name: 'workflowInstanceStatus',
  standalone: true
})

export class WorkflowInstanceStatusPipe implements PipeTransform {
  transform(value: any, ...args: any[]): any {
    if (value === WorkflowInstanceStatus.Cancelled) {
      return 'Cancelled';
    }
    else if (value === WorkflowInstanceStatus.Completed) {
      return 'Completed';
    }
    else if (value === WorkflowInstanceStatus.InProgress) {
      return 'In Progress';
    }
    else if (value === WorkflowInstanceStatus.Initiated) {
      return 'Initiated';
    }
  }
}

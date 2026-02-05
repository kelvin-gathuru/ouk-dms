import { Pipe, PipeTransform } from '@angular/core';
import { WorkflowStepInstanceStatus } from '../../core/domain-classes/workflow-step-instance-status.enum';

@Pipe({
  name: 'workflowStepInstanceStatus',
  standalone: true
})

export class WorkflowStepInstanceStatusPipe implements PipeTransform {
  transform(value: any, ...args: any[]): any {
    if (value === WorkflowStepInstanceStatus.Completed) {
      return 'Completed';
    }
    else if (value === WorkflowStepInstanceStatus.InProgress) {
      return 'In Progress';
    }
    else if (value === WorkflowStepInstanceStatus.Cancelled) {
      return 'Cancelled';
    }
  }
}

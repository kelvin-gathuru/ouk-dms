import { Pipe, PipeTransform } from '@angular/core';
import { WorkflowTransitionInstanceStatus } from '../../core/domain-classes/workflow-transition-instance-status.enum';

@Pipe({
  name: 'workflowTransitionInstanceStatus',
  standalone: true
})

export class WorkflowTransitionInstanceStatusPipe implements PipeTransform {
  transform(value: any, ...args: any[]): any {
    if (value === WorkflowTransitionInstanceStatus.Completed) {
      return 'Completed';
    }
    else if (value === WorkflowTransitionInstanceStatus.InProgress) {
      return 'In Progress';
    }
    else if (value === WorkflowTransitionInstanceStatus.Initiated) {
      return 'Initiated';
    }
  }
}

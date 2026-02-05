import { Pipe, PipeTransform } from '@angular/core';
import { WorkflowShortDetail } from '@core/domain-classes/workflow-short-detail';

@Pipe({
  name: 'workflowCombine',
  standalone: true
})
export class WorkflowCombinePipe implements PipeTransform {
  transform(value: WorkflowShortDetail[] | undefined, ...args: any[]): any {
    if (value && value.length > 0) {
      return value.map(x => x.workflowName).join(', ');
    }
    return ""
  }
}

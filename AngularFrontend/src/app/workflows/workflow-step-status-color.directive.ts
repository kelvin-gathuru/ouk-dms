import { Directive, ElementRef, Input } from '@angular/core';
import { WorkflowStepInstanceStatus } from '../core/domain-classes/workflow-step-instance-status.enum';
@Directive({
  selector: '[workflowstepstatuscolor]',
  standalone: true,
})
export class WorkflowStepStatusColorDirective {
  @Input() set workflowstepstatuscolor(status: WorkflowStepInstanceStatus) {
    this.setColor(status);
  }

  constructor(private el: ElementRef) { }

  private setColor(status: WorkflowStepInstanceStatus): void {
    switch (status) {
      case WorkflowStepInstanceStatus.Cancelled:
        this.el.nativeElement.style.color = 'red';
        break;
      case WorkflowStepInstanceStatus.Completed:
        this.el.nativeElement.style.color = 'green';
        break;
      case WorkflowStepInstanceStatus.InProgress:
        this.el.nativeElement.style.color = 'blue';
        break;
      default:
        this.el.nativeElement.style.color = 'black';
        break;
    }
  }
}

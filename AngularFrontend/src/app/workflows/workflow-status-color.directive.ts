import { Directive, ElementRef, Input } from '@angular/core';
import { WorkflowInstanceStatus } from '../core/domain-classes/workflow-instance-status.enum';

@Directive({
  selector: '[workflowstatuscolor]',
  standalone: true,
})
export class WorkflowStatusColorDirective {
  @Input() set workflowstatuscolor(status: WorkflowInstanceStatus) {
    this.setColor(status);
  }

  constructor(private el: ElementRef) { }

  private setColor(status: WorkflowInstanceStatus): void {
    switch (status) {
      case WorkflowInstanceStatus.Cancelled:
        this.el.nativeElement.style.color = 'red';
        break;
      case WorkflowInstanceStatus.Completed:
        this.el.nativeElement.style.color = 'green';
        break;
      case WorkflowInstanceStatus.InProgress:
        this.el.nativeElement.style.color = 'blue';
        break;
      case WorkflowInstanceStatus.Initiated:
        this.el.nativeElement.style.color = 'gray';
        break;
      default:
        this.el.nativeElement.style.color = 'black';
        break;
    }
  }
}

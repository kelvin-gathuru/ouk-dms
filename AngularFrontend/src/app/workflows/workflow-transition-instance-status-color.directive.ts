import { Directive, ElementRef, Input } from '@angular/core';
import { WorkflowTransitionInstanceStatus } from '../core/domain-classes/workflow-transition-instance-status.enum';
@Directive({
    selector: '[workflowtransitioninstancestatuscolor]',
    standalone: true,
  })
  export class WorkflowTransitionInstanceStatusColorDirective {
    @Input() set workflowtransitioninstancestatuscolor(status: WorkflowTransitionInstanceStatus) {
      this.setColor(status);
    }

    constructor(private el: ElementRef) {}

    private setColor(status: WorkflowTransitionInstanceStatus): void {
      switch (status) {
        case WorkflowTransitionInstanceStatus.Initiated:
          this.el.nativeElement.style.color = 'gray';
          break;
        case WorkflowTransitionInstanceStatus.Completed:
          this.el.nativeElement.style.color = 'green';
          break;
        case WorkflowTransitionInstanceStatus.InProgress:
          this.el.nativeElement.style.color = 'blue';
          break;
        default:
          this.el.nativeElement.style.color = 'black';
          break;
      }
    }
  }
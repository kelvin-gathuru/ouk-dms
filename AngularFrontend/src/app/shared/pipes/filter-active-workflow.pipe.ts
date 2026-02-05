import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'filter-active-workflow',
  standalone: true
})

export class FilterActiveWorkFlowPipe implements PipeTransform {
  transform(value: any, ...args: any[]): any {

  }
}

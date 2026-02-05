import { Pipe, PipeTransform } from '@angular/core';
import { FileRequestStatus } from '../../core/domain-classes/file-request.enum';

@Pipe({
  name: 'fileRequestStatus',
  standalone: true
})

export class FileRequestStatusPipe implements PipeTransform {
  transform(value: any, ...args: any[]): any {
    if (value === FileRequestStatus.CREATED) {
      return 'Created';
    }
    else if (value === FileRequestStatus.APPROVED) {
      return 'Approved';
    }
    else if (value === FileRequestStatus.REJECTED) {
      return 'Rejected';
    }
    else if (value === FileRequestStatus.UPLOADED) {
      return 'Uploaded';
    }
  }
}

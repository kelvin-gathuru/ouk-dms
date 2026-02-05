import { Pipe, PipeTransform } from '@angular/core';
import { FileRequestDocumentStatus } from '@core/domain-classes/file-request-document-status.enum';

@Pipe({
  name: 'fileRequestDocumentStatus',
  standalone: true
})

export class FileRequestDocumentStatusPipe implements PipeTransform {
  transform(value: any, ...args: any[]): any {
    if (value === FileRequestDocumentStatus.PENDING) {
      return 'Pending';
    }
    else if (value === FileRequestDocumentStatus.APPROVED) {
      return 'Approved';
    }
    else if (value === FileRequestDocumentStatus.REJECTED) {
      return 'Rejected';
    }
  }
}

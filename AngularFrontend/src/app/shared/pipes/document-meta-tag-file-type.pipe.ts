import { Pipe, PipeTransform } from '@angular/core';
import { MetaTagType } from '../../core/domain-classes/meta-tag-type.enum';

@Pipe({
  name: 'documentMetaTagFileType',
  standalone: true
})

export class DocumentMetaTagFileTypePipe implements PipeTransform {
  transform(value: any, ...args: any[]): any {
    if (value === MetaTagType.STRING) {
      return 'String';
    }
    else if (value === MetaTagType.DATETIME) {
      return 'DateTime';
    }
  }
}

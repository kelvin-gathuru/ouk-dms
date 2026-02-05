import { Pipe, PipeTransform } from '@angular/core';
import { StorageType } from './storage-type-enum';

@Pipe({
  name: 'storageType',
  standalone: true,
})
export class StorageTypePipe implements PipeTransform {
  transform(value: StorageType | undefined, ...args: any[]): string {
    if (value === undefined) {
      return 'Unknown';
    }
    const argsValue = args[0] ? `- ${args[0]}` : '';
    switch (value) {
      case StorageType.LOCAL:
        return 'Local';
      case StorageType.AWS:
        return `AWS ${argsValue}`;
      case StorageType.CLOUDFLARE:
        return `Cloudflare-R2 ${argsValue}`;
      default:
        return 'Unknown';
    }
  }
}

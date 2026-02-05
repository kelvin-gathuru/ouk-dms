import { Directive, ElementRef, Input, OnInit } from '@angular/core';

@Directive({
  selector: '[appDocumentOperationColor]',
  standalone: true,
})
export class DocumentOperationColorDirective implements OnInit {
  @Input() appDocumentOperationColor!: string;

  constructor(private el: ElementRef) { }

  ngOnInit() {
    this.setColor(this.appDocumentOperationColor);
  }

  private setColor(operationName: string) {
    let color = '';

    switch (operationName?.toLowerCase()) {
      case 'read': color = 'blue'; break;
      case 'created': color = 'green'; break;
      case 'added_folder': color = 'green'; break;
      case 'archived_folder': color = 'red'; break;
      case 'restored_folder': color = 'green'; break;
      case 'modified': color = 'orange'; break;
      case 'deleted': color = 'red'; break;
      case 'added_permission': color = 'green'; break;
      case 'removed_permission': color = 'red'; break;
      case 'send_email': color = 'cyan'; break;
      case 'download': color = 'darkblue'; break;
      case 'added_version': color = 'green'; break;
      case 'added_signature': color = 'green'; break;
      case 'restored_version': color = 'darkred'; break;
      case 'archived': color = 'red'; break;
      case 'restored': color = 'green'; break;
      case 'added_folder_permission': color = 'green'; break;
      case 'removed_folder_permission': color = 'red'; break;
      case 'edited_folder': color = 'orange'; break;
      case 'deleted_folder': color = 'red'; break;
      default: color = 'black';
    }

    this.el.nativeElement.style.color = color;
  }
}

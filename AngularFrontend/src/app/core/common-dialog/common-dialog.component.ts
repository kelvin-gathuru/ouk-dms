import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-common-dialog',
  templateUrl: './common-dialog.component.html',
  styleUrls: ['./common-dialog.component.scss'],
  standalone: true,
  imports: [
    MatDialogModule,
    TranslateModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule
  ]
})
export class CommonDialogComponent {
  primaryMessage: string;
  note: string = '';
  constructor(public dialogRef: MatDialogRef<CommonDialogComponent>) { }

  clickHandler(data: any): void {
    this.dialogRef.close(data);
  }
}

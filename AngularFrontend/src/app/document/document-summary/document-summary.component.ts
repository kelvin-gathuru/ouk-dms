import { Component, Inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { TranslateModule } from '@ngx-translate/core';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-document-summary',
  imports: [
    MatDialogModule,
    FormsModule,
    MatIconModule,
    MatButtonModule,
    TranslateModule,
    PageHelpTextComponent,
    MatCardModule
  ],
  templateUrl: './document-summary.component.html',
  styleUrl: './document-summary.component.scss'
})
export class DocumentSummaryComponent {
  documentSummary: string = '';
  constructor(public dialogRef: MatDialogRef<DocumentSummaryComponent>,
    @Inject(MAT_DIALOG_DATA) public data: string,) {
    this.documentSummary = data;
  }
  onDocumentCancel() {
    this.dialogRef.close(false);
  }
}

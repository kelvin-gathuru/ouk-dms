import { Component, Inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MAT_DIALOG_DATA, MatDialog, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';
import { PageHelper } from '@core/domain-classes/pageHelper';
import { HasClaimDirective } from '@shared/has-claim.directive';
import { TextEditorComponent } from '@shared/text-editor/text-editor.component';

@Component({
  selector: 'app-page-help-preview',
  templateUrl: './page-help-preview.component.html',
  styleUrls: ['./page-help-preview.component.scss'],
  standalone: true,
  imports: [
    MatDialogModule,
    TextEditorComponent,
    FormsModule,
    HasClaimDirective,
    MatIconModule,
    MatCardModule
  ]
})
export class PageHelpPreviewComponent implements OnInit {
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: PageHelper,
    private dialogRef: MatDialogRef<PageHelpPreviewComponent>,
    private router: Router,
    private matDialogRef: MatDialog
  ) { }

  ngOnInit(): void { }

  onCancel(): void {
    this.dialogRef.close();
  }

  editPageHelper() {
    this.matDialogRef.closeAll();
    this.router.navigate(['/page-helper/manage/', this.data.id]);
  }
}

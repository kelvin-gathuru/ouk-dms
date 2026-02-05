import { Component, Inject, OnInit } from '@angular/core';
import { ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { TranslateModule } from '@ngx-translate/core';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { BaseComponent } from '../../../base.component';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';


@Component({
  selector: 'app-upload-new-version-comment',
  imports: [
    ReactiveFormsModule,
    TranslateModule,
    MatDialogModule,
    PageHelpTextComponent,
    MatIconModule,
    MatCardModule
  ],
  templateUrl: './upload-new-version-comment.component.html',
  styleUrl: './upload-new-version-comment.component.scss'
})
export class UploadNewVersionCommentComponent extends BaseComponent implements OnInit {

  commentForm: UntypedFormGroup;

  constructor(
    private fb: UntypedFormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private dialogRef: MatDialogRef<UploadNewVersionCommentComponent>) {
    super();
  }

  ngOnInit(): void {
    this.createForm();
  }

  closeDialog() {
    this.dialogRef.close();
  }

  createForm() {
    this.commentForm = this.fb.group({
      comment: [this.data],
    });
  }
}

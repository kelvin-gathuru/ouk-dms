import { Component, Inject, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { DocumentShareableLink } from '@core/domain-classes/document-shareable-link';
import { BaseComponent } from '../../../base.component';
import { TranslateModule } from '@ngx-translate/core';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-document-link-preview-password',
  imports: [
    ReactiveFormsModule,
    MatDialogModule,
    TranslateModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './document-link-preview-password.component.html',
  styleUrls: ['./document-link-preview-password.component.scss'] // Corrected "styleUrl" to "styleUrls"
})
export class DocumentLinkPreviewPasswordComponent extends BaseComponent implements OnInit {
  documentLinkForm: FormGroup;
  isPasswordInvalid = false;

  constructor(
    private dialogRef: MatDialogRef<DocumentLinkPreviewPasswordComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DocumentShareableLink,
    private fb: FormBuilder
  ) {
    super();
  }

  ngOnInit(): void {
    this.createDocumentLinkForm();
  }

  createDocumentLinkForm() {
    this.documentLinkForm = this.fb.group({
      password: ['', [Validators.required]],
    });
  }

  checkPassword() {
    if (this.documentLinkForm.valid) {
      const password = this.documentLinkForm.get('password')?.value;
      this.dialogRef.close(password);
    } else {
      this.documentLinkForm.markAllAsTouched();
    }
  }
}

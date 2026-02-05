import { Component, ElementRef, inject, Inject, ViewChild } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatOptionModule } from '@angular/material/core';
import {
  MAT_DIALOG_DATA,
  MatDialogModule,
  MatDialogRef,
} from '@angular/material/dialog';
import { MatSelectModule } from '@angular/material/select';
import { TranslationService } from '@core/services/translation.service';

import { ToastrService } from '@core/services/toastr-service';
import SignaturePad from 'signature_pad';
import { DocumentSignature } from '../../core/domain-classes/document-signature';
import { DocumentService } from '../document.service';
import { DocumentAuditTrail } from '@core/domain-classes/document-audit-trail';
import { DocumentOperation } from '@core/domain-classes/document-operation';
import { CommonService } from '@core/services/common.service';
import { DocumentStore } from '../document-list/document-store';
import { MatProgressSpinnerModule, ProgressSpinnerMode } from '@angular/material/progress-spinner';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { TranslateModule } from '@ngx-translate/core';
import { UTCToLocalTime } from '@shared/pipes/utc-to-localtime.pipe';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-document-signature',
  imports: [
    MatDialogModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatOptionModule,
    MatSelectModule,
    MatProgressSpinnerModule,
    PageHelpTextComponent,
    TranslateModule,
    UTCToLocalTime,
    MatCardModule,
    MatIconModule
  ],
  templateUrl: './document-signature.component.html',
  styleUrl: './document-signature.component.scss'
})
export class DocumentSignatureComponent {
  @ViewChild('canvas', { static: true })
  canvasRef!: ElementRef<HTMLCanvasElement>;
  signaturePad!: SignaturePad;
  isLoading = false;
  documentSignatures: DocumentSignature[] = [];
  uploadedSignature: string | null = null;
  documentStore = inject(DocumentStore);
  mode: ProgressSpinnerMode = 'indeterminate';
  constructor(
    private toastrService: ToastrService,
    private translationService: TranslationService,
    private documentService: DocumentService,
    private commonService: CommonService,
    private dialogRef: MatDialogRef<DocumentSignatureComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
  ) { }

  ngOnInit() {
    this.initializeSignaturePad();
    this.getDocumentSignatures();
  }

  getDocumentSignatures() {
    this.documentService.getDocumentSignature(this.data?.id)
      .subscribe((documentSignatures: DocumentSignature[]) => {
        this.documentSignatures = documentSignatures;
      });
  }

  initializeSignaturePad() {
    const canvas = this.canvasRef.nativeElement;
    this.signaturePad = new SignaturePad(canvas);

    this.signaturePad.addEventListener('beginStroke', () => {
      if (this.uploadedSignature) {
        this.uploadedSignature = null;

        const fileInput = document.querySelector('input[type="file"].signature-upload') as HTMLInputElement;
        if (fileInput) fileInput.value = '';
      }
    });

    this.resizeCanvas();
  }

  resizeCanvas() {
    const canvas = this.canvasRef.nativeElement;
    const ratio = Math.max(window.devicePixelRatio || 1, 1);
    canvas.width = canvas.offsetWidth * ratio;
    canvas.height = canvas.offsetHeight * ratio;
    const ctx = canvas.getContext('2d');
    if (ctx) ctx.scale(ratio, ratio);
  }

  clearSignature() {
    this.signaturePad.clear();
  }


  saveSignature() {
    let signatureData: string | null = null;

    if (this.signaturePad && !this.signaturePad.isEmpty()) {
      signatureData = this.signaturePad.toDataURL();
    }
    else if (this.uploadedSignature) {
      signatureData = this.uploadedSignature;
    }
    if (!signatureData || signatureData === 'data:image/png;base64,' || signatureData === 'data:image/jpeg;base64,') {
      this.toastrService.error(this.translationService.getValue('PLEASE_PROVIDE_SIGNATURE'));
      return;
    }
    this.uploadSignature(signatureData);
  }

  uploadSignature(signatureData: string) {
    this.isLoading = true;
    const documentSignature: DocumentSignature = {
      documentId: this.data.id,
      signatureUrl: signatureData,
    };

    this.documentService.saveDocumentSignature(documentSignature).subscribe({
      next: (savedSignature: DocumentSignature) => {
        this.isLoading = false;
        this.documentSignatures.push(savedSignature);
        this.addDocumentTrail(savedSignature.documentId);
        this.dialogRef.close(documentSignature);
        this.uploadedSignature = null;
        this.signaturePad?.clear();
      },
      error: () => {
        this.isLoading = false;
        this.toastrService.error(this.translationService.getValue('SIGNATURE_UPLOAD_FAILED'));
      },
    });
  }

  onFileSelected(event: Event) {
    const fileInput = event.target as HTMLInputElement;
    const file = fileInput.files?.[0];
    if (!file) return;

    if (file.type !== 'image/png' && file.type !== 'image/jpeg') {
      this.toastrService.error(this.translationService.getValue('INVALID_FILE_TYPE'));
      return;
    }

    const reader = new FileReader();
    reader.onload = () => {
      const image = new Image();
      image.src = reader.result as string;

      image.onload = () => {
        const canvas = this.canvasRef.nativeElement;
        const ctx = canvas.getContext('2d');
        if (!ctx) return;
        const tempCanvas = document.createElement('canvas');
        tempCanvas.width = canvas.width;
        tempCanvas.height = canvas.height;
        const tempCtx = tempCanvas.getContext('2d');
        if (!tempCtx) return;
        tempCtx.clearRect(0, 0, tempCanvas.width, tempCanvas.height);
        tempCtx.drawImage(image, 0, 0, tempCanvas.width, tempCanvas.height);
        this.uploadedSignature = tempCanvas.toDataURL(file.type === 'image/jpeg' ? 'image/jpeg' : 'image/png');
        this.signaturePad.clear();
      };
    };
    reader.readAsDataURL(file);
  }



  addDocumentTrail(id: string) {
    const objDocumentAuditTrail: DocumentAuditTrail = {
      documentId: id,
      operationName: DocumentOperation.Added_Signature.toString(),
    };
    this.isLoading = false;
    this.toastrService.success(
      this.translationService.getValue('SIGNATURE_SAVED_SUCCESSFULLY')
    );
    this.documentStore.addDocumentAudit(objDocumentAuditTrail);
  }

  closeDialog() {
    this.dialogRef.close();
  }
}

import { Component, ElementRef, inject, Inject, ViewChild } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule, ProgressSpinnerMode } from '@angular/material/progress-spinner';
import { DocumentSignature } from '@core/domain-classes/document-signature';
import { ToastrService } from '@core/services/toastr-service';
import { TranslationService } from '@core/services/translation.service';
import { TranslateModule } from '@ngx-translate/core';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import SignaturePad from 'signature_pad';

@Component({
  selector: 'app-add-signature',
  imports: [
    MatDialogModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    PageHelpTextComponent,
    TranslateModule,
    MatCardModule,
    MatIconModule
  ],
  templateUrl: './add-signature.html',
  styleUrl: './add-signature.scss'
})
export class AddSignature {
  @ViewChild('canvas', { static: true })
  canvasRef!: ElementRef<HTMLCanvasElement>;
  signaturePad!: SignaturePad;
  isLoading = false;
  uploadedSignature: string | null = null;
  mode: ProgressSpinnerMode = 'indeterminate';
  toastrService = inject(ToastrService);
  translationService = inject(TranslationService);
  dialogRef = inject(MatDialogRef<AddSignature>);

  ngOnInit() {
    this.initializeSignaturePad();
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
      documentId: '',
      signatureUrl: signatureData,
    };
    this.dialogRef.close(documentSignature);
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

  closeDialog() {
    this.dialogRef.close();
  }
}

import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { TextEditorComponent } from '@shared/text-editor/text-editor.component';
import { SignalrService } from '@core/services/signalr.service';
import { TranslateModule } from '@ngx-translate/core';
import { ToastrService } from '@core/services/toastr-service'
import { AiResponseMsg } from '@core/domain-classes/ai-response-msg';
import { DomSanitizer } from '@angular/platform-browser';
import { StreamMarkdownService } from '../stream-markdown-service';
import html2canvas from 'html2canvas';
import jsPDF from 'jspdf';
import { MatDialog } from '@angular/material/dialog';
import { DocumentStore } from '../../document/document-list/document-store';
import { BaseComponent } from '../../base.component';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { TranslationService } from '@core/services/translation.service';

@Component({
  selector: 'app-ai-document-generator-preview',
  imports: [
    TextEditorComponent,
    ReactiveFormsModule,
    TranslateModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './ai-document-generator-preview.component.html',
  styleUrl: './ai-document-generator-preview.component.scss'
})
export class AiDocumentGeneratorPreviewComponent extends BaseComponent implements OnInit {
  isLoadingResults: boolean = false;
  aiEditorForm: FormGroup;
  fb = inject(FormBuilder);
  http = inject(HttpClient);
  toastrService = inject(ToastrService);
  signalrService = inject(SignalrService);
  sanitizer = inject(DomSanitizer);
  streamMarkdownService = inject(StreamMarkdownService);
  base64String: string | undefined;
  file: File | undefined;
  dialog = inject(MatDialog);
  documentStore = inject(DocumentStore);
  /**
   *
   */
  constructor() {
    super();

  }

  ngOnInit(): void {
    this.createAiEditorForm();
    this.getAiMessage();
  }
  createAiEditorForm(): void {
    this.aiEditorForm = this.fb.group({
      editorData: ['', [Validators.required]],
    });
  }
  getAiMessage(): void {
    this.sub$.sink = this.signalrService.aiPromptResponse$.subscribe(
      (response: AiResponseMsg) => {
        if (response && response.msg && response.msg.length > 0) {
          const html = this.streamMarkdownService.addChunk(response.msg);
          if (html === '[[DONE]]') {
            return;
          }
          this.aiEditorForm.patchValue({
            editorData: html,
          });
        }
      }
    );
  }
  generateDocument(): void {
    if (this.aiEditorForm.valid) {
      this.openAddDocumentDialog();
      return;
    } else {
      this.aiEditorForm.markAllAsTouched(); // Mark all fields as touched to show validation errors
    }
  }

  async openAddDocumentDialog() {
    this.isLoadingResults = true;
    try {
      const documentContent = this.aiEditorForm.get('editorData')?.value;
      const screenWidth = window.innerWidth;
      const dialogWidth = screenWidth < 768 ? '90vw' : '60vw';
      const { AddDocumentComponent } = await import(
        '../add-document/add-document.component'
      );
      const dialogRef = this.dialog.open(AddDocumentComponent, {
        maxWidth: dialogWidth,
        data: Object.assign({}, {
          documentContent: documentContent
        }),
      });
      dialogRef.afterClosed().subscribe((result: string) => {
        if (result === 'loaded') {
          this.aiEditorForm.patchValue({
            editorData: '',
          });
        }
      });
    }
    finally {
      this.isLoadingResults = false;
    }
  }
  generatePdf() {
    return new Promise((resolve, reject) => {
      let printContents = document.getElementById('pdfDocument') as HTMLElement;
      if (!printContents) {
        console.error('Content not found!');
        return;
      }
      html2canvas(printContents).then((canvas) => {
        const imgWidth = 210; // A4 width in mm
        const imgHeight = (canvas.height * imgWidth) / canvas.width;
        const imgData = canvas.toDataURL('image/png');
        const pdf = new jsPDF('p', 'mm', 'a4');
        pdf.addImage(imgData, 'PNG', 0, 20, imgWidth, imgHeight);
        // Get PDF as Blob
        this.file = this.blobToFile(
          pdf.output('blob'),
          'ai_document.pdf',
          'application/pdf'
        );
        resolve(true);
      });
    });
  }
  blobToFile(blob: Blob, fileName: string, mimeType = blob.type) {
    return new File([blob], fileName, {
      type: mimeType,
      lastModified: new Date().getTime(),
    });
  }
}

class MyUploadAdapter {
  constructor(
    private loader: any,
    private http: HttpClient,
    private toastrService: ToastrService,
    private translationService: TranslationService
  ) { }
  upload() {
    return this.loader.file.then(
      (file: File) =>
        new Promise((resolve, reject) => {
          this.convertToBase64(file)
            .then((base64: string) => {
              resolve({
                default: base64,
              });
            })
            .catch((error) => {
              this.toastrService.error(
                this.translationService.getValue('ERROR_WHILE_UPLOADING_IMAGE')
              );
              reject();
            });
        })
    );
  }
  convertToBase64(file: File): Promise<string> {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.readAsDataURL(file); // reads file as base64
      reader.onload = () => resolve(reader.result as string);
      reader.onerror = (error) => reject(error);
    });
  }
  abort() {
    console.error('aborted');
  }
}

import {
  ChangeDetectorRef,
  Component,
  ElementRef,
  inject,
  Inject,
  OnInit,
  ViewChild,
} from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import {
  MAT_DIALOG_DATA,
  MatDialogModule,
  MatDialogRef,
} from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import SignaturePad from 'signature_pad';
import { AllowFileExtension } from '@core/domain-classes/allow-file-extension';

import { ToastrService } from '@core/services/toastr-service';
import { CommonService } from '@core/services/common.service';
import { NextTransition } from '@core/domain-classes/next-transition';
import { WorkflowInstanceService } from '../workflow-instance.service';
import { PerformTransition } from './perform-transition';
import { DocumentVersion } from '@core/domain-classes/documentVersion';
import { DocumentInfo } from '@core/domain-classes/document-info';
import { DocumentService } from '../../document/document.service';
import { DocumentStore } from '../../document/document-list/document-store';
import { bufferCount, concatMap, from, mergeMap, Observable, tap } from 'rxjs';
import { DocumentChunk } from '@core/domain-classes/document-chunk';
import { CommonError } from '@core/error-handler/common-error';
import {
  ProgressSpinnerMode,
  MatProgressSpinnerModule,
} from '@angular/material/progress-spinner';
import { environment } from '@environments/environment';
import { SecurityService } from '@core/security/security.service';
import { FoldersViewStore } from '../../document/folders-view/folders-view-store';
import { validateFile } from '@core/domain-classes/extension-types';
import { OverlayPanel } from '@shared/overlay-panel/overlay-panel.service';
import { OverlayModule } from '@angular/cdk/overlay';
import { BaseComponent } from '../../base.component';
import { TranslateModule } from '@ngx-translate/core';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-perform-transition',
  imports: [
    MatDialogModule,
    ReactiveFormsModule,
    MatProgressSpinnerModule,
    OverlayModule,
    TranslateModule,
    PageHelpTextComponent,
    MatIconModule,
    MatCardModule,
    MatButtonModule
  ],
  templateUrl: './perform-transition.component.html',
  styleUrl: './perform-transition.component.scss'
})
export class PerformTransitionComponent
  extends BaseComponent
  implements OnInit {
  //@ViewChild('canvas', { static: true })
  @ViewChild('canvas') canvasRef!: ElementRef<HTMLCanvasElement>;
  signaturePad!: SignaturePad;
  performDocumentWorkflowForm: FormGroup;
  allowFileExtension: AllowFileExtension[] = [];
  fileData: any;
  extension: string = '';
  documentStore = inject(DocumentStore);
  isLoading = false;
  progress = 0;
  documentVersionId: string;
  mode: ProgressSpinnerMode = 'determinate';
  chunkSize = environment.chunkSize;
  uploadedSignature: string | null = null;
  securityService: SecurityService = inject(SecurityService);
  foldersViewStore = inject(FoldersViewStore);
  document: DocumentInfo;
  documentInfo: DocumentInfo;
  isPdf: boolean = false;
  isSignatureShow: boolean = false;

  constructor(
    private dialogRef: MatDialogRef<PerformTransitionComponent>,
    private fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: NextTransition,
    private cd: ChangeDetectorRef,
    private toastrService: ToastrService,
    private commonService: CommonService,
    private workflowInstanceService: WorkflowInstanceService,
    private documentService: DocumentService,
    public overlay: OverlayPanel
  ) {
    super();
  }

  ngAfterViewInit(): void {
    // Initialize after view (canvas) is ready
    setTimeout(() => {
      if (this.canvasRef?.nativeElement) {
        this.initializeSignaturePad();
      }
    });
  }

  ngOnInit() {
    this.isSignatureShow =
      this.data.isSignatureRequired && !this.data.isUploadDocumentVersion ? true : false;
    this.getDocumentInfo();
    this.getAllAllowFileExtension();
    this.createPerformDocumentWorkflowForm();
  }

  getDocumentInfo() {
    this.documentService.getDocumentDetail(this.data.documentId ?? '').subscribe({
      next: (documentInfo: DocumentInfo) => {
        this.documentInfo = documentInfo;
      },
      error: (error) => {
        this.isLoading = false;
      },
    });
  }

  getAllAllowFileExtension() {
    this.commonService.getAllowFileExtensions().subscribe();
    this.sub$.sink = this.commonService.allowFileExtension$.subscribe(
      (allowFileExtension: AllowFileExtension[]) => {
        if (allowFileExtension) {
          this.allowFileExtension = allowFileExtension;
        }
      }
    );
  }

  initializeSignaturePad(): void {
    const canvas = this.canvasRef.nativeElement;
    this.signaturePad = new SignaturePad(canvas, {
      backgroundColor: '#fff',
      penColor: 'black',
    });

    // Optional: resize canvas
    this.resizeCanvas();
  }

  resizeCanvas(): void {
    const canvas = this.canvasRef.nativeElement;
    const ratio = Math.max(window.devicePixelRatio || 1, 1);
    canvas.width = canvas.offsetWidth * ratio;
    canvas.height = canvas.offsetHeight * ratio;
    canvas.getContext('2d')?.scale(ratio, ratio);
  }

  clearSignature() {
    this.signaturePad.clear();
    this.uploadedSignature = null;
  }

  async upload(files: FileList | null): Promise<void> {
    if (!files || files.length === 0) return;
    if (!files[0] || !(await validateFile(files[0]))) {
      this.toastrService.error(
        this.translationService.getValue(
          'INVALID_EXTENSION_OR_CORRUPT_INVALID_SIGNATURE'
        )
      );
      this.fileUploadExtensionValidation('');
      this.cd.markForCheck();
      return;
    }
    this.extension = files[0].name.split('.').pop() ?? '';
    if (!this.fileExtesionValidation(this.extension)) {
      this.fileUploadExtensionValidation('');
      this.cd.markForCheck();
      return;
    } else {
      this.fileUploadExtensionValidation('valid');
    }

    this.fileData = files[0];
    this.performDocumentWorkflowForm.get('url')?.setValue(files[0].name);
  }

  fileUploadExtensionValidation(extension: string) {
    this.performDocumentWorkflowForm.patchValue({
      extension: extension,
    });
    this.performDocumentWorkflowForm.get('extension')?.markAsTouched();
    this.performDocumentWorkflowForm.updateValueAndValidity();
  }

  fileExtesionValidation(extesion: string): boolean {
    const allowExtesions = this.allowFileExtension;
    const allowTypeExtenstion = allowExtesions.find((c) =>
      c.extensions?.find((ext) => ext.toLowerCase() === extesion.toLowerCase())
    );
    return allowTypeExtenstion ? true : false;
  }

  createPerformDocumentWorkflowForm() {
    this.performDocumentWorkflowForm = this.fb.group({
      comment: [''],
      url: [''],
      extension: [''],
    });
    if (this.data.isUploadDocumentVersion) {
      this.performDocumentWorkflowForm
        .get('url')?.setValidators([Validators.required]);
      this.performDocumentWorkflowForm
        .get('extension')?.setValidators([Validators.required]);
      this.performDocumentWorkflowForm.updateValueAndValidity();
    }
  }

  get userAlreadySigned(): boolean {
    return this.data?.isSignatureRequired && this.data?.isUserSignRequired ? true : false;
  }

  get signatureDisplay(): boolean {
    return (
      (this.data?.isUploadDocumentVersion && this.data?.isSignatureRequired) ||
      this.userAlreadySigned
    );
  }

  createPerformDocumentWorkflow() {
    this.isLoading = true;
    let signatureData: string | null = null;

    if (this.data.isSignatureRequired || this.data.isUploadDocumentVersion) {
      if (this.signatureDisplay) {
        if (this.signaturePad && !this.signaturePad.isEmpty()) {
          signatureData = this.signaturePad.toDataURL();
        } else if (this.uploadedSignature) {
          signatureData = this.uploadedSignature;
        }

        if (
          !signatureData ||
          signatureData === 'data:image/png;base64,' ||
          signatureData === 'data:image/jpeg;base64,'
        ) {
          this.toastrService.error(
            this.translationService.getValue('PLEASE_PROVIDE_SIGNATURE')
          );
          this.isLoading = false;
          return;
        }
      }

      if (!this.performDocumentWorkflowForm.valid) {
        this.performDocumentWorkflowForm.markAllAsTouched();
        this.isLoading = false;
        return;
      }

      if (this.data.isUploadDocumentVersion) {
        this.saveDocument();
      } else {
        this.performDocumentWorkflow();
      }
    }
  }

  saveDocument() {
    const speedMbps = this.commonService.getInternetSpeed();
    if (this.fileData.size > this.chunkSize) {
      this.saveDocumentChunk();
    } else {
      const documentversion: DocumentVersion = {
        documentId: this.data.documentId,
        url: this.fileData.name,
        file: this.fileData,
        extension: this.extension,
        comment: '',
      };
      this.documentService.saveNewVersionDocument(documentversion).subscribe({
        next: (documentInfo: DocumentInfo) => {
          this.documentVersionId = documentInfo.id ?? '';
          this.documentStore.loadDocuments();
          if (
            this.foldersViewStore.selectedCategoryId() ==
            documentInfo.categoryId
          ) {
            this.foldersViewStore.setDocumentsEmpty();
            this.foldersViewStore.loadDocumentsByCategory(
              this.foldersViewStore.selectedCategoryId()
            );
          }
          this.performDocumentWorkflow();
        },
        error: (error) => {
          this.isLoading = false;
        },
      });
    }
  }

  saveDocumentChunk() {
    const document: DocumentVersion = {
      documentId: this.data.documentId,
      url: this.fileData.name,
      extension: this.extension,
      comment: '',
    };
    this.documentService.saveNewVersionDocumentChunk(document).subscribe({
      next: (c: DocumentInfo) => {
        this.document = c;
        this.documentVersionId = c.id ?? '';
        this.uploadFileInChunks(c.id ?? '');
      },
      error: (err) => {
        this.isLoading = false;
      },
    });
  }

  uploadFileInChunks(documentVersionId: string) {
    if (!this.fileData) return;
    const { chunkSize1, parallelCalls } = this.commonService.getNetworkSpeed();
    const totalChunks = Math.ceil(this.fileData.size / this.chunkSize);
    const chunkUploads: FormData[] = [];
    this.progress = 0;
    for (let i = 0; i < totalChunks; i++) {
      const start = i * this.chunkSize;
      const end = Math.min(start + this.chunkSize, this.fileData.size);
      const chunk = this.fileData.slice(start, end);
      const formData = new FormData();
      formData.append('file', chunk);
      formData.append('chunkIndex', i.toString());
      formData.append('size', this.chunkSize.toString());
      formData.append('totalChunks', totalChunks.toString());
      formData.append('extension', this.extension);
      formData.append('documentVersionId', documentVersionId);
      chunkUploads.push(formData);
    }

    this.sub$.sink = from(chunkUploads)
      .pipe(
        bufferCount(parallelCalls), // Group chunks in batches based on parallelCalls
        concatMap(
          (
            batch // Change concatMap to mergeMap
          ) =>
            from(batch).pipe(
              tap(() => console.log('Processing batch:', batch)),
              mergeMap((formData) => this.uploadChunk(formData), parallelCalls) // Execute uploads in parallel
            )
        )
      )
      .subscribe({
        next: (data: any) => {
          this.progress = Math.min(this.progress + 100 / totalChunks, 100);
          this.cd.markForCheck();
        },
        complete: () => {
          this.progress = 100;
          this.isLoading = false;
          this.markChunkAsUploaded();
          this.toastrService.success(
            this.translationService.getValue('DOCUMENT_SAVE_SUCCESSFULLY')
          );
          this.cd.markForCheck();
        },
        error: (err) => {
          this.markChunkAsUploaded(false);
          this.isLoading = false;
          this.cd.markForCheck();
        },
      });
  }

  uploadChunk(formData: FormData): Observable<DocumentChunk | CommonError> {
    return this.documentService.uploadChunkDocument(formData);
  }

  markChunkAsUploaded(flag: boolean = true) {
    this.isLoading = true;
    this.commonService
      .markChunkAsUploaded(this.documentVersionId, flag)
      .subscribe({
        next: (c) => {
          this.isLoading = false;
          this.documentStore.loadDocuments();
          if (
            this.foldersViewStore.selectedCategoryId() ==
            this.document.categoryId
          ) {
            this.foldersViewStore.setDocumentsEmpty();
            this.foldersViewStore.loadDocumentsByCategory(
              this.foldersViewStore.selectedCategoryId()
            );
          }
          this.performDocumentWorkflow();
        },
        error: (err) => {
          this.isLoading = false;
        },
      });
  }

  performDocumentWorkflow() {
    this.isLoading = true;
    const nextTransition = this.buidPerformDocumentWorkflow();
    this.mode = 'indeterminate';
    this.workflowInstanceService
      .performNextTransitionWithDocumentAndSignature(nextTransition)
      .subscribe({
        next: (data: boolean) => {
          this.isLoading = false;
          if (data) {
            this.toastrService.success(
              `${this.data.transitionName} ${this.translationService.getValue(
                'HAS_BEEN_SUCCESSFULLY_COMPLETED'
              )}`
            );
            this.dialogRef.close(true);
          }
        },
        error: (error) => {
          this.isLoading = false;
        },
      });
  }

  onClear() {
    this.signaturePad.clear();
  }

  buidPerformDocumentWorkflow(): PerformTransition {
    const nextTransition: PerformTransition = {
      workflowInstanceId: this.data.workflowInstanceId,
      transitionId: this.data.transitionId,
      workflowStepInstanceId: this.data.workflowStepInstanceId,
      isUploadDocumentVersion: this.data.isUploadDocumentVersion ?? false,
      isSignatureRequired: this.data.isSignatureRequired ?? false,
      comment: this.performDocumentWorkflowForm.get('comment')?.value,
      url: this.fileData,
      extension: this.extension,
      signature: this.data.isSignatureRequired
        ? this.uploadedSignature
          ? this.uploadedSignature
          : this.signaturePad.toDataURL()
        : '',
      documentVersionId: this.documentVersionId ? this.documentVersionId : '',
      documentId: this.data.documentId ?? '',
    };
    return nextTransition;
  }

  closeDialog() {
    this.dialogRef.close(false);
  }

  onFileSelected(event: Event) {
    const fileInput = event.target as HTMLInputElement;
    const file = fileInput.files?.[0];
    if (!file) return;

    if (file.type !== 'image/png' && file.type !== 'image/jpeg') {
      this.toastrService.error(
        this.translationService.getValue('INVALID_FILE_TYPE')
      );
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

        // Convert the drawn image to Base64
        this.uploadedSignature = tempCanvas.toDataURL(
          file.type === 'image/jpeg' ? 'image/jpeg' : 'image/png'
        );

        // Clear the previous drawn signature
        this.signaturePad.clear();
      };
    };
    reader.readAsDataURL(file);
  }
}

import { ChangeDetectorRef, Component, inject, Inject, OnInit } from '@angular/core';
import {
  ReactiveFormsModule,
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog, MatDialogModule } from '@angular/material/dialog';
import { DocumentInfo } from '@core/domain-classes/document-info';
import { DocumentVersion } from '@core/domain-classes/documentVersion';
import { ToastrService } from '@core/services/toastr-service';
import { BaseComponent } from '../../base.component';
import { DocumentService } from '../document.service';
import { AllowFileExtension } from '@core/domain-classes/allow-file-extension';
import { CommonService } from '@core/services/common.service';
import { OverlayPanel } from '@shared/overlay-panel/overlay-panel.service';
import { CommonDialogService } from '@core/common-dialog/common-dialog.service';
import { HttpEvent, HttpEventType, HttpResponse } from '@angular/common/http';
import { DocumentAuditTrail } from '@core/domain-classes/document-audit-trail';
import { DocumentOperation } from '@core/domain-classes/document-operation';
import { DocumentView } from '@core/domain-classes/document-view';
import { UploadNewVersionCommentComponent } from './upload-new-version-comment/upload-new-version-comment.component';
import { bufferCount, concatMap, from, mergeMap, Observable, tap } from 'rxjs';
import { DocumentChunk } from '@core/domain-classes/document-chunk';
import { CommonError } from '@core/error-handler/common-error';
import { DocumentChunkDownload } from '@core/domain-classes/document-chunk-download';
import { MatProgressSpinnerModule, ProgressSpinnerMode } from '@angular/material/progress-spinner';
import { DocumentStore } from '../document-list/document-store';
import { environment } from '@environments/environment';
import { FoldersViewStore } from '../folders-view/folders-view-store';
import { validateFile } from '@core/domain-classes/extension-types';
import { SecurityService } from '@core/security/security.service';
import { MatButtonModule } from '@angular/material/button';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { MatTooltipModule } from '@angular/material/tooltip';
import { HasClaimDirective } from '@shared/has-claim.directive';
import { TranslateModule } from '@ngx-translate/core';
import { LimitToPipe } from '@shared/pipes/limit-to.pipe';
import { UTCToLocalTime } from '@shared/pipes/utc-to-localtime.pipe';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';


@Component({
  selector: 'app-document-upload-new-version',
  templateUrl: './document-upload-new-version.component.html',
  styleUrls: ['./document-upload-new-version.component.scss'],
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
    PageHelpTextComponent,
    MatTooltipModule,
    MatProgressSpinnerModule,
    HasClaimDirective,
    TranslateModule,
    LimitToPipe,
    UTCToLocalTime,
    MatIconModule,
    MatCardModule,
  ]
})
export class DocumentUploadNewVersionComponent
  extends BaseComponent
  implements OnInit {
  isLoadingResults = false;
  documentForm: UntypedFormGroup;
  allowFileExtension: AllowFileExtension[] = [];
  fileData: any;
  extension: string = '';
  documentVersions: DocumentVersion[] = [];
  progress = 0;
  documentChunks: DocumentChunk[] = [];
  contentType: string = '';
  documentChunkDownloads: DocumentChunkDownload[] = [];
  documentUrl: Blob;
  isLoading: boolean = false;
  mode: ProgressSpinnerMode = 'determinate';
  documentStore = inject(DocumentStore);
  chunkSize = environment.chunkSize;
  foldersViewStore = inject(FoldersViewStore);
  constructor(
    private fb: UntypedFormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private cd: ChangeDetectorRef,
    private dialogRef: MatDialogRef<DocumentUploadNewVersionComponent>,
    private documentService: DocumentService,
    private toastrService: ToastrService,
    private commonService: CommonService,
    private securityService: SecurityService,
    private overlay: OverlayPanel,
    private dialog: MatDialog,
    private commandDialogService: CommonDialogService
  ) {
    super();
  }

  ngOnInit(): void {
    this.createDocumentForm();
    this.getAllAllowFileExtension();
    this.getDocumentHistories();
  }

  getDocumentHistories() {
    this.sub$.sink = this.documentService
      .getDocumentVersion(this.data.id)
      .subscribe((documentVersions: DocumentVersion[]) => {
        this.documentVersions = documentVersions;
      });
  }

  createDocumentForm() {
    this.documentForm = this.fb.group({
      url: ['', [Validators.required]],
      extension: ['', [Validators.required]],
      comment: [''],
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

  async upload(files: FileList | undefined | null): Promise<void> {
    if (!files || files.length === 0) return;
    if (! await validateFile(files[0])) {
      this.fileUploadExtensionValidation('');
      this.toastrService.error(this.translationService.getValue('INVALID_EXTENSION_OR_CORRUPT_INVALID_SIGNATURE'));
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
    this.documentForm.get('url')?.setValue(files[0].name);
  }

  fileUploadValidation(fileName: string) {
    this.documentForm.patchValue({
      url: fileName,
    });
    this.documentForm.get('url')?.markAsTouched();
    this.documentForm.updateValueAndValidity();
  }

  fileUploadExtensionValidation(extension: string) {
    this.documentForm.patchValue({
      extension: extension,
    });
    this.documentForm.get('extension')?.markAsTouched();
    this.documentForm.updateValueAndValidity();
  }

  fileExtesionValidation(extesion: string): boolean {
    const allowExtesions = this.allowFileExtension;
    var allowTypeExtenstion = allowExtesions.find((c) =>
      c.extensions?.find((ext) => ext.toLowerCase() === extesion.toLowerCase())
    );
    return allowTypeExtenstion ? true : false;
  }

  SaveDocument() {
    if (this.documentForm.invalid) {
      this.documentForm.markAllAsTouched();
      return;
    }
    const speedMbps = this.commonService.getInternetSpeed();
    if (this.fileData.size > this.chunkSize) {
      this.saveDocumentChunk();
    } else {
      const documentversion: DocumentVersion = {
        documentId: this.data.id,
        url: this.documentForm.get('url')?.value,
        file: this.fileData,
        extension: this.extension,
        comment: this.documentForm.get('comment')?.value,
      };
      this.sub$.sink = this.documentService
        .saveNewVersionDocument(documentversion)
        .subscribe((documentInfo: DocumentInfo) => {
          this.toastrService.success(
            this.translationService.getValue('DOCUMENT_SAVE_SUCCESSFULLY')
          );
          this.documentStore.loadDocuments();
          if (this.foldersViewStore.selectedCategoryId() == documentInfo.categoryId) {
            this.foldersViewStore.setDocumentsEmpty();
            this.foldersViewStore.loadDocumentsByCategory(this.foldersViewStore.selectedCategoryId())
          }
          this.dialogRef.close(true);
        });
    }

  }

  saveDocumentChunk() {
    const document: DocumentVersion = {
      documentId: this.data.id,
      url: this.documentForm.get('url')?.value,
      extension: this.extension,
      comment: this.documentForm.get('comment')?.value,
    }
    this.documentService.saveNewVersionDocumentChunk(document)
      .subscribe((c: DocumentInfo) => {
        this.uploadFileInChunks(c.id ?? '');
      });
  }

  uploadFileInChunks(documentVersionId: string) {
    if (!this.fileData) return;
    this.isLoading = true;
    const { chunkSize1, parallelCalls } = this.commonService.getNetworkSpeed();
    const totalChunks = Math.ceil(this.fileData.size / this.chunkSize);
    const chunkUploads = [];
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

    this.sub$.sink = from(chunkUploads).pipe(
      bufferCount(parallelCalls), // Group chunks in batches based on parallelCalls
      concatMap((batch) => // Change concatMap to mergeMap
        from(batch).pipe(
          tap(() => console.log("Processing batch:", batch)),
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
        }
      });
  }

  uploadChunk(formData: FormData): Observable<DocumentChunk | CommonError> {
    return this.documentService.uploadChunkDocument(formData);
  }

  markChunkAsUploaded(flag: boolean = true) {
    this.isLoading = true;
    this.commonService.markChunkAsUploaded(this.data.id, flag)
      .subscribe({
        next: (data) => {
          this.isLoading = false;
        },
        error: (error) => {
          this.isLoading = false;
        }
      });
  }

  closeDialog() {
    this.dialogRef.close();
  }

  async onDocumentView(documentVersion: DocumentVersion) {
    this.isLoadingResults = true;
    try {
      const urls = documentVersion?.url?.split('.') ?? [];
      const extension = urls[1];
      const documentView: DocumentView = {
        documentId: this.data.id,
        name: this.data.name,
        extension: documentVersion.extension,
        isVersion: true,
        id: this.data.id,
        isFromPublicPreview: false,
        isPreviewDownloadEnabled: this.securityService.hasClaim('ALL_DOWNLOAD_DOCUMENT'),
        isFileRequestDocument: false,
        isSignatureExists: documentVersion.isSignatureExists,
        url: documentVersion.url,
        comment: documentVersion.comment,
        documentVersionId: documentVersion.id,
        isChunk: documentVersion.isChunk,
        documentNumber: this.data.documentNumber,
      };

      const { BasePreviewComponent } = await import(
        '../../shared/base-preview/base-preview.component'
      );
      this.overlay.open(BasePreviewComponent, {
        position: 'center',
        origin: 'global',
        panelClass: ['file-preview-overlay-container', 'white-background'],
        data: documentView,
      });
    }
    finally {
      this.isLoadingResults = false;
    }
  }

  restoreDocumentVersion(version: DocumentVersion) {
    this.sub$.sink = this.commandDialogService
      .deleteConfirmtionDialog(this.translationService.getValue(
        'ARE_YOU_SURE_YOU_WANT_TO_RESTORE_THIS_TO_CURRENT_VERSION')
      )
      .subscribe((isTrue) => {
        if (isTrue) {
          this.sub$.sink = this.documentService
            .restoreDocumentVersion(this.data.id, version.id ?? '')
            .subscribe(() => {
              this.toastrService.success(
                this.translationService.getValue(
                  'VERSION_RESTORED_SUCCESSFULLY'
                )
              );
              this.addDocumentTrail(
                this.data.id,
                DocumentOperation.Restored.toString()
              );
              this.documentStore.loadDocuments();
              if (this.foldersViewStore.selectedCategoryId() == this.data.categoryId) {
                this.foldersViewStore.setDocumentsEmpty();
                this.foldersViewStore.loadDocumentsByCategory(this.foldersViewStore.selectedCategoryId())
              }
              this.getDocumentHistories();
            });
        }
      });
  }

  downloadDocument(version: DocumentVersion) {
    if (version.isChunk) {
      this.getAllDocumentChunks(version.id ?? '');
    } else {
      const docuView: DocumentView = {
        documentId: this.data.id,
        name: '',
        extension: version.url?.split('.')[1],
        isVersion: true,
        isFromPublicPreview: false,
        isPreviewDownloadEnabled: false,
        isFileRequestDocument: false,
        isSignatureExists: false,
        comment: version.comment,
        documentVersionId: version.id,
        documentNumber: this.data.documentNumber,
      };
      this.sub$.sink = this.commonService.downloadDocument(docuView).subscribe({
        next: (event: HttpEvent<Blob>) => {
          if (event.type === HttpEventType.Response) {
            this.addDocumentTrail(
              this.data.id,
              DocumentOperation.Download.toString()
            );
            if (event.body) {
              this.documentUrl = new Blob([event.body], { type: event.body.type });
              this.downloadFile();
            } else {
              this.toastrService.error(
                this.translationService.getValue('ERROR_WHILE_DOWNLOADING_DOCUMENT')
              );
            }
          }
        },
        error: (error) => {
          this.toastrService.error(
            this.translationService.getValue('ERROR_WHILE_DOWNLOADING_DOCUMENT')
          );
        }
      });
    }
  }

  getAllDocumentChunks(documentVersionId: string) {
    this.sub$.sink = this.commonService.getDocumentChunks(documentVersionId).subscribe({
      next: (data) => {
        this.documentChunks = data;
        if (this.documentChunks.length > 0) {
          this.startDownload(documentVersionId);
        }
      },
      error: (err) => {
        this.toastrService.error(err.error.message);
      },
    });

  }

  startDownload(documentVersionId: string) {
    this.progress = 0;
    this.isLoading = true;
    const { chunkSize1, parallelCalls } = this.commonService.getNetworkSpeed();
    const chunkRequests = [];
    for (let i = 0; i < this.documentChunks.length; i++) {
      chunkRequests.push(i);
    }
    this.sub$.sink = from(chunkRequests)
      .pipe(
        bufferCount(parallelCalls), // Group requests into batches of 5
        concatMap((batch) =>
          from(batch).pipe(
            mergeMap((chunkIndex) => this.downloadChunk(chunkIndex, documentVersionId), parallelCalls) // Retrieve 5 chunks in parallel
          )
        )
      )
      .subscribe({
        next: (documentChunkDownload: DocumentChunkDownload) => {
          this.progress = Math.min(this.progress + 100 / chunkRequests.length, 100);
          this.contentType = documentChunkDownload.contentType;
          const chunkBlob = this.base64ToBlob(documentChunkDownload.data, documentChunkDownload.contentType);
          documentChunkDownload.blobChunk = chunkBlob;
          this.documentChunkDownloads.push(documentChunkDownload);
        },
        complete: () => this.mergeChunks(),
        error: (err) => {
          this.isLoading = false;
          console.error('Error downloading chunks', err)
        }
      });
  }

  downloadChunk(chunkIndex: number, documentVersionId: string) {
    return this.commonService.downloadDocumentChunk(documentVersionId, chunkIndex);
  }

  mergeChunks() {
    this.progress = 100;
    this.isLoading = false;
    const sortedChunks = this.documentChunkDownloads
      .sort((a, b) => a.chunkIndex - b.chunkIndex)
      .map(entry => entry.blobChunk)
      .filter((chunk): chunk is Blob => chunk !== undefined);
    const blob = new Blob(sortedChunks, { type: this.contentType });
    this.documentUrl = blob;
    this.downloadFile();
  }

  private base64ToBlob(base64: string, contentType: string): Blob {
    const byteCharacters = atob(base64);
    const byteNumbers = new Array(byteCharacters.length);
    for (let i = 0; i < byteCharacters.length; i++) {
      byteNumbers[i] = byteCharacters.charCodeAt(i);
    }
    const byteArray = new Uint8Array(byteNumbers);
    return new Blob([byteArray], { type: contentType });
  }


  addDocumentTrail(id: string, operation: string) {
    const objDocumentAuditTrail: DocumentAuditTrail = {
      documentId: id,
      operationName: operation,
    };
    this.documentStore.addDocumentAudit(objDocumentAuditTrail);

  }

  private downloadFile() {
    const a = document.createElement('a');
    a.setAttribute('style', 'display:none;');
    document.body.appendChild(a);
    a.download = this.data.name;
    a.href = URL.createObjectURL(this.documentUrl);
    a.target = '_blank';
    a.click();
    document.body.removeChild(a);
  }

  showComment(comment: string) {
    const dialogRef = this.dialog.open(UploadNewVersionCommentComponent, {
      width: '800px',
      maxHeight: '70vh',
      data: comment,
    });
  }
}

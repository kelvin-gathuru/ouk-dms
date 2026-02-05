import { HttpEvent, HttpEventType } from '@angular/common/http';
import { Component, inject, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog, MatDialogModule } from '@angular/material/dialog';
import { CommonDialogService } from '@core/common-dialog/common-dialog.service';
import { DocumentAuditTrail } from '@core/domain-classes/document-audit-trail';
import { DocumentInfo } from '@core/domain-classes/document-info';
import { DocumentOperation } from '@core/domain-classes/document-operation';
import { DocumentView } from '@core/domain-classes/document-view';
import { DocumentVersion } from '@core/domain-classes/documentVersion';
import { CommonService } from '@core/services/common.service';
import { OverlayPanel } from '@shared/overlay-panel/overlay-panel.service';
import { ToastrService } from '@core/services/toastr-service';
import { BaseComponent } from '../../base.component';
import { DocumentService } from '../document.service';
import { UploadNewVersionCommentComponent } from '../document-upload-new-version/upload-new-version-comment/upload-new-version-comment.component';
import { DocumentChunk } from '@core/domain-classes/document-chunk';
import { DocumentChunkDownload } from '@core/domain-classes/document-chunk-download';
import { bufferCount, concatMap, from, mergeMap } from 'rxjs';
import { DocumentStore } from '../document-list/document-store';
import { FoldersViewStore } from '../folders-view/folders-view-store';
import { SecurityService } from '@core/security/security.service';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { HasClaimDirective } from '@shared/has-claim.directive';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { TranslateModule } from '@ngx-translate/core';
import { LimitToPipe } from '@shared/pipes/limit-to.pipe';
import { UTCToLocalTime } from '@shared/pipes/utc-to-localtime.pipe';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-document-version-history',
  templateUrl: './document-version-history.component.html',
  styleUrls: ['./document-version-history.component.scss'],
  standalone: true,
  imports: [
    PageHelpTextComponent,
    HasClaimDirective,
    MatDialogModule,
    MatProgressSpinnerModule,
    MatIconModule,
    TranslateModule,
    LimitToPipe,
    UTCToLocalTime,
    MatButtonModule,
    MatCardModule
  ]
})
export class DocumentVersionHistoryComponent
  extends BaseComponent
  implements OnInit {
  isLoadingResults = false;
  documentVersions: DocumentVersion[] = [];
  documentUrl: Blob;
  documentChunks: DocumentChunk[] = [];
  contentType: string = '';
  documentChunkDownloads: DocumentChunkDownload[] = [];
  isLoading: boolean = false;
  documentStore = inject(DocumentStore);
  foldersViewStore = inject(FoldersViewStore);
  progress: number = 0;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: DocumentInfo,
    private overlay: OverlayPanel,
    public dialogRef: MatDialogRef<DocumentVersionHistoryComponent>,
    private toastrService: ToastrService,
    private documentService: DocumentService,
    private commonService: CommonService,
    private securityService: SecurityService,
    private dialog: MatDialog,
    private commandDialogService: CommonDialogService
  ) {
    super();
  }

  ngOnInit(): void { }

  closeDialog() {
    this.dialogRef.close();
  }

  async onDocumentView(documentVersion: DocumentVersion) {
    try {
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
            .restoreDocumentVersion(this.data.id ?? '', version.id ?? '')
            .subscribe(() => {
              this.toastrService.success(
                this.translationService.getValue(
                  'VERSION_RESTORED_SUCCESSFULLY'
                )
              );
              this.addDocumentTrail(
                this.data.id ?? '',
                DocumentOperation.Restored.toString()
              );
              this.documentStore.loadDocuments();
              if (this.foldersViewStore.selectedCategoryId() == this.data.categoryId) {
                this.foldersViewStore.setDocumentsEmpty();
                this.foldersViewStore.loadDocumentsByCategory(this.foldersViewStore.selectedCategoryId())
              }
              this.dialogRef.close(true);
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
              this.data.id ?? '',
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
    this.isLoading = true;
    this.progress = 0;
    const { chunkSize1, parallelCalls } = this.commonService.getNetworkSpeed();
    const chunkRequests = [];
    for (let i = 0; i < this.documentChunks.length; i++) {
      chunkRequests.push({ chunkIndex: i, documentVersionId: this.documentChunks[i].documentVersionId });
    }
    this.sub$.sink = from(chunkRequests)
      .pipe(
        bufferCount(parallelCalls), // Group requests into batches of 5
        concatMap((batch) =>
          from(batch).pipe(
            mergeMap((chunk) => this.downloadChunk(chunk.chunkIndex, chunk.documentVersionId), parallelCalls) // Retrieve 5 chunks in parallel
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
          this.progress = 0;
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
    a.download = this.data.name ?? '';
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

import { SelectionModel } from '@angular/cdk/collections';
import { AfterViewInit, Component, OnInit } from '@angular/core';
import { DocumentInfo } from '@core/domain-classes/document-info';
import { BaseComponent } from '../../base.component';
import { DocumentService } from '../document.service';
import { CommonDialogService } from '@core/common-dialog/common-dialog.service';
import { MatDialog } from '@angular/material/dialog';
import { OverlayPanel } from '@shared/overlay-panel/overlay-panel.service';
import { ClonerService } from '@core/services/clone.service';
import { CommonService } from '@core/services/common.service';
import { ToastrService } from '@core/services/toastr-service';
import { DocumentOperation } from '@core/domain-classes/document-operation';
import { DocumentCategoryStatus } from '@core/domain-classes/document-category';
import { DocumentCommentComponent } from '../document-comment/document-comment.component';
import { DocumentPermissionListComponent } from '../document-permission/document-permission-list/document-permission-list.component';
import { DocumentPermissionMultipleComponent } from '../document-permission/document-permission-multiple/document-permission-multiple.component';
import { DocumentUploadNewVersionComponent } from '../document-upload-new-version/document-upload-new-version.component';
import { DocumentView } from '@core/domain-classes/document-view';
import { HttpEvent, HttpEventType } from '@angular/common/http';
import { DocumentAuditTrail } from '@core/domain-classes/document-audit-trail';
import { SendEmailComponent } from '../send-email/send-email.component';
import { DocumentReminderComponent } from '../document-reminder/document-reminder.component';
import { DocumentVersion } from '@core/domain-classes/documentVersion';
import { DocumentVersionHistoryComponent } from '../document-version-history/document-version-history.component';
import { DocumentShareableLink } from '@core/domain-classes/document-shareable-link';
import { DocumentSharedLinkComponent } from '../document-shared-link/document-shared-link.component';
import { DocumentEditComponent } from '../document-edit/document-edit.component';
import { DocumentChunk } from '@core/domain-classes/document-chunk';
import { bufferCount, concatMap, from, mergeMap } from 'rxjs';
import { DocumentChunkDownload } from '@core/domain-classes/document-chunk-download';
import { SharePermissionComponent } from '../document-permission/share-permission/share-permission.component';
import { SecurityService } from '@core/security/security.service';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { FormsModule } from '@angular/forms';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { HasClaimDirective } from '@shared/has-claim.directive';
import { TranslateModule } from '@ngx-translate/core';
import { MatTableModule } from '@angular/material/table';
import { StorageTypePipe } from '../../storage-setting/storage-type.pipe';
import { MatBadgeModule } from '@angular/material/badge';
import { MatMenuModule } from '@angular/material/menu';
import { MatTooltipModule } from '@angular/material/tooltip';
import { UTCToLocalTime } from '@shared/pipes/utc-to-localtime.pipe';
import { MatCardModule } from '@angular/material/card';
import { NgStyle } from '@angular/common';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-deep-document-search',
  templateUrl: './deep-document-search.component.html',
  styleUrl: './deep-document-search.component.scss',
  standalone: true,
  imports: [
    PageHelpTextComponent,
    FormsModule,
    MatCheckboxModule,
    MatButtonModule,
    MatIconModule,
    HasClaimDirective,
    TranslateModule,
    MatTableModule,
    StorageTypePipe,
    MatBadgeModule,
    MatMenuModule,
    MatTooltipModule,
    UTCToLocalTime,
    MatCardModule,
    NgStyle,
    MatProgressSpinnerModule
  ]
})
export class DeepDocumentSearchComponent
  extends BaseComponent
  implements OnInit, AfterViewInit {
  displayedColumns: string[] = [
    'select',
    'action',
    'documentNumber',
    'versionNumber',
    'name',
    'isSigned',
    'signDate',
    'categoryName',
    'documentStatus',
    'storageType',
    'createdDate',
    'createdBy',
  ];
  footerToDisplayed = ['footer'];
  isLoadingResults = false;
  dataSource: DocumentInfo[] = [];
  searchQuery: string = '';
  isExactMatch: boolean = false;
  documentChunks: DocumentChunk[] = [];
  selectedDocument: DocumentInfo;
  documentChunkDownloads: DocumentChunkDownload[] = [];
  contentType: string;

  selection = new SelectionModel<DocumentInfo>(true, []);
  constructor(
    private documentService: DocumentService,
    private commonDialogService: CommonDialogService,
    private dialog: MatDialog,
    public overlay: OverlayPanel,
    public clonerService: ClonerService,
    private securityService: SecurityService,
    private commonService: CommonService,
    private toastrService: ToastrService
  ) {
    super();
  }

  ngOnInit(): void { }

  ngAfterViewInit() { }

  /** Whether the number of selected elements matches the total number of rows. */
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.length;
    return numSelected === numRows;
  }
  /** Selects all rows if they are not all selected; otherwise clear selection. */
  masterToggle() {
    this.isAllSelected()
      ? this.selection.clear()
      : this.dataSource.forEach((row) => this.selection.select(row));
  }

  searchDocuments(): void {
    if (this.searchQuery) {
      this.isLoadingResults = true;
      let searchQuery = this.searchQuery.trim();
      if (this.isExactMatch) {
        searchQuery = `"${searchQuery}"`;
      }
      this.sub$.sink = this.documentService
        .getDocumentsByDeepSearch(searchQuery)
        .subscribe({
          next: (resp: DocumentInfo[]) => {
            this.isLoadingResults = false;
            if (resp.length > 0) {
              this.dataSource = [...resp];
            } else {
              this.dataSource = [];
              this.toastrService.info(
                `${this.translationService.getValue('NO_DATA_FOUND')}`
              );
            }
          },
          error: (error) => {
            this.dataSource = [];
            this.isLoadingResults = false;
          },
        });
    }
  }

  removeDocumentPageIndexing(document: DocumentInfo) {
    this.sub$.sink = this.commonDialogService
      .deleteConfirmtionDialog(
        `${this.translationService.getValue(
          'ARE_YOU_SURE_WANT_TO_REMOVE_DOCUMENT_PAGE_INDEXING'
        )} :: ${document.name}`,
        this.translationService.getValue('DEEP_SEARCH_REMOVE_NOTE')
      )
      .subscribe((isTrue: boolean) => {
        if (isTrue) {
          this.sub$.sink = this.documentService
            .removePageIndexing(document.id ?? '')
            .subscribe((c: boolean) => {
              if (c) {
                this.searchDocuments();
                this.toastrService.success(
                  this.translationService.getValue(
                    'DOCUMENT_PAGE_INDEXING_IS_DELETED'
                  )
                );
              }
            });
        }
      });
  }

  editDocument(documentInfo: DocumentInfo) {
    const documentCategories: DocumentCategoryStatus = {
      document: documentInfo,
      categories: [],
      documentStatuses: [],
      clients: [],
    };
    const dialogRef = this.dialog.open(DocumentEditComponent, {
      width: '60vw',
      data: Object.assign({}, documentCategories),
    });

    this.sub$.sink = dialogRef.afterClosed().subscribe((result: string) => {
      if (result === 'loaded') {
      }
    });
  }

  addComment(document: Document) {
    const dialogRef = this.dialog.open(DocumentCommentComponent, {
      width: '800px',
      maxHeight: '70vh',
      data: Object.assign({}, document),
    });

    this.sub$.sink = dialogRef.afterClosed().subscribe((result: string) => {
      if (result === 'loaded') {
      }
    });
  }

  manageSharePermission(documentInfo: DocumentInfo) {
    const screenWidth = window.innerWidth;
    const dialogWidth = screenWidth < 768 ? '80vw' : '58vw';
    const dialogRef = this.dialog.open(SharePermissionComponent, {
      data: documentInfo,
      width: dialogWidth,
      maxHeight: '80vh',
    });

    this.sub$.sink = dialogRef.afterClosed().subscribe((result: string) => {
      if (result === 'loaded') {
      }
    });
  }

  manageDocumentPermission(documentInfo: DocumentInfo) {
    this.dialog.open(DocumentPermissionListComponent, {
      data: documentInfo,
      width: '80vw',
      maxHeight: '80vh',
    });
  }
  onSharedSelectDocument() {
    const dialogRef = this.dialog.open(DocumentPermissionMultipleComponent, {
      data: this.selection.selected,
      width: '100%',
      maxWidth: '50vw',
    });
    dialogRef.afterClosed().subscribe(() => {
      this.selection.clear();
    });
  }

  uploadNewVersion(document: Document) {
    const dialogRef = this.dialog.open(DocumentUploadNewVersionComponent, {
      width: '800px',
      maxHeight: '70vh',
      data: Object.assign({}, document),
    });

    this.sub$.sink = dialogRef
      .afterClosed()
      .subscribe((isNewVersionUploaded: boolean) => {
        if (isNewVersionUploaded) {
        }
      });
  }

  downloadDocument(documentInfo: DocumentInfo) {
    if (documentInfo.isChunk) {
      this.selectedDocument = documentInfo;
      this.getAllDocumentChunks();
    } else {
      const docuView: DocumentView = {
        documentId: documentInfo.id,
        name: '',
        extension: documentInfo.url?.split('.')[1],
        isVersion: false,
        isFromPublicPreview: false,
        isPreviewDownloadEnabled: false,
        isFileRequestDocument: false,
        isSignatureExists: false,
        documentVersionId: documentInfo.documentVersionId,
        documentNumber: documentInfo.documentNumber,
      };
      this.sub$.sink = this.commonService.downloadDocument(docuView).subscribe({
        next: (event: HttpEvent<Blob>) => {
          if (event.type === HttpEventType.Response) {
            this.addDocumentTrail(
              documentInfo.id ?? '',
              DocumentOperation.Download.toString()
            );
            if (event.body) {
              const downloadedFile = new Blob([event.body], {
                type: event.body.type,
              });
              this.downloadFile(downloadedFile);
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
        },
      });
    }
  }

  private getAllDocumentChunks() {
    this.sub$.sink = this.commonService
      .getDocumentChunks(this.selectedDocument.documentVersionId ?? '')
      .subscribe({
        next: (data) => {
          this.documentChunks = data;
          if (this.documentChunks.length > 0) {
            this.startDownload();
          }
        },
        error: (err) => {
          this.toastrService.error(err.error.message);
        },
      });
  }

  addDocumentTrail(id: string, operation: string) {
    const objDocumentAuditTrail: DocumentAuditTrail = {
      documentId: id,
      operationName: operation,
    };

    this.sub$.sink = this.commonService
      .addDocumentAuditTrail(objDocumentAuditTrail)
      .subscribe((c) => { });
  }

  sendEmail(documentInfo: DocumentInfo) {
    this.dialog.open(SendEmailComponent, {
      data: documentInfo,
      width: '100%',
      maxWidth: '50vw'
    });
  }

  addReminder(documentInfo: DocumentInfo) {
    this.dialog.open(DocumentReminderComponent, {
      data: documentInfo,
      width: '100%',
      maxWidth: '50vw'
    });
  }

  async onDocumentView(document: DocumentInfo) {
    this.isLoadingResults = true;
    try {
      const urls = document.url?.split('.') ?? [];
      const extension = document.extension ? document.extension : urls[1];
      const documentView: DocumentView = {
        id: document.id,
        documentId: document.id,
        name: document.name,
        extension: extension,
        isVersion: (document.versionNumber ?? 1) > 1,
        isFromPublicPreview: false,
        isPreviewDownloadEnabled: this.securityService.hasClaim('ALL_DOWNLOAD_DOCUMENT'),
        isFileRequestDocument: false,
        isSignatureExists: false,
        url: document.url,
        documentVersionId: document.documentVersionId,
        isChunk: document.isChunk,
        documentNumber: document.documentNumber,
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
      if (extension === 'pdf') {
        this.sub$.sink = this.overlay.isClosePanelClose$.subscribe((c: boolean) => {
          if (c && extension === 'pdf') {
            this.searchDocuments();
          }
        });
      }
    }
    finally {
      this.isLoadingResults = false;
    }
  }

  onVersionHistoryClick(document: DocumentInfo): void {
    let documentInfo = this.clonerService.deepClone<DocumentInfo>(document);
    this.sub$.sink = this.documentService
      .getDocumentVersion(document.id ?? '')
      .subscribe((documentVersions: DocumentVersion[]) => {
        documentInfo.documentVersions = documentVersions;
        const dialogRef = this.dialog.open(DocumentVersionHistoryComponent, {
          maxWidth: '80vw',
          panelClass: 'full-width-dialog',
          data: Object.assign({}, documentInfo),
        });
        dialogRef.afterClosed().subscribe((result: boolean) => {
          if (result) {
          }
        });
      });
  }

  onCreateShareableLink(document: DocumentInfo): void {
    this.sub$.sink = this.documentService
      .getDocumentShareableLink(document.id ?? '')
      .subscribe((link: DocumentShareableLink) => {
        this.dialog.open(DocumentSharedLinkComponent, {
          width: '500px',
          data: { document, link },
        });
      });
  }

  private startDownload() {
    const { chunkSize1, parallelCalls } = this.commonService.getNetworkSpeed();
    const chunkRequests = [];
    for (let i = 0; i < this.documentChunks.length; i++) {
      chunkRequests.push({
        chunkIndex: i,
        documentVersionId: this.documentChunks[i].documentVersionId,
      });
    }
    this.sub$.sink = from(chunkRequests)
      .pipe(
        bufferCount(parallelCalls), // Group requests into batches of 5
        concatMap((batch) =>
          from(batch).pipe(
            mergeMap(
              (chunk) =>
                this.downloadChunk(chunk.chunkIndex, chunk.documentVersionId),
              parallelCalls
            ) // Retrieve 5 chunks in parallel
          )
        )
      )
      .subscribe({
        next: (documentChunkDownload: DocumentChunkDownload) => {
          this.contentType = documentChunkDownload.contentType;
          const chunkBlob = this.base64ToBlob(
            documentChunkDownload.data,
            documentChunkDownload.contentType
          );
          documentChunkDownload.blobChunk = chunkBlob;
          this.documentChunkDownloads.push(documentChunkDownload);
        },
        complete: () => this.mergeChunks(),
        error: (err) => {
          this.toastrService.error(
            this.translationService.getValue('ERROR_WHILE_DOWNLOADING_DOCUMENT')
          );
          console.error('Error downloading chunks', err);
        },
      });
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

  private downloadChunk(chunkIndex: number, documentVersionId: string) {
    return this.commonService.downloadDocumentChunk(
      documentVersionId,
      chunkIndex
    );
  }

  private mergeChunks() {
    const sortedChunks = this.documentChunkDownloads
      .sort((a, b) => a.chunkIndex - b.chunkIndex)
      .map((entry) => entry.blobChunk);
    const validChunks = sortedChunks.filter((chunk): chunk is Blob => !!chunk);
    const blob = new Blob(validChunks, { type: this.contentType });
    this.addDocumentTrail(
      this.selectedDocument.id ?? '',
      DocumentOperation.Download.toString()
    );
    this.downloadFile(blob);
  }

  private downloadFile(data: Blob) {
    const a = document.createElement('a');
    a.setAttribute('style', 'display:none;');
    document.body.appendChild(a);
    a.download = this.selectedDocument.name ?? 'downloaded-file';
    a.href = URL.createObjectURL(data);
    a.target = '_blank';
    a.click();
    document.body.removeChild(a);
  }
}

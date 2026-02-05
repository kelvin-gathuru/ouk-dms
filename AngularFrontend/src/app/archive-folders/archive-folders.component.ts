import { Component, HostListener, inject, OnInit } from "@angular/core";
import { BaseComponent } from "../base.component";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatDialog, MatDialogModule } from "@angular/material/dialog";
import { MatSortModule, Sort } from "@angular/material/sort";
import { MatTooltipModule } from "@angular/material/tooltip";
import { MatProgressSpinnerModule, ProgressSpinnerMode } from "@angular/material/progress-spinner";
import { MatIconModule } from "@angular/material/icon";
import { MatButtonModule } from "@angular/material/button";
import { TranslateModule } from "@ngx-translate/core";
import { ImagePathPipe } from "../document/folders-view/image-path.pipe";
import { MatMenuModule } from "@angular/material/menu";
import { ActivatedRoute, RouterModule } from "@angular/router";
import { WorkflowCombinePipe } from "../document/folders-view/workflow-combine.pipe";
import { FoldersViewStore } from "../document/folders-view/folders-view-store";
import { OverlayPanel } from "@shared/overlay-panel/overlay-panel.service";
import { DocumentStore } from "../document/document-list/document-store";
import { CategoryService } from "@core/services/category.service";
import { Category } from "@core/domain-classes/category";
import { ClonerService } from "@core/services/clone.service";
import { DocumentStatus } from "../document-status/document-status";
import { DocumentStatusService } from "../document-status/document-status.service";
import { ClientStore } from "../client/client-store";
import { DocumentService } from "../document/document.service";
import { DocumentInfo } from "@core/domain-classes/document-info";
import { CommonService } from "@core/services/common.service";
import { ToastrService } from '@core/services/toastr-service';

import { DocumentChunk } from "@core/domain-classes/document-chunk";
import { DocumentChunkDownload } from "@core/domain-classes/document-chunk-download";
import { CommonDialogService } from "@core/common-dialog/common-dialog.service";
import { ArchiveDocumentsStore } from "../archive-documents/archive-documents-store";
import { DocumentView } from "@core/domain-classes/document-view";
import { DocumentShareableLink } from "@core/domain-classes/document-shareable-link";
import { DocumentSharedLinkComponent } from "../document/document-shared-link/document-shared-link.component";
import { DocumentAuditTrail } from "@core/domain-classes/document-audit-trail";
import { HttpEvent, HttpEventType } from "@angular/common/http";
import { DocumentOperation } from "@core/domain-classes/document-operation";
import { bufferCount, concatMap, from, mergeMap } from "rxjs";
import { DocumentVersion } from "@core/domain-classes/documentVersion";
import { DocumentVersionHistoryComponent } from "../document/document-version-history/document-version-history.component";
import { SendEmailComponent } from "../document/send-email/send-email.component";
import { ArchiveFoldersViewStore } from "./archive-folders-view-store";
import { AssignFoldersViewStore } from "../document-library/folders-view/assign-folders-view-store";
import { DocumentResource } from "@core/domain-classes/document-resource";
import { SecurityService } from "../core/security/security.service";
import { HasClaimDirective } from "@shared/has-claim.directive";
import { PageHelpTextComponent } from "@shared/page-help-text/page-help-text.component";
import { UTCToLocalTime } from "@shared/pipes/utc-to-localtime.pipe";
import { MatCardModule } from "@angular/material/card";

@Component({
  selector: 'app-archive-folders',
  imports: [
    ReactiveFormsModule,
    MatDialogModule,
    MatSortModule,
    MatTooltipModule,
    FormsModule,
    MatProgressSpinnerModule,
    MatIconModule,
    MatButtonModule,
    TranslateModule,
    ImagePathPipe,
    MatMenuModule,
    RouterModule,
    MatTooltipModule,
    WorkflowCombinePipe,
    HasClaimDirective,
    PageHelpTextComponent,
    UTCToLocalTime,
    MatCardModule,
    MatIconModule,
    MatButtonModule
  ],
  templateUrl: './archive-folders.component.html',
  styleUrl: './archive-folders.component.scss'
})
export class ArchiveFoldersComponent extends BaseComponent implements OnInit {
  foldersViewStore = inject(FoldersViewStore);
  assignFoldersViewStore = inject(AssignFoldersViewStore);
  activatedRoute = inject(ActivatedRoute);
  overlay = inject(OverlayPanel);
  dialog = inject(MatDialog);
  documentStore = inject(DocumentStore);
  categoryService = inject(CategoryService);
  categories: Category[] = [];
  allCategories: Category[] = [];
  clonerService = inject(ClonerService);
  documentStatuses: DocumentStatus[] = [];
  documentStatusService = inject(DocumentStatusService);
  securityService = inject(SecurityService);
  clientStore = inject(ClientStore);
  documentService = inject(DocumentService);
  selectedDocument: DocumentInfo;
  commonService = inject(CommonService);
  toastrService = inject(ToastrService);
  documentChunks: DocumentChunk[] = [];
  contentType: string = '';
  documentChunkDownloads: DocumentChunkDownload[] = [];
  commonDialogService = inject(CommonDialogService);
  archiveDocumentsStore = inject(ArchiveDocumentsStore);
  archiveFoldersViewStore = inject(ArchiveFoldersViewStore);
  isLoadingResults = false;
  mode: ProgressSpinnerMode = 'indeterminate';
  documentResource: DocumentResource = this.documentStore.documentResourceParameter();

  @HostListener('window:scroll', [])
  onScroll(): void {
    if (
      window.innerHeight + window.scrollY >=
      document.body.offsetHeight - 100
    ) {
      this.loadDocument();
    }
  }

  trackByFn(index: number, item: DocumentInfo): string {
    return item.id ? item.id : ''; // Unique ID for tracking
  }
  trackByCategoryPath(index: number, item: any) {
    return item.id ? item.id : '';
  }

  ngOnInit(): void {
    if (this.archiveFoldersViewStore.categories().length === 0) {
      this.archiveFoldersViewStore.loadCategoriesById('');
    }
    this.getCategories();
    this.getDocumentStatus();
  }

  getDocumentStatus() {
    this.documentStatusService
      .getDocumentStatuss()
      .subscribe((c: DocumentStatus[]) => {
        if (c && c.length > 0) {
          this.documentStatuses = [...c];
        }
      });
  }

  getCategories(): void {
    this.categoryService.getAllCategories().subscribe((c) => {
      this.categories = [...c];
      const categories = this.categories.filter((c) => c.parentId == null);
      categories.forEach((category: Category, index: number) => {
        category.deafLevel = 0;
        category.index = index * Math.pow(0.1, category.deafLevel);
        category.displayName = category.name;
        this.allCategories.push(category);
        this.setDeafLevel(category);
      });
      this.allCategories = this.clonerService.deepClone(this.allCategories);
      // if (this.documentResource.categoryId) {
      //   this.selectedCategory = this.allCategories.find(
      //     (c) => c.id === this.documentResource.categoryId
      //   );
      // }
    });
  }

  setDeafLevel(parent?: Category) {
    if (parent?.children && parent.children.length > 0) {
      parent.children.map((c, index) => {
        c.displayName = parent.displayName + ' > ' + c.name;
        c.deafLevel = parent?.deafLevel ? parent?.deafLevel + 1 : 0;
        c.index =
          (parent?.index ? parent.index : 0) +
          index * Math.pow(0.1, c.deafLevel);
        this.allCategories.push(c);
        this.setDeafLevel(c);
      });
    }
    return parent;
  }

  onFolderClick(category: Category) {
    if (category.id === this.archiveFoldersViewStore.selectedCategoryId()) {
      return;
    }
    const documentResource = this.archiveFoldersViewStore.documentResourceParameter();
    documentResource.categoryId = category.id;
    this.archiveFoldersViewStore.loadCategoriesById(category.id ? category.id : '');
    this.archiveFoldersViewStore.setDocumentsEmpty();
    documentResource.skip = 0;
    this.archiveFoldersViewStore.loadByQuery(documentResource);
    this.archiveFoldersViewStore.addCategoryPath(category);
  }

  onFolderClickFromPath(category: Category) {
    this.archiveFoldersViewStore.removeCategoryPath(category);
    const documentResource = this.archiveFoldersViewStore.documentResourceParameter();
    documentResource.categoryId = category.id;
    this.archiveFoldersViewStore.loadCategoriesById(category.id ? category.id : '');
    this.archiveFoldersViewStore.setDocumentsEmpty();
    documentResource.skip = 0;
    this.archiveFoldersViewStore.loadByQuery(documentResource);
  }

  async onDocumentView(document: DocumentInfo) {
    let extension = document.url?.split('.')[1];
    const documentView: DocumentView = {
      documentId: document.id,
      name: document.name,
      url: document.url,
      extension: document.extension ? document.extension : extension,
      isVersion: false,
      isFromPublicPreview: false,
      isPreviewDownloadEnabled: this.securityService.hasClaim('archive_documents_download_document'),
      isFileRequestDocument: false,
      isSignatureExists: document.isSignatureExists,
      isChunk: document.isChunk,
      documentNumber: document.documentNumber,
    };
    const { BasePreviewComponent } = await import(
      '../shared/base-preview/base-preview.component'
    );
    this.overlay.open(BasePreviewComponent, {
      position: 'center',
      origin: 'global',
      panelClass: ['file-preview-overlay-container', 'white-background'],
      data: documentView,
    });
  }

  sortData(sort: Sort) {
    if (sort.active) {
      if (sort.active == "name" && this.archiveFoldersViewStore.documents().length == 0 && this.archiveFoldersViewStore.categories().length > 0) {
        const documentResource = this.archiveFoldersViewStore.documentResourceParameter();
        documentResource.orderBy = `${sort.active} ${sort.direction}`;
        this.archiveFoldersViewStore.setDocumentResource(documentResource);
      } else if (
        this.archiveFoldersViewStore.documentResourceParameter().totalCount ==
        this.archiveFoldersViewStore.documents().length) {
        const documentResource = this.archiveFoldersViewStore.documentResourceParameter();
        documentResource.orderBy = `${sort.active} ${sort.direction}`;
        this.archiveFoldersViewStore.setDocumentResource(documentResource);
      }
      else {
        this.archiveFoldersViewStore.setDocumentsEmpty();
        const documentResource = this.archiveFoldersViewStore.documentResourceParameter();
        documentResource.orderBy = `${sort.active} ${sort.direction}`;
        documentResource.skip = 0;
        this.archiveFoldersViewStore.loadByQuery(documentResource);
      }
    }
  }

  loadDocument() {
    if (this.archiveFoldersViewStore.isLoading()) {
      return;
    }
    if (
      this.archiveFoldersViewStore.documentResourceParameter().totalCount ==
      this.archiveFoldersViewStore.documents().length
    ) {
      return;
    }
    const documentResource = this.archiveFoldersViewStore.documentResourceParameter();
    documentResource.categoryId =
      this.archiveFoldersViewStore.selectedCategoryId();
    documentResource.skip = this.archiveFoldersViewStore.documents().length;
    this.archiveFoldersViewStore.loadByQuery(documentResource);
  }

  onHomeClick() {
    this.archiveFoldersViewStore.setDocumentsEmpty();
    this.archiveFoldersViewStore.removeCategoryPath(null);
    this.archiveFoldersViewStore.loadCategoriesById('');
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

  addDocumentTrail(id: string, operation: string) {
    const objDocumentAuditTrail: DocumentAuditTrail = {
      documentId: id,
      operationName: operation,
    };
    this.documentStore.addDocumentAudit(objDocumentAuditTrail);
  }

  downloadDocument(documentInfo: DocumentInfo) {
    this.selectedDocument = documentInfo;
    this.documentStore.setLoadingFlag(true);
    if (documentInfo.isChunk) {
      this.getAllDocumentChunks();
    } else {
      let extension = documentInfo.extension;
      if (!extension?.includes('.')) {
        extension = '.' + extension;
      }
      const docuView: DocumentView = {
        documentId: documentInfo.id,
        name: '',
        extension: documentInfo.extension
          ? extension
          : documentInfo.url?.split('.')[1],
        isVersion: false,
        isFromPublicPreview: false,
        isPreviewDownloadEnabled: false,
        isFileRequestDocument: false,
        isSignatureExists: false,
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
      .getDocumentChunks(this.selectedDocument.id ?? '')
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
          this.documentStore.setLoadingFlag(false);
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
      .map((entry) => entry.blobChunk)
      .filter((chunk): chunk is Blob => chunk !== undefined);
    const blob = new Blob(sortedChunks, { type: this.contentType });
    this.documentStore.setLoadingFlag(false);
    this.addDocumentTrail(
      this.selectedDocument.id ?? '',
      DocumentOperation.Download.toString()
    );
    this.downloadFile(blob);
  }

  private downloadFile(data: Blob) {
    this.documentStore.setLoadingFlag(false);
    // const downloadedFile = new Blob([data.body], { type: data.body.type });
    const a = document.createElement('a');
    a.setAttribute('style', 'display:none;');
    document.body.appendChild(a);
    a.download = this.selectedDocument.name;
    a.href = URL.createObjectURL(data);
    a.target = '_blank';
    a.click();
    document.body.removeChild(a);
  }

  onVersionHistoryClick(document: DocumentInfo): void {
    let documentInfo = this.clonerService.deepClone<DocumentInfo>(document);
    this.sub$.sink = this.documentService
      .getDocumentVersion(document.id ?? '')
      .subscribe((documentVersions: DocumentVersion[]) => {
        documentInfo.documentVersions = documentVersions;
        const dialogRef = this.dialog.open(DocumentVersionHistoryComponent, {
          maxWidth: '95vw',
          panelClass: 'full-width-dialog',
          data: Object.assign({}, documentInfo),
        });
        dialogRef.afterClosed().subscribe((result: boolean) => {
          if (result) {
            this.documentStore.loadDocuments();
            if (this.archiveFoldersViewStore.selectedCategoryId() == document.categoryId) {
              this.archiveFoldersViewStore.setDocumentsEmpty();
              this.archiveFoldersViewStore.loadDocumentsByCategory(this.archiveFoldersViewStore.selectedCategoryId())
            }
          }
        });
      });
  }


  sendEmail(documentInfo: DocumentInfo) {
    this.dialog.open(SendEmailComponent, {
      data: documentInfo,
      width: '80vw',
      maxHeight: '80vh',
    });
  }

  archiveDocument(document: DocumentInfo) {
    this.sub$.sink = this.commonDialogService
      .deleteConfirmtionDialog(
        `${this.translationService.getValue(
          'ARE_YOU_SURE_YOU_WANT_TO_ARCHIVE'
        )} ${document.name}`
      )
      .subscribe((isTrue: boolean) => {
        if (isTrue) {
          this.sub$.sink = this.documentService
            .archiveDocument(document.id ?? '')
            .subscribe(() => {
              this.addDocumentTrail(
                document.id ?? '',
                DocumentOperation.Archived.toString()
              );
              this.toastrService.success(
                this.translationService.getValue(
                  'DOCUMENT_ARCHIVE_SUCCESSFULLY'
                )
              );
              this.archiveFoldersViewStore.setDocumentsEmpty();
              this.archiveFoldersViewStore.loadDocumentsByCategory(this.archiveFoldersViewStore.selectedCategoryId())
              this.archiveDocumentsStore.getArchiveDocuments();
            });
        }
      });
  }

  restoreFolder(category: Category) {
    this.sub$.sink = this.commonDialogService
      .deleteConfirmtionDialog(
        `${this.translationService.getValue(
          'ARE_YOU_SURE_YOU_WANT_TO_RESTORE'
        )} ${category.name}`
      )
      .subscribe((isTrue: boolean) => {
        if (isTrue) {
          this.sub$.sink = this.categoryService
            .restoreFolder(category.id ?? '')
            .subscribe({
              next: () => {
                this.toastrService.success(
                  this.translationService.getValue(
                    'CATEGORY_RESTORE_SUCCESSFULLY'
                  )
                );
                this.archiveFoldersViewStore.loadCategoriesById(this.archiveFoldersViewStore.selectedCategoryId());

                if (this.foldersViewStore.selectedCategoryId() == category.id || (this.foldersViewStore.selectedCategoryId() == '' && category.parentId == null)) {
                  this.foldersViewStore.loadCategoriesById(this.foldersViewStore.selectedCategoryId());
                  this.foldersViewStore.loadDocuments();
                }
                if (this.assignFoldersViewStore.selectedCategoryId() == category.id || (this.assignFoldersViewStore.selectedCategoryId() == '' && category.parentId == null)) {
                  this.assignFoldersViewStore.loadCategoriesById(this.assignFoldersViewStore.selectedCategoryId());
                  this.assignFoldersViewStore.loadDocuments();
                }
                this.archiveDocumentsStore.loadByQuery(this.archiveDocumentsStore.documentResourceParameter());
                this.documentStore.loadDocuments();
              },
              error: (err) => {
                let errorMsg = this.translationService.getValue('ERROR_WHILE_RESTORING_CATEGORY');
                if (err?.error?.errors && Array.isArray(err.error.errors) && err.error.errors.length > 0) {
                  errorMsg = err.error.errors.join(', ');
                }
                this.toastrService.error(errorMsg);
              }
            });
        }
      });
  }

  deleteDocument(document: DocumentInfo) {
    this.sub$.sink = this.commonDialogService
      .deleteConfirmtionDialog(
        `${this.translationService.getValue(
          'ARE_YOU_SURE_YOU_WANT_TO_DELETE'
        )} ${document.name}`
      )
      .subscribe((isTrue: boolean) => {
        if (isTrue) {
          this.sub$.sink = this.documentService
            .deleteDocument(document.id ?? '')
            .subscribe(() => {
              this.addDocumentTrail(
                document.id ?? '',
                DocumentOperation.Deleted.toString()
              );
              this.toastrService.success(
                this.translationService.getValue(
                  'DOCUMENT_DELETED_SUCCESSFULLY'
                )
              );
              this.archiveDocumentsStore.loadByQuery(this.archiveDocumentsStore.documentResourceParameter());
              this.archiveFoldersViewStore.loadByQuery(this.archiveFoldersViewStore.documentResourceParameter());
              // this.dataSource.loadDocuments(this.documentResource);
            });
        }
      });
  }

  deleteFolder(category: Category) {
    this.commonDialogService
      .deleteConfirmtionDialog(
        this.translationService.getValue('ARE_YOU_SURE_YOU_WANT_TO_DELETE')
      )
      .subscribe((isTrue) => {
        if (isTrue) {
          this.categoryService.delete(category.id ?? '').subscribe(d => {
            this.toastrService.success(this.translationService.getValue(`CATEGORY_DELETED_SUCCESSFULLY`));
            this.archiveFoldersViewStore.loadCategoriesById(this.archiveFoldersViewStore.selectedCategoryId());
            this.archiveDocumentsStore.loadByQuery(this.archiveDocumentsStore.documentResourceParameter());
            this.archiveFoldersViewStore.loadByQuery(this.archiveFoldersViewStore.documentResourceParameter());
            this.getCategories();
          });
        }
      });
  }
  onRefresh() {
    this.archiveFoldersViewStore.setDocumentsEmpty();
    this.archiveFoldersViewStore.loadCategoriesById(
      this.archiveFoldersViewStore.selectedCategoryId()
    );
    this.archiveFoldersViewStore.loadDocuments();
  }
}

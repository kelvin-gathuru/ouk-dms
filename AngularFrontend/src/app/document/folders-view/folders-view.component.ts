import { Component, HostListener, inject, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSortModule, Sort } from '@angular/material/sort';
import { MatTooltipModule } from '@angular/material/tooltip';
import {
  MatProgressSpinnerModule,
  ProgressSpinnerMode,
} from '@angular/material/progress-spinner';
import { FoldersViewStore } from './folders-view-store';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { Category } from '../../core/domain-classes/category';
import { DocumentInfo } from '../../core/domain-classes/document-info';
import { DocumentView } from '../../core/domain-classes/document-view';
import { OverlayPanel } from '../../shared/overlay-panel/overlay-panel.service';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { TranslateModule } from '@ngx-translate/core';
import { ImagePathPipe } from './image-path.pipe';
import { DocumentCategoryStatus } from '@core/domain-classes/document-category';
import { DocumentEditComponent } from '../document-edit/document-edit.component';
import { DocumentStore } from '../document-list/document-store';
import { CategoryService } from '../../core/services/category.service';
import { ClonerService } from '../../core/services/clone.service';
import { DocumentStatusService } from '../../document-status/document-status.service';
import { DocumentStatus } from '../../document-status/document-status';
import { ClientStore } from '../../client/client-store';
import { WorkflowInstanceStatus } from '@core/domain-classes/workflow-instance-status.enum';
import { WorkflowInstance } from '@core/domain-classes/workflow-instance';
import { BaseComponent } from '../../base.component';
import { DocumentPermissionListComponent } from '../document-permission/document-permission-list/document-permission-list.component';
import { DocumentShareableLink } from '@core/domain-classes/document-shareable-link';
import { DocumentService } from '../document.service';
import { HttpEvent, HttpEventType } from '@angular/common/http';
import { CommonService } from '@core/services/common.service';
import { bufferCount, concatMap, from, mergeMap } from 'rxjs';
import { DocumentChunkDownload } from '@core/domain-classes/document-chunk-download';
import { DocumentOperation } from '@core/domain-classes/document-operation';

import { ToastrService } from '@core/services/toastr-service';
import { DocumentChunk } from '@core/domain-classes/document-chunk';
import { DocumentAuditTrail } from '@core/domain-classes/document-audit-trail';
import { DocumentVersion } from '@core/domain-classes/documentVersion';
import { DocumentCommentComponent } from '../document-comment/document-comment.component';
import { CommonDialogService } from '@core/common-dialog/common-dialog.service';
import { ArchiveDocumentsStore } from '../../archive-documents/archive-documents-store';
import { MatMenuModule } from '@angular/material/menu';
import { ManageCategoryComponent } from '../../category/manage-category/manage-category.component';
import { WorkflowCombinePipe } from './workflow-combine.pipe';
import { CategoryPermissionListComponent } from '../../document/category-permission/category-permission-list/category-permission-list.component';
import { ArchiveFoldersViewStore } from '../../archive-folders/archive-folders-view-store';
import { CategoryPermissionService } from '@core/services/category-permission.service';
import { DocumentMetaTagStore } from '../../document-meta-tag/document-meta-tag-store';
import { MatBadgeModule } from '@angular/material/badge';
import { DocumentCommentService } from '../document-comment/document-comment.service';
import { SharePermissionComponent } from '../document-permission/share-permission/share-permission.component';
import { SecurityService } from '@core/security/security.service';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { HasClaimDirective } from '@shared/has-claim.directive';
import { UTCToLocalTime } from '@shared/pipes/utc-to-localtime.pipe';
import { MatCardModule } from '@angular/material/card';
import { LimitToPipe } from '@shared/pipes/limit-to.pipe';
import { NgClass } from '@angular/common';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';


@Component({
  selector: 'app-folders-view',
  imports: [
    ReactiveFormsModule,
    MatDialogModule,
    MatSortModule,
    MatProgressSpinnerModule,
    MatIconModule,
    MatButtonModule,
    TranslateModule,
    ImagePathPipe,
    MatMenuModule,
    RouterModule,
    MatTooltipModule,
    WorkflowCombinePipe,
    MatBadgeModule,
    PageHelpTextComponent,
    HasClaimDirective,
    UTCToLocalTime,
    MatCardModule,
    LimitToPipe,
    NgClass,
  ],
  templateUrl: './folders-view.component.html',
  styleUrl: './folders-view.component.scss',
})
export class FoldersViewComponent extends BaseComponent implements OnInit {
  foldersViewStore = inject(FoldersViewStore);
  activatedRoute = inject(ActivatedRoute);
  overlay = inject(OverlayPanel);
  dialog = inject(MatDialog);
  documentStore = inject(DocumentStore);
  categoryService = inject(CategoryService);
  categoryPermissionService = inject(CategoryPermissionService);
  categories: Category[] = [];
  allCategories: Category[] = [];
  clonerService = inject(ClonerService);
  documentStatuses: DocumentStatus[] = [];
  documentStatusService = inject(DocumentStatusService);
  clientStore = inject(ClientStore);
  documentService = inject(DocumentService);
  selectedDocument: DocumentInfo;
  commonService = inject(CommonService);
  securityService = inject(SecurityService);
  toastrService = inject(ToastrService);
  documentChunks: DocumentChunk[] = [];
  contentType: string = '';
  documentChunkDownloads: DocumentChunkDownload[] = [];
  commonDialogService = inject(CommonDialogService);
  archiveDocumentsStore = inject(ArchiveDocumentsStore);
  archiveFoldersViewStore = inject(ArchiveFoldersViewStore);
  documentCommentService = inject(DocumentCommentService);
  isLoadingResults = false;
  mode: ProgressSpinnerMode = 'indeterminate';
  documentMetaTagStore = inject(DocumentMetaTagStore);
  breakpointObserver = inject(BreakpointObserver);
  isMobile: boolean = false;

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
    if (this.foldersViewStore.categories().length === 0) {
      this.foldersViewStore.loadCategoriesById(
        this.foldersViewStore.selectedCategoryId()
      );
      this.foldersViewStore.loadDocuments();
    }
    this.getCategories();
    this.getDocumentStatus();
    this.ismobileCheck()
  }

  ismobileCheck() {
    if (this.breakpointObserver.isMatched(Breakpoints.Handset)) {
      this.isMobile = true;
    }
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

  getDocumentSummary(document: DocumentInfo) {
    this.commonService
      .getDocumentSummary(document.id ?? '')
      .subscribe(async (content: string) => {
        if (content) {
          this.isLoadingResults = true;
          try {
            const { DocumentSummaryComponent } = await import(
              '../document-summary/document-summary.component'
            );
            const dialogRef = this.dialog.open(DocumentSummaryComponent, {
              data: content,
              width: '80vw',
              maxHeight: '80vh',
            });
          } finally {
            this.isLoadingResults = false;
          }
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
  onFoldersOrCategories() {
    const categoryId = this.foldersViewStore.selectedCategoryId();
    if (categoryId) {
      this.categoryService.getCategoryById(categoryId).subscribe({
        next: (c: Category) => {
          this.openManageCategoryDialog(c);
        },
        error: (error) => { },
      });
    } else {
      this.openManageCategoryDialog(null);
    }
  }

  openManageCategoryDialog(category: Category | null) {
    if (category) {
      category.parentId = category.id;
      category.id = '';
      category.name = '';
      category.description = '';
    }
    const dialogRef = this.dialog.open(ManageCategoryComponent, {
      width: '400px',
      data: Object.assign({}, category),
    });
    dialogRef.afterClosed().subscribe((result: Category) => {
      if (result) {
        this.onFolderClick(result);
      }
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
    if (category.id === this.foldersViewStore.selectedCategoryId()) {
      return;
    }

    if (category.parentId !== this.foldersViewStore.selectedCategoryId()) {
      const documentResource =
        this.foldersViewStore.documentResourceParameter();
      documentResource.categoryId = category.id;
      this.foldersViewStore.loadCategoriesById(category.id ? category.id : '');
      this.foldersViewStore.setDocumentsEmpty();
      documentResource.skip = 0;
      this.foldersViewStore.loadByQuery(documentResource);
      this.foldersViewStore.categoryPath(category.id ?? '');
    } else {
      const documentResource =
        this.foldersViewStore.documentResourceParameter();
      documentResource.categoryId = category.id;
      this.foldersViewStore.loadCategoriesById(category.id ? category.id : '');
      this.foldersViewStore.setDocumentsEmpty();
      documentResource.skip = 0;
      this.foldersViewStore.loadByQuery(documentResource);
      this.foldersViewStore.addCategoryPath(category);
    }
  }
  onRefresh() {
    this.foldersViewStore.setDocumentsEmpty();
    this.foldersViewStore.loadCategoriesById(
      this.foldersViewStore.selectedCategoryId()
    );
    this.foldersViewStore.loadDocuments();
  }

  onFolderClickFromPath(category: Category) {
    this.foldersViewStore.removeCategoryPath(category);
    const documentResource = this.foldersViewStore.documentResourceParameter();
    documentResource.categoryId = category.id;
    this.foldersViewStore.loadCategoriesById(category.id ? category.id : '');
    this.foldersViewStore.setDocumentsEmpty();
    documentResource.skip = 0;
    this.foldersViewStore.loadByQuery(documentResource);
  }

  async onDocumentView(document: DocumentInfo) {
    this.isLoadingResults = true;
    try {
      let extension = document.url?.split('.')[1];
      const documentView: DocumentView = {
        documentId: document.id,
        name: document.name,
        url: document.url,
        extension: document.extension ? document.extension : extension,
        isVersion: false,
        isFromPublicPreview: false,
        isPreviewDownloadEnabled:
          this.securityService.hasClaim('ALL_VIEW_DOCUMENTS'),
        isFileRequestDocument: false,
        isSignatureExists: document.isSignatureExists,
        isChunk: document.isChunk,
        documentNumber: document.documentNumber,
      };
      const { BasePreviewComponent } = await import(
        '@shared/base-preview/base-preview.component'
      );
      this.overlay.open(BasePreviewComponent, {
        position: 'center',
        origin: 'global',
        panelClass: ['file-preview-overlay-container', 'white-background'],
        data: documentView,
      });
    } finally {
      this.isLoadingResults = false;
    }
  }

  sortData(sort: Sort) {
    if (sort.active) {
      if (
        sort.active == 'name' &&
        this.foldersViewStore.documents().length == 0 &&
        this.foldersViewStore.categories().length > 0
      ) {
        const documentResource =
          this.foldersViewStore.documentResourceParameter();
        documentResource.orderBy = `${sort.active} ${sort.direction}`;
        this.foldersViewStore.setDocumentResource(documentResource);
      } else if (
        this.foldersViewStore.documentResourceParameter().totalCount ==
        this.foldersViewStore.documents().length
      ) {
        const documentResource =
          this.foldersViewStore.documentResourceParameter();
        documentResource.orderBy = `${sort.active} ${sort.direction}`;
        this.foldersViewStore.setDocumentResource(documentResource);
      } else {
        this.foldersViewStore.setDocumentsEmpty();
        const documentResource =
          this.foldersViewStore.documentResourceParameter();
        documentResource.orderBy = `${sort.active} ${sort.direction}`;
        documentResource.skip = 0;
        this.foldersViewStore.loadByQuery(documentResource);
      }
    }
  }

  loadDocument() {
    if (this.foldersViewStore.isLoading()) {
      return;
    }
    if (
      this.foldersViewStore.documentResourceParameter().totalCount ==
      this.foldersViewStore.documents().length
    ) {
      return;
    }
    const documentResource = this.foldersViewStore.documentResourceParameter();
    documentResource.categoryId = this.foldersViewStore.selectedCategoryId();
    documentResource.skip = this.foldersViewStore.documents().length;
    this.foldersViewStore.loadByQuery(documentResource);
  }

  onHomeClick() {
    this.foldersViewStore.setDocumentsEmpty();
    this.foldersViewStore.removeCategoryPath(null);
    this.foldersViewStore.loadCategoriesById('');
  }

  editDocument(documentInfo: DocumentInfo) {
    const documentCategories: DocumentCategoryStatus = {
      document: documentInfo,
      categories: this.categories,
      documentStatuses: this.documentStatuses,
      clients: this.clientStore.clients(),
    };
    const dialogRef = this.dialog.open(DocumentEditComponent, {
      width: '60vw',
      data: Object.assign({}, documentCategories),
    });
    dialogRef.afterClosed().subscribe((result: string) => {
      if (result === 'loaded') {
        this.foldersViewStore.loadDocumentsByCategory(
          this.foldersViewStore.selectedCategoryId()
        );
      }
    });
  }

  checkWorkflowInstance(documentInfo: DocumentInfo) {
    if (
      documentInfo?.workflowsDetail &&
      documentInfo?.workflowsDetail?.length > 0
    ) {
      const workflowInstance = documentInfo.workflowsDetail.find(
        (c) =>
          c.workflowInstaceStatus === WorkflowInstanceStatus.InProgress ||
          c.workflowInstaceStatus === WorkflowInstanceStatus.Initiated
      );
      if (workflowInstance) {
        return false;
      }
    }
    return true;
  }

  async manageWorkflowInstance(documentInfo: DocumentInfo) {
    this.isLoadingResults = true;
    try {
      const documentCategories: DocumentCategoryStatus = {
        document: documentInfo,
        categories: this.categories,
        documentStatuses: this.documentStatuses,
        clients: this.clientStore.clients(),
      };
      const { ManageWorkflowInstanceComponent } = await import(
        '../../workflows/manage-workflow-instance/manage-workflow-instance.component'
      );

      const dialogRef = this.dialog.open(ManageWorkflowInstanceComponent, {
        width: '60vw',
        data: Object.assign({}, documentCategories),
      });

      this.sub$.sink = dialogRef
        .afterClosed()
        .subscribe((result: WorkflowInstance) => {
          if (result && result?.workflowId) {
            this.foldersViewStore.loadDocumentsByCategory(
              this.foldersViewStore.selectedCategoryId()
            );
          }
        });
    } finally {
      this.isLoadingResults = false;
    }
  }

  async onDocumentSignatureClick(document: DocumentInfo) {
    if (document.extension == 'pdf') {
      this.onDocumentView(document);
    } else {
      this.isLoadingResults = true;
      try {
        const screenWidth = window.innerWidth;
        const dialogWidth = screenWidth < 768 ? '80vw' : '60vw';
        const { DocumentSignatureComponent } = await import(
          '../document-signature/document-signature.component'
        );
        const dialogRef = this.dialog.open(DocumentSignatureComponent, {
          maxWidth: dialogWidth,
          data: Object.assign({}, document),
        });
        dialogRef.afterClosed().subscribe((result: boolean) => {
          if (result) {
            this.foldersViewStore.loadDocumentsByCategory(
              this.foldersViewStore.selectedCategoryId()
            );
          }
        });
      } finally {
        this.isLoadingResults = false;
      }
    }
  }

  manageDocumentPermission(documentInfo: DocumentInfo) {
    const dialogRef = this.dialog.open(DocumentPermissionListComponent, {
      data: documentInfo,
      width: '80vw',
      maxHeight: '80vh',
    });

    this.sub$.sink = dialogRef.afterClosed().subscribe((result: string) => {
      if (result === 'loaded') {
        this.documentStore.loadDocuments();
        if (
          this.foldersViewStore.selectedCategoryId() == documentInfo.categoryId
        ) {
          this.foldersViewStore.setDocumentsEmpty();
          this.foldersViewStore.loadDocumentsByCategory(
            this.foldersViewStore.selectedCategoryId()
          );
        }
      }
    });
  }

  manageSharePermission(documentInfo: DocumentInfo) {
    const screenWidth = window.innerWidth;
    const dialogWidth = screenWidth < 768 ? '80vw' : '58vw';
    const dialogRef = this.dialog.open(SharePermissionComponent, {
      data: documentInfo,
      width: dialogWidth,
      maxHeight: '90vh',
      maxWidth: dialogWidth,
    });

    this.sub$.sink = dialogRef.afterClosed().subscribe((result: string) => {
      if (result === 'loaded') {
        this.documentStore.loadDocuments();
        if (
          this.foldersViewStore.selectedCategoryId() == documentInfo.categoryId
        ) {
          this.foldersViewStore.setDocumentsEmpty();
          this.foldersViewStore.loadDocumentsByCategory(
            this.foldersViewStore.selectedCategoryId()
          );
        }
      }
    });
  }

  manageCategoryPermission(category: Category) {
    const dialogRef = this.dialog.open(CategoryPermissionListComponent, {
      data: category,
      width: '100%',
      maxWidth: '55vw',
    });

    dialogRef.afterClosed().subscribe((result: boolean) => {
      if (result) {
        this.documentStore.loadDocuments();
        this.foldersViewStore.setDocumentsEmpty();
        this.foldersViewStore.loadCategoriesById(
          this.foldersViewStore.selectedCategoryId()
        );
        this.foldersViewStore.loadByQuery(
          this.foldersViewStore.documentResourceParameter()
        );
      }
    });
  }

  onCreateShareableLink(document: DocumentInfo): void {
    this.sub$.sink = this.documentService
      .getDocumentShareableLink(document.id ?? '')
      .subscribe(async (link: DocumentShareableLink) => {
        this.isLoadingResults = true;
        try {
          const { DocumentSharedLinkComponent } = await import(
            '../document-shared-link/document-shared-link.component'
          );
          this.dialog.open(DocumentSharedLinkComponent, {
            width: '500px',
            data: { document, link },
          });
        } finally {
          this.isLoadingResults = false;
        }
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
          : documentInfo?.url?.split('.')[1],
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
                this.translationService.getValue(
                  'ERROR_WHILE_DOWNLOADING_DOCUMENT'
                )
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
  async uploadNewVersion(document: DocumentInfo) {
    this.isLoadingResults = true;
    try {
      const { DocumentUploadNewVersionComponent } = await import(
        '../document-upload-new-version/document-upload-new-version.component'
      );

      const dialogRef = this.dialog.open(DocumentUploadNewVersionComponent, {
        maxWidth: '60vw',
        width: '100%',
        data: Object.assign({}, document),
      });

      this.sub$.sink = dialogRef
        .afterClosed()
        .subscribe((isNewVersionUploaded: boolean) => {
          if (isNewVersionUploaded) {
            this.documentStore.loadDocuments();
            if (
              this.foldersViewStore.selectedCategoryId() == document.categoryId
            ) {
              this.foldersViewStore.setDocumentsEmpty();
              this.foldersViewStore.loadDocumentsByCategory(
                this.foldersViewStore.selectedCategoryId()
              );
            }
          }
        });
    } finally {
      this.isLoadingResults = false;
    }
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
      .subscribe(async (documentVersions: DocumentVersion[]) => {
        this.isLoadingResults = true;
        try {
          documentInfo.documentVersions = documentVersions;
          const { DocumentVersionHistoryComponent } = await import(
            '../document-version-history/document-version-history.component'
          );

          const dialogRef = this.dialog.open(DocumentVersionHistoryComponent, {
            maxWidth: '95vw',
            panelClass: 'full-width-dialog',
            data: Object.assign({}, documentInfo),
          });
          dialogRef.afterClosed().subscribe((result: boolean) => {
            if (result) {
              this.documentStore.loadDocuments();
              if (
                this.foldersViewStore.selectedCategoryId() ==
                document.categoryId
              ) {
                this.foldersViewStore.setDocumentsEmpty();
                this.foldersViewStore.loadDocumentsByCategory(
                  this.foldersViewStore.selectedCategoryId()
                );
              }
            }
          });
        } finally {
          this.isLoadingResults = false;
        }
      });
  }
  addComment(document: DocumentInfo) {
    const dialogRef = this.dialog.open(DocumentCommentComponent, {
      width: '800px',
      maxHeight: '70vh',
      data: Object.assign({}, document),
    });

    this.sub$.sink = dialogRef.afterClosed().subscribe((result: string) => {
      if (result === 'loaded') {
        this.documentStore.loadDocuments();
        if (this.foldersViewStore.selectedCategoryId() == document.categoryId) {
          this.foldersViewStore.setDocumentsEmpty();
          this.foldersViewStore.loadDocumentsByCategory(
            this.foldersViewStore.selectedCategoryId()
          );
        }
      }
    });
  }
  async addReminder(documentInfo: DocumentInfo) {
    this.isLoadingResults = true;
    try {
      const { DocumentReminderComponent } = await import(
        '../document-reminder/document-reminder.component'
      );
      this.dialog.open(DocumentReminderComponent, {
        data: documentInfo,
        width: '80vw',
        maxHeight: '80vh',
      });
    } finally {
      this.isLoadingResults = false;
    }
  }
  async sendEmail(documentInfo: DocumentInfo) {
    this.isLoadingResults = true;
    try {
      const { SendEmailComponent } = await import(
        '../send-email/send-email.component'
      );
      this.dialog.open(SendEmailComponent, {
        data: documentInfo,
        width: '80vw',
        maxHeight: '80vh',
      });
    } finally {
      this.isLoadingResults = false;
    }
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
              this.foldersViewStore.setDocumentsEmpty();
              this.foldersViewStore.loadDocumentsByCategory(
                this.foldersViewStore.selectedCategoryId()
              );
              this.archiveDocumentsStore.getArchiveDocuments();
            });
        }
      });
  }

  archiveFolder(category: Category) {
    this.sub$.sink = this.commonDialogService
      .deleteConfirmtionDialog(
        `${this.translationService.getValue(
          'ARE_YOU_SURE_YOU_WANT_TO_ARCHIVE'
        )} ${category.name}`
      )
      .subscribe((isTrue: boolean) => {
        if (isTrue) {
          this.sub$.sink = this.categoryService
            .archiveFolder(category.id ?? '')
            .subscribe({
              next: () => {
                this.toastrService.success(
                  this.translationService.getValue(
                    'CATEGORY_ARCHIVE_SUCCESSFULLY'
                  )
                );
                this.foldersViewStore.loadCategoriesById(
                  this.foldersViewStore.selectedCategoryId()
                );
                if (
                  category.parentId ==
                  this.archiveFoldersViewStore.selectedCategoryId() ||
                  category.id == null ||
                  this.archiveFoldersViewStore.selectedCategoryId() == ''
                ) {
                  this.archiveFoldersViewStore.loadCategoriesById(
                    this.archiveFoldersViewStore.selectedCategoryId()
                  );
                }
              },
              error: (err) => {
                let errorMsg = this.translationService.getValue(
                  'ERROR_WHILE_ARCHIVE_FOLDER'
                );
                if (
                  err?.error?.errors &&
                  Array.isArray(err.error.errors) &&
                  err.error.errors.length > 0
                ) {
                  errorMsg = err.error.errors.join(', ');
                }
                this.toastrService.error(errorMsg);
              },
            });
        }
      });
  }
}

import {
  AfterViewInit,
  Component,
  ElementRef,
  inject,
  OnInit,
  signal,
  ViewChild,
} from '@angular/core';
import { BaseComponent } from '../base.component';
import { Category } from '@core/domain-classes/category';
import { NgClass, NgStyle } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSelectChange, MatSelectModule } from '@angular/material/select';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatMenuModule } from '@angular/material/menu';
import { StorageTypePipe } from '../storage-setting/storage-type.pipe';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatExpansionModule } from '@angular/material/expansion';
import { StorageSetting } from '../storage-setting/storage-setting';
import { DocumentStatus } from '../document-status/document-status';
import { ArchiveDocumentsStore } from './archive-documents-store';
import { DocumentResource } from '@core/domain-classes/document-resource';
import { SelectionModel } from '@angular/cdk/collections';
import { DocumentInfo } from '@core/domain-classes/document-info';
import { DocumentService } from '../document/document.service';
import { CommonDialogService } from '@core/common-dialog/common-dialog.service';
import { CategoryService } from '@core/services/category.service';
import { OverlayPanel } from '@shared/overlay-panel/overlay-panel.service';
import { ClonerService } from '@core/services/clone.service';
import { CommonService } from '@core/services/common.service';

import { ToastrService } from '@core/services/toastr-service';
import { DocumentStatusService } from '../document-status/document-status.service';
import { BreakpointsService } from '@core/services/breakpoints.service';
import { WorkflowInstanceService } from '../workflows/workflow-instance.service';
import { StorageSettingService } from '../storage-setting/storage-setting.service';
import {
  bufferCount,
  concatMap,
  debounceTime,
  distinctUntilChanged,
  from,
  fromEvent,
  merge,
  mergeMap,
  tap,
} from 'rxjs';
import { DocumentOperation } from '@core/domain-classes/document-operation';
import { WorkflowInstanceStatus } from '@core/domain-classes/workflow-instance-status.enum';
import { DocumentView } from '@core/domain-classes/document-view';
import { HttpEvent, HttpEventType } from '@angular/common/http';
import { DocumentAuditTrail } from '@core/domain-classes/document-audit-trail';
import { SendEmailComponent } from '../document/send-email/send-email.component';
import { DocumentVersion } from '@core/domain-classes/documentVersion';
import { DocumentVersionHistoryComponent } from '../document/document-version-history/document-version-history.component';
import { DocumentSharedLinkComponent } from '../document/document-shared-link/document-shared-link.component';
import { DocumentShareableLink } from '@core/domain-classes/document-shareable-link';
import { WorkflowShortDetail } from '@core/domain-classes/workflow-short-detail';
import { VisualWorkflowInstance } from '@core/domain-classes/visual-workflow-instance';
import { DocumentStore } from '../document/document-list/document-store';
import { Client } from '@core/domain-classes/client';
import { ClientStore } from '../client/client-store';
import { DocumentChunkDownload } from '@core/domain-classes/document-chunk-download';
import { DocumentChunk } from '@core/domain-classes/document-chunk';
import { MatNativeDateModule } from '@angular/material/core';
import {
  UTCToLocalTime,
  UTCToLocalTimeFormat,
} from '../shared/pipes/utc-to-localtime.pipe';
import { DocumentMetaTagStore } from '../document-meta-tag/document-meta-tag-store';
import { SecurityService } from '../core/security/security.service';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { TranslateModule } from '@ngx-translate/core';
import { HasClaimDirective } from '@shared/has-claim.directive';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';

@Component({
  selector: 'app-archive-documents',
  imports: [
    ReactiveFormsModule,
    MatTableModule,
    MatDialogModule,
    MatSelectModule,
    MatSortModule,
    MatPaginatorModule,
    MatInputModule,
    MatDatepickerModule,
    MatIconModule,
    MatButtonModule,
    MatProgressBarModule,
    MatCheckboxModule,
    MatMenuModule,
    StorageTypePipe,
    MatFormFieldModule,
    MatTooltipModule,
    FormsModule,
    MatExpansionModule,
    MatNativeDateModule,
    PageHelpTextComponent,
    TranslateModule,
    UTCToLocalTime,
    HasClaimDirective,
    MatIconModule,
    MatCardModule,
    NgStyle,
    NgClass,
    MatProgressSpinnerModule
  ],
  templateUrl: './archive-documents.component.html',
  styleUrl: './archive-documents.component.scss'
})
export class ArchiveDocumentsComponent
  extends BaseComponent
  implements OnInit, AfterViewInit {
  step = signal(0);
  // dataSource: DocumentDataSource;
  selectedCategory: Category | null = null;
  displayedColumns: string[] = [
    'action',
    'documentNumber',
    'name',
    'categoryName',
    'createdDate',
    'createdBy',
    'archiveName',
    'client',
    'documentStatus',
    'currentWorkflow',
    'isSigned',
    'signDate',
    'storageType',
  ];
  footerToDisplayed = ['footer'];
  isLoadingResults = false;
  contentType: string = '';
  documentChunkDownloads: DocumentChunkDownload[] = [];

  categories: Category[] = [];
  hierarchicalCategories: Category[] = [];
  storageSetting: StorageSetting<any>[] = [];
  documentStatuses: DocumentStatus[] = [];
  allCategories: Category[] = [];
  documentStore = inject(ArchiveDocumentsStore);
  allDocumentStore = inject(DocumentStore);
  documentResource: DocumentResource =
    this.documentStore.documentResourceParameter();
  loading = this.documentStore.isLoading();
  selectedStatus: DocumentStatus | null = null;
  selectStorage: StorageSetting<any> | null = null;
  selectedCreatedDate!: Date;
  name: string | null = null;
  metatagText: string | null = null;
  selectedDocument: DocumentInfo | null = null;
  documentChunks: DocumentChunk[] = [];
  filterForm: FormGroup;
  selectedType: number | null = null;
  startDate: Date | null = null;
  endDate: Date | null = null;
  startDateHasValue = false;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('input') input: ElementRef;
  @ViewChild('metatag') metatag: ElementRef;
  selection = new SelectionModel<DocumentInfo>(true, []);
  public clientStore = inject(ClientStore);
  public documentMetaTagStore = inject(DocumentMetaTagStore);
  constructor(
    private fb: FormBuilder,
    private documentService: DocumentService,
    private commonDialogService: CommonDialogService,
    private categoryService: CategoryService,
    private dialog: MatDialog,
    public overlay: OverlayPanel,
    public clonerService: ClonerService,
    private securityService: SecurityService,
    private commonService: CommonService,
    private toastrService: ToastrService,
    private documentStatusService: DocumentStatusService,
    private StorageSettingService: StorageSettingService,
    private breakpointsService: BreakpointsService,
    private workflowInstanceService: WorkflowInstanceService,
    private uTCToLocalTime: UTCToLocalTime,
    private breakpointObserver: BreakpointObserver
  ) {
    super();
    if (this.documentStore.documents().length === 0) {
      this.documentStore.getArchiveDocuments();
    }
    this.filterForm = this.fb.group({
      type: [''],
      metaTag: [''],
      startDate: [''],
      endDate: ['', this.endDateValidator.bind(this)],
    });

    this.filterForm.get('startDate')?.valueChanges.subscribe((startDate) => {
      this.filterForm.get('endDate')?.updateValueAndValidity();
    });
    // this.documentResource = new DocumentResource();
    // this.documentResource.pageSize = 10;
    // this.documentResource.orderBy = 'CreatedDate desc';
  }

  endDateValidator(control: any) {
    const startDate = this.filterForm?.get('startDate')?.value;
    if (startDate && !control.value) {
      return { required: true };
    }
    return null;
  }

  ngOnInit(): void {
    if (this.documentResource.createDate) {
      this.selectedCreatedDate = new Date(this.documentResource.createDate);
    }
    this.name = this.documentResource.name ?? '';
    this.metatagText = this.documentResource.metaTags ?? '';

    this.ismobileCheck();
    // this.dataSource = new DocumentDataSource(this.documentService);
    // this.dataSource.loadDocuments(this.documentResource);
    this.getCategories();
    this.getDocumentStatus();
    this.getStorageSetting();
    //this.getResourceParameter();
    if (!this.filterForm.get('startDate')?.value) {
      this.filterForm.get('endDate')?.disable();
    }
  }
  ismobileCheck() {
    if (this.breakpointObserver.isMatched(Breakpoints.Handset)) {
      this.step.set(1);
    }
  }

  onRefresh() {
    this.documentStore.loadByQuery(this.documentResource);
  }

  ngAfterViewInit() {
    this.sub$.sink = this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 0));

    this.sub$.sink = merge(this.sort.sortChange, this.paginator.page)
      .pipe(
        tap((c: any) => {
          this.documentResource.pageSize = this.paginator.pageSize;
          this.documentResource.skip =
            this.paginator.pageIndex * this.paginator.pageSize;
          this.documentResource.pageSize = this.paginator.pageSize;
          this.documentResource.orderBy =
            this.sort.active + ' ' + this.sort.direction;
          // this.dataSource.loadDocuments(this.documentResource);
          this.documentStore.loadByQuery(this.documentResource);
        })
      )
      .subscribe();

    this.sub$.sink = fromEvent(this.input.nativeElement, 'keyup')
      .pipe(
        debounceTime(1000),
        distinctUntilChanged(),
        tap(() => {
          this.paginator.pageIndex = 0;
          this.documentResource.skip = 0;
          this.documentResource.name = this.input.nativeElement.value;
          // this.dataSource.loadDocuments(this.documentResource);
          this.documentStore.loadByQuery(this.documentResource);
        })
      )
      .subscribe();

    this.sub$.sink = fromEvent(this.metatag.nativeElement, 'keyup')
      .pipe(
        debounceTime(1000),
        distinctUntilChanged(),
        tap(() => {
          this.paginator.pageIndex = 0;
          this.documentResource.skip = 0;
          this.documentResource.metaTags = this.metatag.nativeElement.value;
          // this.dataSource.loadDocuments(this.documentResource);
          this.documentStore.loadByQuery(this.documentResource);
        })
      )
      .subscribe();
  }

  /** Whether the number of selected elements matches the total number of rows. */
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.documentStore.documents().length;
    return numSelected === numRows;
  }
  /** Selects all rows if they are not all selected; otherwise clear selection. */
  masterToggle() {
    this.isAllSelected()
      ? this.selection.clear()
      : this.documentStore
        .documents()
        .forEach((row) => this.selection.select(row));
  }

  onCategoryChange(filtervalue: any) {
    if (filtervalue && filtervalue.value) {
      this.documentResource.categoryId = filtervalue.value.id;
    } else {
      this.documentResource.categoryId = '';
    }
    this.documentResource.skip = 0;
    this.paginator.pageIndex = 0;
    //this.dataSource.loadDocuments(this.documentResource);
    this.documentStore.loadByQuery(this.documentResource);
  }

  onClientChange(filterValue: MatSelectChange) {
    const client: Client = filterValue.value;
    if (client) {
      this.documentResource.clientId = client.id;
    } else {
      this.documentResource.clientId = '';
    }
    this.documentResource.skip = 0;
    this.paginator.pageIndex = 0;
    this.documentStore.loadByQuery(this.documentResource);
  }

  onCreatedDateChange(filtervalue: any) {
    if (filtervalue) {
      this.documentResource.createDate = this.selectedCreatedDate;
    } else {
      this.documentResource.createDate = undefined;
    }
    this.documentResource.skip = 0;
    this.paginator.pageIndex = 0;
    this.documentStore.loadByQuery(this.documentResource);
  }

  onDateFilterChange(filterStartValue: any, filterEndValue: any) {
    if (filterStartValue && filterEndValue) {
      const [startDay, startMonth, startYear] = filterStartValue
        .split('/')
        .map(Number);
      const [endDay, endMonth, endYear] = filterEndValue.split('/').map(Number);
      const parsedStartDate = new Date(startYear, startMonth - 1, startDay);
      const parsedEndDate = new Date(endYear, endMonth - 1, endDay);
      const startDate = this.uTCToLocalTime.transform(
        parsedStartDate,
        UTCToLocalTimeFormat.DATE_FORMAT_MMDDYYYY
      );
      const endDate = this.uTCToLocalTime.transform(
        parsedEndDate,
        UTCToLocalTimeFormat.DATE_FORMAT_MMDDYYYY
      );
      this.documentResource.startDate = startDate;
      this.documentResource.endDate = endDate;
    } else {
      this.documentResource.startDate = undefined;
      this.documentResource.endDate = undefined;
    }
    this.documentResource.skip = 0;
    this.paginator.pageIndex = 0;
    this.documentStore.loadByQuery(this.documentResource);
  }

  onStartDateChange(): void {
    const startDate = this.filterForm.get('startDate')?.value;
    const endDateControl = this.filterForm.get('endDate');

    if (startDate) {
      endDateControl?.enable();
    } else {
      endDateControl?.disable();
    }
  }

  getDocumentStatus() {
    this.documentStatusService
      .getDocumentStatuss()
      .subscribe((c: DocumentStatus[]) => {
        if (c && c.length > 0) {
          this.documentStatuses = [...c];
          if (this.documentResource.documentStatusId) {
            this.selectedStatus = this.documentStatuses.find(
              (c) => c.id === this.documentResource.documentStatusId
            ) ?? null;
          }
        }
      });
  }

  getStorageSetting() {
    this.StorageSettingService.getStorageSettings().subscribe(
      (c: StorageSetting<any>[]) => {
        if (c && c.length > 0) {
          this.storageSetting = [...c];
          if (this.documentResource.storageSettingId) {
            this.selectStorage = this.storageSetting.find(
              (c) => c.id === this.documentResource.storageSettingId
            ) ?? null;
          }
        }
      }
    );
  }

  getCategories(): void {
    this.categoryService.getAllCategories().subscribe((c) => {
      this.categories = [...c];
      const categories = this.categories.filter((c) => c.parentId == null);
      categories.forEach((category: Category, index: number) => {
        category.deafLevel = 0;
        category.index = index * Math.pow(0.1, category.deafLevel ?? 0);
        category.displayName = category.name;
        this.allCategories.push(category);
        this.setDeafLevel(category);
      });
      this.allCategories = this.clonerService.deepClone(this.allCategories);
      if (this.documentResource.categoryId) {
        this.selectedCategory = this.allCategories.find(
          (c) => c.id === this.documentResource.categoryId
        ) ?? null;
      }
    });
  }

  setDeafLevel(parent?: Category) {
    if (parent?.children && parent.children.length > 0) {
      parent.children.map((c, index) => {
        c.displayName = parent.displayName + ' > ' + c.name;
        c.deafLevel = parent ? (parent.deafLevel ?? 0) + 1 : 0;
        c.index = parent
          ? (parent.index ?? 0) + index * Math.pow(0.1, c.deafLevel)
          : 0;
        this.allCategories.push(c);
        this.setDeafLevel(c);
      });
    }
    return parent;
  }

  restoreDocument(document: DocumentInfo) {
    const categoryId = document.categoryId;
    if (categoryId) {
      this.categoryService
        .getCategoryById(categoryId)
        .subscribe((category: Category) => {
          if (category?.isArchive) {
            this.toastrService.error(
              this.translationService.getValue(
                'CATEGORY_IS_ARCHIVED_CANNOT_RESTORE_DOCUMENT'
              )
            );
          } else {
            this.sub$.sink = this.commonDialogService
              .deleteConfirmtionDialog(
                `${this.translationService.getValue(
                  'ARE_YOU_SURE_YOU_WANT_TO_RESTORE'
                )} ${document.name}`
              )
              .subscribe((isTrue: boolean) => {
                if (isTrue) {
                  this.sub$.sink = this.documentService
                    .restoreDocument(document.id ?? '')
                    .subscribe(() => {
                      this.toastrService.success(
                        this.translationService.getValue(
                          'DOCUMENT_RESTORE_SUCCESSFULLY'
                        )
                      );
                      this.documentStore.loadByQuery(this.documentResource);
                      this.allDocumentStore.loadDocuments();
                    });
                }
              });
          }
        });
    } else {
      this.toastrService.error(
        this.translationService.getValue('CATEGORY_NOT_FOUND')
      );
    }
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
              this.toastrService.success(
                this.translationService.getValue(
                  'DOCUMENT_DELETED_SUCCESSFULLY'
                )
              );
              this.documentStore.loadByQuery(this.documentResource);
            });
        }
      });
  }

  onStatusChange(filterValue: MatSelectChange) {
    const documentStatus: DocumentStatus = filterValue.value;
    if (documentStatus) {
      this.documentResource.documentStatusId = documentStatus.id;
    } else {
      this.documentResource.documentStatusId = '';
    }
    this.documentResource.skip = 0;
    this.paginator.pageIndex = 0;
    this.documentStore.loadByQuery(this.documentResource);
  }

  onStorageChange(filterValue: MatSelectChange) {
    const storage: StorageSetting<any> = filterValue.value;
    if (storage) {
      this.documentResource.storageSettingId = storage.id;
    } else {
      this.documentResource.storageSettingId = '';
    }
    this.documentResource.skip = 0;
    this.paginator.pageIndex = 0;
    this.documentStore.loadByQuery(this.documentResource);
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

  downloadDocument(documentInfo: DocumentInfo) {
    this.selectedDocument = documentInfo;
    this.documentStore.setLoadingFlag(true);
    if (documentInfo.isChunk) {
      this.getAllDocumentChunks();
    } else {
      const docuView: DocumentView = {
        documentId: documentInfo.id,
        name: '',
        extension: documentInfo?.url?.split('.')[1] ?? '',
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
                type: event?.body?.type,
              });
              this.downloadFile(downloadedFile);
            }
          }
        },
        error: (error) => {
          this.documentStore.setLoadingFlag(false);
          this.toastrService.error(
            this.translationService.getValue('ERROR_WHILE_DOWNLOADING_DOCUMENT')
          );
        },
      });
    }
  }

  addDocumentTrail(id: string, operation: string) {
    const objDocumentAuditTrail: DocumentAuditTrail = {
      documentId: id,
      operationName: operation,
    };
    this.allDocumentStore.addDocumentAudit(objDocumentAuditTrail);
  }

  sendEmail(documentInfo: DocumentInfo) {
    this.dialog.open(SendEmailComponent, {
      data: documentInfo,
      width: '100%',
      maxWidth: '50vw'
    });
  }

  async onDocumentView(document: DocumentInfo) {
    this.isLoadingResults = true;
    try {
      const urls = document.url?.split('.') ?? [];
      const extension = urls[1];
      const documentView: DocumentView = {
        documentId: document.id,
        name: document.name,
        url: document.url,
        extension: extension,
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

      if (extension === 'pdf') {
        this.sub$.sink = this.overlay.isClosePanelClose$.subscribe((c: boolean) => {
          if (c && extension === 'pdf') {
            this.documentStore.loadByQuery(this.documentResource);
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
          maxWidth: '95vw',
          panelClass: 'full-width-dialog',
          data: Object.assign({}, documentInfo),
        });
        dialogRef.afterClosed().subscribe((result: boolean) => {
          if (result) {
            this.documentStore.loadByQuery(this.documentResource);
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
  viewVisualWorkflow(workflowInstance: WorkflowShortDetail): void {
    this.workflowInstanceService
      .getvisualWorkflowInstance(workflowInstance.workflowInstanceId)
      .subscribe({
        next: async (data: VisualWorkflowInstance) => {
          this.isLoadingResults = true;
          try {
            const { VisualWorkflowGraphComponent } = await import(
              '../workflows/visual-workflow-graph/visual-workflow-graph.component'
            );
            const dialogRef = this.dialog.open(VisualWorkflowGraphComponent, {
              width: '95vw',
              data: Object.assign({}, data),
            });
          }
          finally {
            this.isLoadingResults = false;
          }
        },
        error: (error) => { },
      });
  }

  private getAllDocumentChunks() {
    this.sub$.sink = this.commonService
      .getDocumentChunks(this.selectedDocument?.id ?? '')
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
                this.downloadChunk(chunk?.chunkIndex, chunk?.documentVersionId),
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
      this.selectedDocument?.id ?? '',
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
    a.download = this.selectedDocument?.name ?? '';
    a.href = URL.createObjectURL(data);
    a.target = '_blank';
    a.click();
    document.body.removeChild(a);
  }

  onMetaTagChange(filterValue: any) {
    if (filterValue && filterValue.value) {
      this.selectedType = Number(filterValue.value.type);
      if (Number(this.selectedType) === 0) {
        this.filterForm.get('metaTag')?.setValue('');
        this.metatagText = '';
      } else if (Number(this.selectedType) === 1) {
        this.filterForm.get('startDate')?.setValue(null);
        this.filterForm.get('endDate')?.setValue(null);
      }
      this.documentResource.metaTagsTypeId = filterValue.value.id;
    } else {
      this.documentResource.metaTags = '';
      this.metatagText = '';
      this.selectedType = null;
      this.documentResource.metaTagsTypeId = undefined;
      this.documentResource.startDate = undefined;
      this.documentResource.endDate = undefined;
      this.documentResource.skip = 0;
      this.paginator.pageIndex = 0;
      this.documentStore.loadByQuery(this.documentResource);
    }
  }
}

import { SelectionModel } from '@angular/cdk/collections';
import { HttpEvent, HttpEventType, HttpResponse } from '@angular/common/http';
import {
  AfterViewInit,
  Component,
  ElementRef,
  inject,
  OnInit,
  signal,
  ViewChild,
} from '@angular/core';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { CommonDialogService } from '@core/common-dialog/common-dialog.service';
import { Category } from '@core/domain-classes/category';
import { DocumentAuditTrail } from '@core/domain-classes/document-audit-trail';
import { DocumentCategoryStatus } from '@core/domain-classes/document-category';
import { ResponseHeader } from '@core/domain-classes/document-header';
import { DocumentInfo } from '@core/domain-classes/document-info';
import { DocumentOperation } from '@core/domain-classes/document-operation';
import { DocumentResource } from '@core/domain-classes/document-resource';
import { DocumentView } from '@core/domain-classes/document-view';
import { DocumentVersion } from '@core/domain-classes/documentVersion';
import { CategoryService } from '@core/services/category.service';
import { ClonerService } from '@core/services/clone.service';
import { CommonService } from '@core/services/common.service';
import { OverlayPanel } from '@shared/overlay-panel/overlay-panel.service';
import { ToastrService } from '@core/services/toastr-service';
import { from, fromEvent, merge } from 'rxjs';
import {
  bufferCount,
  concatMap,
  debounceTime,
  distinctUntilChanged,
  mergeMap,
  tap,
} from 'rxjs/operators';
import { BaseComponent } from '../../base.component';
import { DocumentCommentComponent } from '../document-comment/document-comment.component';
import { DocumentPermissionListComponent } from '../document-permission/document-permission-list/document-permission-list.component';
import { DocumentPermissionMultipleComponent } from '../document-permission/document-permission-multiple/document-permission-multiple.component';
import { DocumentService } from '../document.service';
import { DocumentShareableLink } from '@core/domain-classes/document-shareable-link';
import { StorageSetting } from '../../storage-setting/storage-setting';
import { DocumentStatus } from '../../document-status/document-status';
import { DocumentStatusService } from '../../document-status/document-status.service';
import { StorageSettingService } from '../../storage-setting/storage-setting.service';
import { MatSelectChange, MatSelectModule } from '@angular/material/select';
import { BreakpointsService } from '@core/services/breakpoints.service';
import { WorkflowShortDetail } from '@core/domain-classes/workflow-short-detail';
import { WorkflowInstanceService } from '../../workflows/workflow-instance.service';
import { VisualWorkflowInstance } from '@core/domain-classes/visual-workflow-instance';
import { WorkflowInstance } from '@core/domain-classes/workflow-instance';
import { WorkflowInstanceStatus } from '@core/domain-classes/workflow-instance-status.enum';
import { DocumentStore } from './document-store';
import { ArchiveDocumentsStore } from '../../archive-documents/archive-documents-store';
import { ClientStore } from '../../client/client-store';
import { Client } from '@core/domain-classes/client';
import { Router, RouterModule } from '@angular/router';
import { DocumentChunkDownload } from '@core/domain-classes/document-chunk-download';
import { DocumentChunk } from '@core/domain-classes/document-chunk';
import {
  UTCToLocalTime,
  UTCToLocalTimeFormat,
} from '@shared/pipes/utc-to-localtime.pipe';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { FoldersViewStore } from '../folders-view/folders-view-store';
import { ArchiveFoldersViewStore } from '../../archive-folders/archive-folders-view-store';
import { DocumentMetaTagStore } from '../../document-meta-tag/document-meta-tag-store';
import { SharePermissionComponent } from '../document-permission/share-permission/share-permission.component';
import { SecurityService } from '@core/security/security.service';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { MatMenuModule } from '@angular/material/menu';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatTableModule } from '@angular/material/table';
import { HasClaimDirective } from '@shared/has-claim.directive';
import { TranslateModule } from '@ngx-translate/core';
import { MatIconModule } from '@angular/material/icon';
import { NgClass, NgStyle } from '@angular/common';
import { StorageTypePipe } from '../../storage-setting/storage-type.pipe';
import { MatBadgeModule } from '@angular/material/badge';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';

@Component({
  selector: 'app-document-list',
  templateUrl: './document-list.component.html',
  styleUrls: ['./document-list.component.scss'],
  standalone: true,
  imports: [
    PageHelpTextComponent,
    MatDialogModule,
    MatMenuModule,
    MatExpansionModule,
    MatSelectModule,
    MatDatepickerModule,
    MatTableModule,
    HasClaimDirective,
    MatSortModule,
    MatPaginator,
    UTCToLocalTime,
    TranslateModule,
    MatIconModule,
    ReactiveFormsModule,
    FormsModule,
    RouterModule,
    StorageTypePipe,
    MatBadgeModule,
    MatCheckboxModule,
    MatTooltipModule,
    MatCardModule,
    NgStyle,
    NgClass,
    MatButtonModule,
    MatProgressSpinnerModule,
  ],
})
export class DocumentListComponent
  extends BaseComponent
  implements OnInit, AfterViewInit {
  step = signal(0);
  selectedCategory: Category | null = null;
  displayedColumns: string[] = [
    'select',
    'action',
    'documentNumber',
    'name',
    'categoryName',
    'createdDate',
    'createdBy',
    'client',
    'documentStatus',
    'currentWorkflow',
    'isSigned',
    'signDate',
    'storageType',
  ];
  footerToDisplayed = ['footer'];
  isLoadingResults = false;
  categories: Category[] = [];
  hierarchicalCategories: Category[] = [];
  storageSetting: StorageSetting<any>[] = [];
  documentStatuses: DocumentStatus[] = [];
  allCategories: Category[] = [];
  documentStore = inject(DocumentStore);
  archiveDocumentsStore = inject(ArchiveDocumentsStore);
  documentResource: DocumentResource =
    this.documentStore.documentResourceParameter();
  loading = this.documentStore.isLoading();
  selectedStatus: DocumentStatus | null = null;
  selectStorage: StorageSetting<any> | null = null;
  selectedCreatedDate?: Date;
  name: string | null = null;
  docNo: string | null = null;
  metatagText: string | null = null;
  selectedDocument: DocumentInfo | null = null;
  documentChunks: DocumentChunk[] = [];
  contentType: string | null = null;
  documentChunkDownloads: DocumentChunkDownload[] = [];
  filterForm: FormGroup;
  selectedType: number | null = null;
  startDate: Date | null = null;
  endDate: Date | null = null;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('input') input: ElementRef;
  @ViewChild('metatag') metatag!: ElementRef;
  @ViewChild('docNoInput') docNoInput: ElementRef;
  selection = new SelectionModel<DocumentInfo>(true, []);
  public clientStore = inject(ClientStore);
  public foldersViewStore = inject(FoldersViewStore);
  public archiveFoldersViewStore = inject(ArchiveFoldersViewStore);
  public documentMetaTagStore = inject(DocumentMetaTagStore);
  constructor(
    private documentService: DocumentService,
    private commonDialogService: CommonDialogService,
    private categoryService: CategoryService,
    private securityService: SecurityService,
    private dialog: MatDialog,
    public overlay: OverlayPanel,
    public clonerService: ClonerService,
    private commonService: CommonService,
    private toastrService: ToastrService,
    private documentStatusService: DocumentStatusService,
    private StorageSettingService: StorageSettingService,
    private breakpointsService: BreakpointsService,
    private workflowInstanceService: WorkflowInstanceService,
    private router: Router,
    private fb: FormBuilder,
    private uTCToLocalTime: UTCToLocalTime,
    private breakpointObserver: BreakpointObserver
  ) {
    super();
    if (this.documentStore.documents().length === 0) {
      this.documentStore.loadDocuments();
    }
    this.filterForm = this.fb.group({
      type: [''],
      metaTag: [''],
      startDate: [''],
      endDate: [
        { value: '', disabled: true },
        this.endDateValidator.bind(this),
      ],
    });

    this.filterForm.get('startDate')?.valueChanges.subscribe((startDate) => {
      const endDateControl = this.filterForm.get('endDate');

      if (startDate) {
        endDateControl?.enable({ emitEvent: false });
      } else {
        endDateControl?.disable({ emitEvent: false });
      }

      endDateControl?.updateValueAndValidity();
    });
  }

  // get visibleTableKeys(): string[] {
  //   return this.documentStore.visibleTableKeys().map((c: any) => c.key);
  // }

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
    this.docNo = this.documentResource.documentNumber ?? '';

    this.ismobileCheck();
    this.getCategories();
    this.getDocumentStatus();
    this.getStorageSetting();

    if (!this.filterForm.get('startDate')?.value) {
      this.filterForm.get('endDate')?.disable();
    }
  }

  onOpened() {
    this.step.set(0);
  }
  onClosed() {
    this.step.set(1);
  }

  isOddDataRow(index: number): boolean {
    // index = the index in dataSource, not in DOM
    return index % 2 !== 0;
  }

  getDataIndex(row: any) {
    return this.documentStore.documents().indexOf(row);
  }

  ismobileCheck() {
    if (this.breakpointObserver.isMatched(Breakpoints.Handset)) {
      this.step.set(1);
    }
  }

  onRefresh() {
    this.documentStore.loadByQuery(this.documentResource);
  }

  onFolderView() {
    this.router.navigate(['/folder-view']);
  }

  ngAfterViewInit() {
    this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 0));

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

    this.sub$.sink = fromEvent(this.docNoInput.nativeElement, 'keyup')
      .pipe(
        debounceTime(1000),
        distinctUntilChanged(),
        tap(() => {
          this.paginator.pageIndex = 0;
          this.documentResource.skip = 0;
          this.documentResource.documentNumber =
            this.docNoInput.nativeElement.value;
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
        .forEach((row: any) => this.selection.select(row));
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

  onCreatedDateChange(filtervalue: any) {
    if (filtervalue) {
      console.log('date', this.selectedCreatedDate);

      const [day, month, year] = filtervalue.split('/').map(Number);
      const parsedDate = new Date(year, month - 1, day);
      this.documentResource.createDate = this.selectedCreatedDate;
    } else {
      this.documentResource.createDate = undefined;
    }
    this.documentResource.skip = 0;
    this.paginator.pageIndex = 0;
    // this.dataSource.loadDocuments(this.documentResource);
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

  getDocumentStatus() {
    this.documentStatusService
      .getDocumentStatuss()
      .subscribe((c: DocumentStatus[]) => {
        if (c && c.length > 0) {
          this.documentStatuses = [...c];
          if (this.documentResource.documentStatusId) {
            this.selectedStatus =
              this.documentStatuses.find(
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
            this.selectStorage =
              this.storageSetting.find(
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
        category.index = index * Math.pow(0.1, category.deafLevel);
        category.displayName = category.name;
        this.allCategories.push(category);
        this.setDeafLevel(category);
      });
      this.allCategories = this.clonerService.deepClone(this.allCategories);
      if (this.documentResource.categoryId) {
        this.selectedCategory =
          this.allCategories.find(
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
              this.documentStore.loadByQuery(this.documentResource);
              this.archiveDocumentsStore.getArchiveDocuments();

              if (
                document.categoryId ==
                this.foldersViewStore.selectedCategoryId() ||
                document.id == null ||
                this.foldersViewStore.selectedCategoryId() == ''
              ) {
                this.foldersViewStore.documents();
              }
              if (
                document.categoryId ==
                this.archiveFoldersViewStore.selectedCategoryId() ||
                document.id == null ||
                this.archiveFoldersViewStore.selectedCategoryId() == ''
              ) {
                this.foldersViewStore.documents();
              }
            });
        }
      });
  }

  getDocuments(): void {
    this.isLoadingResults = true;
    this.sub$.sink = this.documentService
      .getDocuments(this.documentResource)
      .subscribe({
        next: (resp: HttpResponse<DocumentInfo[]>) => {
          const paginationParam = JSON.parse(
            resp.headers?.get('X-Pagination') ?? '{}'
          ) as ResponseHeader;
          this.documentResource.pageSize = paginationParam.pageSize;
          this.documentResource.skip = paginationParam.skip;
          this.isLoadingResults = false;
        },
        error: (error) => {
          this.isLoadingResults = false;
        },
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

  async editDocument(documentInfo: DocumentInfo) {
    this.isLoadingResults = true;
    try {
      const documentCategories: DocumentCategoryStatus = {
        document: documentInfo,
        categories: this.categories,
        documentStatuses: this.documentStatuses,
        clients: this.clientStore.clients(),
      };
      const screenWidth = window.innerWidth;
      const dialogWidth = screenWidth < 768 ? '90vw' : '60vw';
      const { DocumentEditComponent } = await import(
        '../document-edit/document-edit.component'
      );
      const dialogRef = this.dialog.open(DocumentEditComponent, {
        maxWidth: dialogWidth,
        data: Object.assign({}, documentCategories),
      });

      this.sub$.sink = dialogRef.afterClosed().subscribe((result: string) => {
        if (result === 'loaded') {
          this.documentStore.loadByQuery(this.documentResource);
        }
      });
    } finally {
      this.isLoadingResults = false;
    }
  }

  openTableSettings() {
    this.router.navigate(['/documents/table-settings']);
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
        data: Object.assign({}, documentCategories),
      });

      this.sub$.sink = dialogRef
        .afterClosed()
        .subscribe((result: WorkflowInstance) => {
          if (result && result?.workflowId) {
            this.documentStore.loadByQuery(this.documentResource);
          }
        });
    } finally {
      this.isLoadingResults = false;
    }
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

  addComment(documentInfo: DocumentInfo) {
    const dialogRef = this.dialog.open(DocumentCommentComponent, {
      width: '800px',
      maxHeight: '70vh',
      data: Object.assign({}, documentInfo),
    });

    this.sub$.sink = dialogRef.afterClosed().subscribe((result: string) => {
      if (result === 'loaded') {
        this.documentStore.loadByQuery(this.documentResource);
        if (
          documentInfo.categoryId ==
          this.foldersViewStore.selectedCategoryId() ||
          documentInfo.id == null ||
          this.foldersViewStore.selectedCategoryId() == ''
        ) {
          this.foldersViewStore.documents();
        }
      }
    });
  }

  manageDocumentPermission(documentInfo: DocumentInfo) {
    const dialogRef = this.dialog.open(DocumentPermissionListComponent, {
      data: documentInfo,
      maxWidth: '50vw',
      width: '100%',
    });

    this.sub$.sink = dialogRef.afterClosed().subscribe((result: string) => {
      if (result === 'loaded') {
        this.documentStore.loadByQuery(this.documentResource);
        if (
          documentInfo.categoryId ==
          this.foldersViewStore.selectedCategoryId() ||
          documentInfo.id == null ||
          this.foldersViewStore.selectedCategoryId() == ''
        ) {
          this.foldersViewStore.documents();
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
        this.documentStore.loadByQuery(this.documentResource);
        if (
          documentInfo.categoryId ==
          this.foldersViewStore.selectedCategoryId() ||
          documentInfo.id == null ||
          this.foldersViewStore.selectedCategoryId() == ''
        ) {
          this.foldersViewStore.documents();
        }
        this.foldersViewStore.loadCategoriesById(
          this.foldersViewStore.selectedCategoryId()
        );
      }
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

    this.sub$.sink = dialogRef.afterClosed().subscribe((result: string) => {
      this.selection.clear();
      if (result === 'loaded') {
        this.documentStore.loadByQuery(this.documentResource);
        this.foldersViewStore.documents();
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

  async uploadNewVersion(document: Document) {
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
            this.documentStore.loadByQuery(this.documentResource);
          }
        });
    } finally {
      this.isLoadingResults = false;
    }
  }

  downloadDocument(documentInfo: DocumentInfo) {
    this.selectedDocument = documentInfo;
    this.documentStore.setLoadingFlag(true);
    if (documentInfo.isChunk) {
      this.getAllDocumentChunks();
    } else {
      let extension = documentInfo.extension ?? '';
      if (!extension.includes('.')) {
        extension = '.' + extension;
      }
      const docuView: DocumentView = {
        documentId: documentInfo.id,
        name: '',
        extension: documentInfo.extension
          ? extension
          : documentInfo?.url?.split('.')[1] ?? '',
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

  addDocumentTrail(id: string, operation: string) {
    const objDocumentAuditTrail: DocumentAuditTrail = {
      documentId: id,
      operationName: operation,
    };
    this.sub$.sink = this.commonService
      .addDocumentAuditTrail(objDocumentAuditTrail)
      .subscribe((c) => { });
  }

  async sendEmail(documentInfo: DocumentInfo) {
    this.isLoadingResults = true;
    try {
      const { SendEmailComponent } = await import(
        '../send-email/send-email.component'
      );
      this.dialog.open(SendEmailComponent, {
        data: documentInfo,
        width: '100%',
        maxWidth: '50vw',
      });
    } finally {
      this.isLoadingResults = false;
    }
  }

  async addReminder(documentInfo: DocumentInfo) {
    this.isLoadingResults = true;
    try {
      const { DocumentReminderComponent } = await import(
        '../document-reminder/document-reminder.component'
      );
      this.dialog.open(DocumentReminderComponent, {
        data: documentInfo,
        width: '100%',
        maxWidth: '55vw',
      });
    } finally {
      this.isLoadingResults = false;
    }
  }

  async onDocumentView(document: DocumentInfo) {
    this.isLoadingResults = true;
    try {
      let extension = document.extension ?? '';
      if (extension.includes('.')) {
        extension = extension.split('.')[1];
      }
      const documentView: DocumentView = {
        documentId: document.id,
        name: document.name,
        url: document.url,
        extension: extension ? extension : document.url?.split('.')[1],
        isVersion: false,
        isFromPublicPreview: false,
        isPreviewDownloadEnabled: this.securityService.hasClaim(
          'ALL_DOWNLOAD_DOCUMENT'
        ),
        isFileRequestDocument: false,
        isSignatureExists: document.isSignatureExists,
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
        this.sub$.sink = this.overlay.isClosePanelClose$.subscribe(
          (c: boolean) => {
            if (c && extension === 'pdf') {
              this.documentStore.loadByQuery(this.documentResource);
            }
          }
        );
      }
    } finally {
      this.isLoadingResults = false;
    }
  }

  async onVersionHistoryClick(document: DocumentInfo) {
    let documentInfo = this.clonerService.deepClone<DocumentInfo>(document);
    this.sub$.sink = this.documentService
      .getDocumentVersion(document.id ?? '')
      .subscribe(async (documentVersions: DocumentVersion[]) => {
        documentInfo.documentVersions = documentVersions;
        this.isLoadingResults = true;
        try {
          const { DocumentVersionHistoryComponent } = await import(
            '../document-version-history/document-version-history.component'
          );
          const dialogRef = this.dialog.open(DocumentVersionHistoryComponent, {
            maxWidth: '50vw',
            width: '100%',
            panelClass: 'full-width-dialog',
            data: Object.assign({}, documentInfo),
          });
          dialogRef.afterClosed().subscribe((result: boolean) => {
            if (result) {
              this.documentStore.loadByQuery(this.documentResource);
            }
          });
        } finally {
          this.isLoadingResults = false;
        }
      });
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
          '../../document/document-signature/document-signature.component'
        );
        const dialogRef = this.dialog.open(DocumentSignatureComponent, {
          maxWidth: dialogWidth,
          data: Object.assign({}, document),
        });
        dialogRef.afterClosed().subscribe((result: boolean) => {
          if (result) {
            this.documentStore.loadByQuery(this.documentResource);
          }
        });
      } finally {
        this.isLoadingResults = false;
      }
    }
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

  viewVisualWorkflow(workflowInstance: WorkflowShortDetail): void {
    this.workflowInstanceService
      .getvisualWorkflowInstance(workflowInstance.workflowInstanceId)
      .subscribe({
        next: async (data: VisualWorkflowInstance) => {
          this.isLoadingResults = true;
          try {
            const { VisualWorkflowGraphComponent } = await import(
              '../../workflows/visual-workflow-graph/visual-workflow-graph.component'
            );
            const screenWidth = window.innerWidth;
            const dialogWidth = screenWidth < 768 ? '90vw' : '90vw';
            const dialogRef = this.dialog.open(VisualWorkflowGraphComponent, {
              minWidth: dialogWidth,
              data: Object.assign({}, data),
            });
          } finally {
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
    const blob = new Blob(sortedChunks, {
      type: this.contentType ?? undefined,
    });
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
    a.download = this.selectedDocument?.name ?? 'downloaded-file';
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
      this.selectedType = null;
      this.documentResource.metaTagsTypeId = undefined;
      this.documentResource.startDate = undefined;
      this.documentResource.endDate = undefined;
      this.metatagText = '';
      this.documentResource.skip = 0;
      this.paginator.pageIndex = 0;
      this.documentStore.loadByQuery(this.documentResource);
    }
  }
}

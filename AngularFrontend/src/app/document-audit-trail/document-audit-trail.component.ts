import {
  AfterViewInit,
  Component,
  ElementRef,
  inject,
  OnInit,
  signal,
  ViewChild,
} from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { Category } from '@core/domain-classes/category';
import { DocumentAuditTrail } from '@core/domain-classes/document-audit-trail';
import { ResponseHeader } from '@core/domain-classes/document-header';
import { DocumentResource } from '@core/domain-classes/document-resource';
import { CategoryService } from '@core/services/category.service';
import { fromEvent, merge, Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, tap } from 'rxjs/operators';
import { BaseComponent } from '../base.component';
import { DocumentAuditTrialDataSource } from './document-audit-trail-datassource';
import { DocumentAuditTrailService } from './document-audit-trail.service';
import { DocumentView } from '@core/domain-classes/document-view';
import { OverlayPanel } from '@shared/overlay-panel/overlay-panel.service';
import { BreakpointsService } from '@core/services/breakpoints.service';
import { ClonerService } from '@core/services/clone.service';
import { DocumentOperation } from '@core/domain-classes/document-operation';
import { UserStore } from '../user/store/user.store';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';
import { TranslateModule } from '@ngx-translate/core';
import { UTCToLocalTime } from '@shared/pipes/utc-to-localtime.pipe';
import { DocumentOperationColorDirective } from './document-operation-color.directive';
import { MatCardModule } from '@angular/material/card';
import { AsyncPipe, NgClass, NgStyle } from '@angular/common';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-document-audit-trail',
  templateUrl: './document-audit-trail.component.html',
  styleUrls: ['./document-audit-trail.component.scss'],
  standalone: true,
  imports: [
    MatButtonModule,
    MatTooltipModule,
    PageHelpTextComponent,
    MatExpansionModule,
    MatSelectModule,
    MatTableModule,
    TranslateModule,
    MatPaginator,
    MatSort,
    UTCToLocalTime,
    DocumentOperationColorDirective,
    MatCardModule,
    NgClass,
    NgStyle,
    AsyncPipe,
    MatProgressSpinnerModule,
    MatIconModule
  ]
})
export class DocumentAuditTrailComponent
  extends BaseComponent
  implements OnInit, AfterViewInit {
  dataSource: DocumentAuditTrialDataSource;
  operations: { key: string; value: number }[] = [];
  documentOperation = DocumentOperation;
  documentAuditTrails: DocumentAuditTrail[] = [];
  displayedColumns: string[] = [
    'createdDate',
    'documentNumber',
    'documentName',
    'categoryName',
    'operationName',
    'createdBy',
    'permissionUser',
    'permissionRole',
  ];
  isLoadingResults = false;
  documentResource: DocumentResource;
  categories: Category[] = [];
  allCategories: Category[] = [];
  selectedCategory: Category | null = null;
  loading$: Observable<boolean>;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('input') input: ElementRef;
  @ViewChild('docNoInput') docNoInput: ElementRef;
  userStore = inject(UserStore);
  footerToDisplayed = ['footer'];
  step = signal(0);

  constructor(
    private documentAuditTrailService: DocumentAuditTrailService,
    private categoryService: CategoryService,
    public overlay: OverlayPanel,
    private clonerService: ClonerService,
    private breakpointObserver: BreakpointObserver
  ) {
    super();
    this.documentResource = new DocumentResource();
    this.documentResource.pageSize = 10;
    this.documentResource.orderBy = 'createdDate desc';
  }

  ngOnInit(): void {
    this.ismobileCheck();
    this.dataSource = new DocumentAuditTrialDataSource(
      this.documentAuditTrailService
    );
    this.dataSource.loadDocumentAuditTrails(this.documentResource);
    this.getCategories();
    this.getResourceParameter();
    this.getOperations();
  }
  ismobileCheck() {
    if (this.breakpointObserver.isMatched(Breakpoints.Handset)) {
      this.step.set(1);
    }
  }

  ngAfterViewInit() {
    this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 0));

    this.sub$.sink = merge(this.sort.sortChange, this.paginator.page)
      .pipe(
        tap((c: any) => {
          this.documentResource.skip =
            this.paginator.pageIndex * this.paginator.pageSize;
          this.documentResource.pageSize = this.paginator.pageSize;
          this.documentResource.orderBy =
            this.sort.active + ' ' + this.sort.direction;
          this.dataSource.loadDocumentAuditTrails(this.documentResource);
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
          this.dataSource.loadDocumentAuditTrails(this.documentResource);
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
          this.documentResource.documentNumber = this.docNoInput.nativeElement.value;
          this.dataSource.loadDocumentAuditTrails(this.documentResource);
        })
      )
      .subscribe();

  }

  onCategoryChange(filtervalue: any) {
    if (filtervalue && filtervalue.value) {
      this.documentResource.categoryId = filtervalue.value.id;
    } else {
      this.documentResource.categoryId = '';
    }
    this.documentResource.skip = 0;
    this.paginator.pageIndex = 0;
    this.dataSource.loadDocumentAuditTrails(this.documentResource);
  }

  onUserChange(filterValue: any) {
    if (filterValue.value) {
      this.documentResource.createdBy = filterValue.value;
    } else {
      this.documentResource.createdBy = '';
    }
    this.documentResource.skip = 0;
    this.paginator.pageIndex = 0;
    this.dataSource.loadDocumentAuditTrails(this.documentResource);
  }

  onOperationChange(filterValue: any) {
    if (filterValue.value) {
      this.documentResource.operation = filterValue.value;
    } else {
      this.documentResource.operation = '';
    }
    this.documentResource.skip = 0;
    this.paginator.pageIndex = 0;
    this.dataSource.loadDocumentAuditTrails(this.documentResource);
  }

  getOperations(): void {
    this.operations = [
      { key: 'Read', value: 1 },
      { key: 'Created', value: 2 },
      { key: 'Modified', value: 3 },
      { key: 'Deleted', value: 4 },
      { key: 'Added Permission', value: 5 },
      { key: 'Removed Permission', value: 6 },
      { key: 'Send Email', value: 7 },
      { key: 'Download', value: 8 },
      { key: 'Added Version', value: 9 },
      { key: 'Added Signature', value: 10 },
      { key: 'Restored Version', value: 11 },
      { key: 'Archived', value: 12 },
      { key: 'Restored', value: 13 },
      { key: 'Add Folder Permission', value: 14 },
      { key: 'Removed Folder Permission', value: 15 },
      { key: 'Archived Folder', value: 16 },
      { key: 'Restored Folder', value: 17 },
      { key: 'Added Folder', value: 18 },
      { key: 'Edited Folder', value: 19 },
      { key: 'Deleted Folder', value: 20 },
    ];
  }

  getCategories(): void {
    this.categoryService.getAllCategories().subscribe((c) => {
      this.categories = [...c];
      const categories = this.categories.filter((c) => c.parentId == null);
      categories.forEach((category: Category, index: number) => {
        category.deafLevel = 0;
        category.index =
          index * Math.pow(0.1, category.deafLevel);
        category.displayName = category.name;
        this.allCategories.push(category);
        this.setDeafLevel(category);
      });
      this.allCategories = this.clonerService.deepClone(this.allCategories);
    });
  }

  setDeafLevel(parent?: Category) {
    if (parent?.children && parent.children.length > 0) {
      parent.children.map((c, index) => {
        c.displayName = parent.displayName + ' > ' + c.name;
        c.deafLevel = parent ? (parent.deafLevel ?? 0) + 1 : 0;
        c.index =
          (parent ? parent.index ?? 0 : 0) + index * Math.pow(0.1, c.deafLevel);
        this.allCategories.push(c);
        this.setDeafLevel(c);
      });
    }
    return parent;
  }

  getResourceParameter() {
    this.sub$.sink = this.dataSource.responseHeaderSubject$.subscribe(
      (c: ResponseHeader) => {
        if (c) {
          this.documentResource.pageSize = c.pageSize;
          this.documentResource.skip = c.skip;
          this.documentResource.totalCount = c.totalCount;
        }
      }
    );
  }
  async onDocumentView(documentAuditTrail: DocumentAuditTrail) {
    this.isLoadingResults = true;
    try {
      const urls = documentAuditTrail.url?.split('.') ?? [];
      const extension = urls[1];
      const documentView: DocumentView = {
        documentId: documentAuditTrail.documentId,
        name: documentAuditTrail.documentName,
        extension: extension,
        isVersion: false,
        isFromPublicPreview: false,
        isPreviewDownloadEnabled: false,
        isSignatureExists: false,
        isFileRequestDocument: false,
        url: documentAuditTrail.url,
        isChunk: documentAuditTrail.isChunk,
        documentNumber: documentAuditTrail.documentNumber
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
    finally {
      this.isLoadingResults = false;
    }
  }
}

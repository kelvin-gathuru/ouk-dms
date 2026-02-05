import {
  AfterViewInit,
  Component,
  ElementRef,
  OnInit,
  signal,
  ViewChild,
} from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { BaseComponent } from '../base.component';
import { DocumentOperation } from '@core/domain-classes/document-operation';
import { DocumentAuditTrail } from '@core/domain-classes/document-audit-trail';
import { DocumentResource } from '@core/domain-classes/document-resource';
import { Category } from '@core/domain-classes/category';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import {
  debounceTime,
  distinctUntilChanged,
  fromEvent,
  merge,
  Observable,
  tap,
} from 'rxjs';
import { User } from '@core/domain-classes/user';
import { RecentActivityService } from './recent-activity.service';
import { CategoryService } from '@core/services/category.service';
import { CommonService } from '@core/services/common.service';
import { OverlayPanel } from '@shared/overlay-panel/overlay-panel.service';
import { ClonerService } from '@core/services/clone.service';
import { BreakpointsService } from '@core/services/breakpoints.service';
import { ResponseHeader } from '@core/domain-classes/document-header';
import { DocumentView } from '@core/domain-classes/document-view';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatInputModule } from '@angular/material/input';
import { TranslateModule } from '@ngx-translate/core';
import { MatTooltipModule } from '@angular/material/tooltip';
import { DocumentOperationColorDirective } from '../document-audit-trail/document-operation-color.directive';
import { RecentActivityDataSource } from './recent-activity-datassource';
import { ToastrService } from '@core/services/toastr-service';
import { UserAuth } from '@core/domain-classes/user-auth';
import { SecurityService } from '@core/security/security.service';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { UTCToLocalTime } from '@shared/pipes/utc-to-localtime.pipe';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { AsyncPipe, NgClass, NgStyle } from '@angular/common';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';

@Component({
  selector: 'app-recent-activity',
  imports: [
    MatTableModule,
    MatSelectModule,
    MatSlideToggleModule,
    MatSortModule,
    MatPaginatorModule,
    MatInputModule,
    TranslateModule,
    MatExpansionModule,
    MatTooltipModule,
    DocumentOperationColorDirective,
    PageHelpTextComponent,
    UTCToLocalTime,
    MatIconModule,
    MatCardModule,
    AsyncPipe,
    NgClass,
    NgStyle,
    MatProgressSpinnerModule
  ],
  templateUrl: './recent-activity.component.html',
  styleUrl: './recent-activity.component.scss'
})
export class RecentActivityComponent
  extends BaseComponent
  implements OnInit, AfterViewInit {
  dataSource: RecentActivityDataSource;
  operations: { key: string; value: number }[] = [];
  documentOperation = DocumentOperation;
  documentAuditTrails: DocumentAuditTrail[] = [];
  displayedColumns: string[] = [
    'createdDate',
    'documentNumber',
    'documentName',
    'categoryName',
    'operationName',
  ];
  isLoadingResults = true;
  documentResource: DocumentResource;
  categories: Category[] = [];
  allCategories: Category[] = [];
  selectedCategory: Category | null = null;
  loading$: Observable<boolean>;
  securityObject: UserAuth;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('input') input: ElementRef;
  @ViewChild('docNoInput') docNoInput: ElementRef;
  users: User[] = [];
  footerToDisplayed = ['footer'];
  step = signal(0);

  constructor(
    private categoryService: CategoryService,
    private commonService: CommonService,
    public overlay: OverlayPanel,
    private clonerService: ClonerService,
    private toastrService: ToastrService,
    private securityService: SecurityService,
    private recentActivityService: RecentActivityService,
    private breakpointObserver: BreakpointObserver
  ) {
    super();
    this.documentResource = new DocumentResource();
    this.documentResource.pageSize = 10;
    this.documentResource.orderBy = 'createdDate desc';
  }

  ngOnInit(): void {
    this.getAuthObj();
    this.ismobileCheck();
    this.dataSource = new RecentActivityDataSource(this.recentActivityService);
    this.dataSource.loadRecentActivityDocuments(this.documentResource);
    this.getCategories();
    this.getResourceParameter();
    this.getOperations();
  }
  ismobileCheck() {
    if (this.breakpointObserver.isMatched(Breakpoints.Handset)) {
      this.step.set(1);
    }
  }

  getAuthObj() {
    this.sub$.sink = this.securityService.SecurityObject.subscribe((c) => {
      if (c)
        this.securityObject = c;
    });
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
          this.dataSource.loadRecentActivityDocuments(this.documentResource);
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
          this.dataSource.loadRecentActivityDocuments(this.documentResource);
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
          this.dataSource.loadRecentActivityDocuments(this.documentResource);
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
    this.dataSource.loadRecentActivityDocuments(this.documentResource);
  }

  onOperationChange(filterValue: any) {
    if (filterValue.value) {
      this.documentResource.operation = filterValue.value;
    } else {
      this.documentResource.operation = '';
    }
    this.documentResource.skip = 0;
    this.paginator.pageIndex = 0;
    this.dataSource.loadRecentActivityDocuments(this.documentResource);
  }

  getOperations(): void {
    this.operations = [
      { key: 'Read', value: 1 },
      { key: 'Created', value: 2 },
      { key: 'Modified', value: 3 },
      { key: 'Deleted', value: 4 },
      { key: 'Download', value: 8 },
      { key: 'Added Version', value: 9 },
      { key: 'Added Signature', value: 10 },
      { key: 'Restored Version', value: 11 },
      { key: 'Archived', value: 12 },
      { key: 'Restored', value: 13 },
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
        category.index = index * Math.pow(0.1, category.deafLevel);
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
          (parent ? (parent.index ?? 0) + index * Math.pow(0.1, c.deafLevel) : 0);
        this.allCategories.push(c);
        this.setDeafLevel(c);
      });
    }
    return parent;
  }

  getResourceParameter() {
    this.sub$.sink = this.dataSource.responseHeaderSubject$.subscribe(
      (c: ResponseHeader | null) => {
        if (c) {
          this.documentResource.pageSize = c.pageSize;
          this.documentResource.skip = c.skip;
          this.documentResource.totalCount = c.totalCount;
        }
      }
    );
  }

  async onDocumentView(documentAuditTrail: DocumentAuditTrail) {
    const urls = documentAuditTrail?.url?.split('.') ?? [];
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
      documentNumber: documentAuditTrail.documentNumber,
    };

    if (this.securityObject.isSuperAdmin) {

      const { BasePreviewComponent } = await import(
        '../shared/base-preview/base-preview.component'
      );

      this.overlay.open(BasePreviewComponent, {
        position: 'center',
        origin: 'global',
        panelClass: ['file-preview-overlay-container', 'white-background'],
        data: documentView,
      });
    } else {
      this.commonService
        .checkDocumentPermission(documentView.documentId ?? '')
        .subscribe(async (c) => {
          if (!c) {
            this.toastrService.error(
              this.translationService.getValue(
                'YOUR_PERMISSION_IS_REVOKED_YOU_CANNOT_SEE_THIS_DOCUMENT'
              )
            );
            return;
          } else {
            const { BasePreviewComponent } = await import(
              '../shared/base-preview/base-preview.component'
            );
            this.overlay.open(BasePreviewComponent, {
              position: 'center',
              origin: 'global',
              panelClass: [
                'file-preview-overlay-container',
                'white-background',
              ],
              data: documentView,
            });
          }
        });
    }
  }
}

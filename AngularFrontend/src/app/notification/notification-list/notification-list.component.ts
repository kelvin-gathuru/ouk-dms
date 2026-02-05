import {
  AfterViewInit,
  Component,
  ElementRef,
  OnInit,
  ViewChild,
} from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { ResponseHeader } from '@core/domain-classes/document-header';
import { DocumentResource } from '@core/domain-classes/document-resource';
import { NotificationType, UserNotification } from '@core/domain-classes/notification';
import { OverlayPanel } from '@shared/overlay-panel/overlay-panel.service';
import { fromEvent, merge, Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, tap } from 'rxjs/operators';
import { BaseComponent } from '../../base.component';
import { NotificationDataSource } from '../notification-datassource';
import { NotificationService } from '../notification.service';
import { DocumentView } from '@core/domain-classes/document-view';
import { ToastrService } from '@core/services/toastr-service';
import { ServiceResponse } from '../../core/domain-classes/service-response';
import { WorkflowInstanceService } from '../../workflows/workflow-instance.service';
import { VisualWorkflowInstance } from '@core/domain-classes/visual-workflow-instance';
import { MatDialog } from '@angular/material/dialog';
import { DocumentService } from '../../document/document.service';
import { DocumentInfo } from '@core/domain-classes/document-info';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { MatTableModule } from '@angular/material/table';
import { TranslateModule } from '@ngx-translate/core';
import { UTCToLocalTime } from '@shared/pipes/utc-to-localtime.pipe';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { AsyncPipe } from '@angular/common';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-notification-list',
  templateUrl: './notification-list.component.html',
  styleUrls: ['./notification-list.component.scss'],
  standalone: true,
  imports: [
    PageHelpTextComponent,
    MatPaginator,
    MatSort,
    MatTableModule,
    TranslateModule,
    UTCToLocalTime,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    AsyncPipe,
    MatProgressSpinnerModule
  ],
})
export class NotificationListComponent
  extends BaseComponent
  implements OnInit, AfterViewInit {
  dataSource: NotificationDataSource;
  notifications: UserNotification[] = [];
  displayedColumns: string[] = ['action', 'createdDate', 'message'];
  isLoadingResults = false;
  notificationResource: DocumentResource;
  loading$: Observable<boolean>;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('input') input: ElementRef;
  footerToDisplayed = ['footer'];
  constructor(
    private notificationService: NotificationService,
    private toastrService: ToastrService,
    public overlay: OverlayPanel,
    private workflowInstanceService: WorkflowInstanceService,
    private dialog: MatDialog,
    private documentService: DocumentService
  ) {
    super();
    this.notificationResource = new DocumentResource();
    this.notificationResource.pageSize = 10;
    this.notificationResource.orderBy = 'createdDate desc';
  }

  ngOnInit(): void {
    this.dataSource = new NotificationDataSource(this.notificationService);
    this.dataSource.loadNotifications(this.notificationResource);
    this.getResourceParameter();
  }

  ngAfterViewInit() {
    this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 0));

    this.sub$.sink = merge(this.sort.sortChange, this.paginator.page)
      .pipe(
        tap(() => {
          this.notificationResource.skip = this.paginator.pageIndex * this.paginator.pageSize;
          this.notificationResource.pageSize = this.paginator.pageSize;
          this.notificationResource.orderBy =
            this.sort.active + ' ' + this.sort.direction;
          this.dataSource.loadNotifications(this.notificationResource);
        })
      )
      .subscribe();

    this.sub$.sink = fromEvent(this.input.nativeElement, 'keyup')
      .pipe(
        debounceTime(1000),
        distinctUntilChanged(),
        tap(() => {
          this.paginator.pageIndex = 0;
          this.notificationResource.skip = 0;
          this.notificationResource.name = this.input.nativeElement.value;
          this.dataSource.loadNotifications(this.notificationResource);
        })
      )
      .subscribe();
  }
  getResourceParameter() {
    this.sub$.sink = this.dataSource.responseHeaderSubject$.subscribe(
      (c: ResponseHeader) => {
        if (c) {
          this.notificationResource.pageSize = c.pageSize;
          this.notificationResource.skip = c.skip;
          this.notificationResource.totalCount = c.totalCount;
        }
      }
    );
  }

  viewDocument(notification: UserNotification) {
    if (notification.notificationsType === NotificationType.REMINDER) {
      this.notificationService
        .checkReminderByDocumentId(notification.documentId ?? '')
        .subscribe((response: ServiceResponse<boolean>) => {
          if (response.data) {
            this.previewDocument(notification);
          } else {
            this.toastrService.error(
              this.translationService.getValue('YOU_ARE_NOT_AUTHORIZED_TO_VIEW_THIS_REMINDER')
            );
          }
        });
    } else if (notification.notificationsType === NotificationType.SHARE_USER) {
      this.notificationService
        .checkShareUserByDocumentId(notification.documentId ?? '')
        .subscribe((isShare: ServiceResponse<boolean>) => {
          if (isShare.data) {
            this.previewDocument(notification);
          } else {
            this.toastrService.error(
              this.translationService.getValue('YOU_ARE_NOT_AUTHORIZED_TO_VIEW_THIS_DOCUMENT')
            );
          }
        });
    } else if (notification.notificationsType === NotificationType.WORKFLOW) {
      this.previewDocument(notification);
    }
  }

  async previewDocument(notification: UserNotification) {
    this.documentService.getDocument(notification.documentId ?? '')
      .subscribe(async (document: DocumentInfo) => {
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
            isPreviewDownloadEnabled: false,
            isFileRequestDocument: false,
            isSignatureExists: document.isSignatureExists,
            documentNumber: document.documentNumber
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
      });
  }
  viewVisualWorkflow(notification: UserNotification): void {
    this.workflowInstanceService
      .getvisualWorkflowInstance(notification.workflowInstanceId ?? '')
      .subscribe({
        next: async (data: VisualWorkflowInstance) => {
          this.isLoadingResults = true;
          try {
            const { VisualWorkflowGraphComponent } = await import(
              '../../workflows/visual-workflow-graph/visual-workflow-graph.component'
            );
            const dialogRef = this.dialog.open(VisualWorkflowGraphComponent, {
              width: '100%',
              maxWidth: '70vw',
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
}

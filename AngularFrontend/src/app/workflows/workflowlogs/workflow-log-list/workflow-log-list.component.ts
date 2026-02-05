import { AfterViewInit, Component, inject, OnInit, ViewChild } from '@angular/core';
import { WorkflowLogResource } from '@core/domain-classes/workflow-log-resource';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { RouterModule } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { WorkflowLog } from '@core/domain-classes/workflow-log';
import { merge, Observable, tap } from 'rxjs';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { WorkflowLogService } from '../workflow-log.service';
import { BaseComponent } from '../../../base.component';
import { WorkflowLogDataSource } from '../workflow-log-datasource';
import { ResponseHeader } from '@core/domain-classes/document-header';
import { WorkflowInstanceService } from '../../workflow-instance.service';
import { Workflow } from '@core/domain-classes/workflow';
import { CommonError } from '@core/error-handler/common-error';
import { WorkflowInstanceData } from '@core/domain-classes/workflow-instance-data';
import { VisualWorkflowInstance } from '@core/domain-classes/visual-workflow-instance';
import { DocumentView } from '@core/domain-classes/document-view';
import { CommonDialogService } from '@core/common-dialog/common-dialog.service';
import { OverlayPanel } from '@shared/overlay-panel/overlay-panel.service';
import { MatDialog } from '@angular/material/dialog';
import { WorkflowInstanceStatus } from '@core/domain-classes/workflow-instance-status.enum';
import { WorkflowInstanceStatusPipe } from '@shared/pipes/workflow-instance-status.pipe';
import { WorkflowTransitionInstanceStatusPipe } from '../../../shared/pipes/workflow-transition-instance-status.pipe';
import { MatSelectChange, MatSelectModule } from '@angular/material/select';
import { WorkflowStatusColorDirective } from '../../workflow-status-color.directive';
import { WorkflowTransitionInstanceStatusColorDirective } from '../../workflow-transition-instance-status-color.directive';
import { DocumentShort } from '@core/domain-classes/document-short';
import { UTCToLocalTime } from '@shared/pipes/utc-to-localtime.pipe';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { MatIconModule } from '@angular/material/icon';
import { AsyncPipe } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatButtonModule } from "@angular/material/button";

@Component({
  selector: 'app-workflow-log-list',
  imports: [
    FormsModule,
    TranslateModule,
    RouterModule,
    MatSelectModule,
    MatPaginatorModule,
    WorkflowInstanceStatusPipe,
    WorkflowTransitionInstanceStatusPipe,
    WorkflowStatusColorDirective,
    WorkflowTransitionInstanceStatusColorDirective,
    MatTableModule,
    MatSortModule,
    UTCToLocalTime,
    PageHelpTextComponent,
    MatIconModule,
    AsyncPipe,
    MatCardModule,
    MatProgressSpinnerModule,
    MatButtonModule
],
  templateUrl: './workflow-log-list.component.html',
  styleUrl: './workflow-log-list.component.scss'
})
export class WorkflowLogListComponent extends BaseComponent implements OnInit, AfterViewInit {
  dataSource: WorkflowLogDataSource;
  workflowlogs: WorkflowLog[] = [];
  WorkflowInstanceStatus = WorkflowInstanceStatus;
  displayedColumns: string[] = [
    'detail',
    'workflowname',
    'workflowInstanceStatus',
    'initiatedUser',
    'initiateddate',
    'documentNumber',
    'documentname',
    'transitionname',
    'workflowsteps',
    'workflowtransitioninstancestatus',
    'performBy',
    'transitiondate',
    'comment',
  ];
  isLoadingResults = false;
  workflowLogResource: WorkflowLogResource;
  loading$: Observable<boolean>;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  footerToDisplayed = ['footer'];
  workflows: Workflow[] = [];
  documents: DocumentShort[] = [];
  workflowInstanceStatuses: { key: string; value: number }[] = [];
  private dialog = inject(MatDialog);
  public overlay = inject(OverlayPanel);
  public commonDialogService = inject(CommonDialogService);

  constructor(
    private workflowLogService: WorkflowLogService,
    private workflowInstanceService: WorkflowInstanceService

  ) {
    super();
    this.workflowLogResource = new WorkflowLogResource();
    this.workflowLogResource.pageSize = 10;
    this.workflowLogResource.orderBy = 'transitiondate asc';
  }

  ngOnInit(): void {
    this.getWorkflowInstanceStatuses();
    this.getWorkflows();
    this.getAllDocuments();
    this.dataSource = new WorkflowLogDataSource(this.workflowLogService);
    this.dataSource.loadWorkflowLogs(this.workflowLogResource);
    this.getResourceParameter();
  }

  getWorkflowInstanceStatuses() {
    this.workflowInstanceStatuses = [
      {
        key: '-- None --',
        value: 4,
      },
      {
        key: 'Initiated',
        value: 0,
      },
      {
        key: 'In Progress',
        value: 1,
      },
      {
        key: 'Completed',
        value: 2,
      },
      {
        key: 'Cancelled',
        value: 3,
      }
    ];
  }

  getAllDocuments() {
    this.workflowInstanceService.getallDocuments()
      .subscribe((documents: DocumentShort[]) => {
        this.documents = documents;
      });
  }

  getWorkflows() {
    this.workflowInstanceService.getallWorkflows()
      .subscribe((c: Workflow[] | CommonError) => {
        if (Array.isArray(c) && c.length > 0) {
          this.workflows = [...c];
        }
      });
  }


  ngAfterViewInit() {
    if (this.sort && this.paginator) {
      this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 0));

      this.sub$.sink = merge(this.sort.sortChange, this.paginator.page)
        .pipe(
          tap(() => {
            this.workflowLogResource.skip =
              this.paginator.pageIndex * this.paginator.pageSize;
            this.workflowLogResource.pageSize = this.paginator.pageSize;
            this.workflowLogResource.orderBy =
              this.sort.active + ' ' + this.sort.direction;
            this.dataSource.loadWorkflowLogs(this.workflowLogResource);
          })
        )
        .subscribe();
    }
  }

  viewVisualWorkflow(workflowInstance: WorkflowInstanceData): void {
    this.workflowInstanceService
      .getvisualWorkflowInstance(workflowInstance.workflowInstanceId)
      .subscribe({
        next: async (data: VisualWorkflowInstance) => {
          this.isLoadingResults = true;
          try {
            const { VisualWorkflowGraphComponent } = await import(
              '../../visual-workflow-graph/visual-workflow-graph.component'
            );
            const screenWidth = window.innerWidth;
            const dialogWidth = screenWidth < 768 ? '90vw' : '90vw';
            const dialogRef = this.dialog.open(VisualWorkflowGraphComponent, {
              maxWidth: dialogWidth,
              width: '100%',
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

  async onDocumentView(document: WorkflowInstanceData) {
    const urls = document.documentUrl.split('.');
    const extension = urls[1];
    const documentView: DocumentView = {
      documentId: document.documentId,
      name: document.documentName,
      extension: extension,
      isVersion: false,
      isFromPublicPreview: false,
      isPreviewDownloadEnabled: false,
      isFileRequestDocument: false,
      isSignatureExists: false,
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
  }

  onWorkflowStatusChange(filterValue: MatSelectChange) {
    this.workflowLogResource.workflowInstanceStatus = filterValue.value;
    this.workflowLogResource.workflowInstanceStatus = filterValue.value;
    this.dataSource.loadWorkflowLogs(this.workflowLogResource);
  }

  onWorkflowsChanges(filterValue: MatSelectChange) {
    if (filterValue.value) {
      this.workflowLogResource.workflowId = filterValue.value;
    } else {
      this.workflowLogResource.workflowId = '';
    }
    this.workflowLogResource.skip = 0;
    this.paginator.pageIndex = 0;
    this.dataSource.loadWorkflowLogs(this.workflowLogResource);
  }

  onDocumentChange(filterValue: MatSelectChange) {
    if (filterValue.value) {
      this.workflowLogResource.documentId = filterValue.value;
    } else {
      this.workflowLogResource.documentId = '';
    }
    this.workflowLogResource.skip = 0;
    this.paginator.pageIndex = 0;
    this.dataSource.loadWorkflowLogs(this.workflowLogResource);
  }

  getResourceParameter() {
    this.sub$.sink = this.dataSource.responseHeaderSubject$.subscribe(
      (c: ResponseHeader) => {
        if (c) {
          this.workflowLogResource.pageSize = c.pageSize;
          this.workflowLogResource.skip = c.skip;
          this.workflowLogResource.totalCount = c.totalCount;
        }
      }
    );
  }
}

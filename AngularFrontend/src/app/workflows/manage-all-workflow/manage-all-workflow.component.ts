import { AsyncPipe, NgClass } from '@angular/common';
import {
  AfterViewInit,
  Component,
  inject,
  OnInit,
  ViewChild,
} from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatStepperModule } from '@angular/material/stepper';
import { MatTableModule } from '@angular/material/table';
import { Router, RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { WorkflowInstanceService } from '../workflow-instance.service';
import { WorkflowInstanceData } from '../../core/domain-classes/workflow-instance-data';
import { CurrentWorkflowTransition } from '@core/domain-classes/current-workflow-transition';
import { CommonDialogService } from '@core/common-dialog/common-dialog.service';
import { WorkflowInstanceStatusPipe } from '@shared/pipes/workflow-instance-status.pipe';
import { NextTransition } from '@core/domain-classes/next-transition';
import { MatDialog } from '@angular/material/dialog';
import { VisualWorkflowInstance } from '@core/domain-classes/visual-workflow-instance';
import { DocumentView } from '@core/domain-classes/document-view';
import { OverlayPanel } from '@shared/overlay-panel/overlay-panel.service';
import { MatTooltipModule, TooltipPosition } from '@angular/material/tooltip';
import { WorkflowInstanceStatus } from '@core/domain-classes/workflow-instance-status.enum';
import {
  merge,
  Observable,
  tap,
} from 'rxjs';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { WorkflowsResource } from '@core/domain-classes/workflows-resource';
import { WorkflowsDataSource } from './workflows-datasource';
import { ResponseHeader } from '@core/domain-classes/document-header';
import { BaseComponent } from '../../base.component';
import { MatSelectChange, MatSelectModule } from '@angular/material/select';
import { Workflow } from '@core/domain-classes/workflow';
import { WorkflowStatusColorDirective } from '../workflow-status-color.directive';
import { CommonService } from '@core/services/common.service';

import { ToastrService } from '@core/services/toastr-service';
import { PerformTransitionComponent } from '../perform-transition/perform-transition.component';
import { SignalrService } from '../../core/services/signalr.service';
import { DocumentShort } from '@core/domain-classes/document-short';
import { MatMenuModule } from '@angular/material/menu';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { UTCToLocalTime } from '@shared/pipes/utc-to-localtime.pipe';
import { LimitToPipe } from '@shared/pipes/limit-to.pipe';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';


@Component({
  selector: 'app-manage-all-workflow',
  imports: [
    FormsModule,
    TranslateModule,
    RouterModule,
    MatButtonModule,
    ReactiveFormsModule,
    MatIconModule,
    MatCardModule,
    MatStepperModule,
    MatFormFieldModule,
    MatSortModule,
    MatPaginatorModule,
    MatTableModule,
    MatInputModule,
    MatSelectModule,
    WorkflowInstanceStatusPipe,
    WorkflowStatusColorDirective,
    MatTooltipModule,
    MatMenuModule,
    PageHelpTextComponent,
    UTCToLocalTime,
    LimitToPipe,
    NgClass,
    AsyncPipe,
    MatProgressSpinnerModule
  ],
  templateUrl: './manage-all-workflow.component.html',
  styleUrl: './manage-all-workflow.component.scss'
})
export class ManageAllWorkflowComponent
  extends BaseComponent
  implements OnInit, AfterViewInit {
  dataSource: WorkflowsDataSource;
  isLoadingResults = false;
  workflowsResource: WorkflowsResource;
  loading$: Observable<boolean>;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  footerToDisplayed = ['footer'];
  workflowNameFilter: string = '';
  workflowStatusFilter: string = '';
  workflowInstances: WorkflowInstanceData[] = [];
  WorkflowInstanceStatus = WorkflowInstanceStatus;
  workflowInstanceStatuses: { key: string; value: number }[] = [];
  workflows: Workflow[] = [];
  documents: DocumentShort[] = [];
  displayedColumns: string[] = [
    'transition',
    'workflowInitiatedDate',
    'workflowname',
    'workflowInstanceStatus',
    'initiatedUser',
    'documentname',
    'lastTransition',
    'updatedAt',
    'performBy'
  ];
  private dialog = inject(MatDialog);
  public overlay = inject(OverlayPanel);
  public commonDialogService = inject(CommonDialogService);
  public commonService = inject(CommonService);
  public toastrService = inject(ToastrService);
  private router = inject(Router)
  positionOptions: TooltipPosition[] = ['below', 'above', 'left', 'right'];

  constructor(
    private workflowInstanceService: WorkflowInstanceService,
    private signalrService: SignalrService
  ) {
    super();
    this.workflowsResource = new WorkflowsResource();
    this.workflowsResource.pageSize = 10;
    this.workflowsResource.orderBy = 'workflowInstanceStatus asc';
  }

  ngOnInit(): void {
    this.getWorkflows();
    this.getAllDocuments();
    this.getWorkflowInstanceStatuses();
    this.dataSource = new WorkflowsDataSource(this.workflowInstanceService);
    this.dataSource.loadWorkflows(this.workflowsResource);
    this.getResourceParameter();
    this.sub$.sink = this.signalrService.workItemNotification$.subscribe(() => {
      this.dataSource.loadWorkflows(this.workflowsResource);
    });
  }
  openCreateWorkflowDialog(): void {
    // const screenWidth = window.innerWidth;
    // const dialogWidth = screenWidth < 768 ? '90vw' : '60vw';
    // const dialogRef = this.dialog.open(RequestDocumentThroughWorkflowComponent, {
    //   maxWidth: dialogWidth,
    // });
    // dialogRef.afterClosed().subscribe((result: boolean) => {
    //   if (result) {
    //     this.dataSource.loadWorkflows(this.workflowsResource);
    //   }
    // });
    this.router.navigate(['request_document_through_workflow']);
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
    this.workflowInstanceService.getallDocuments().subscribe((documents: DocumentShort[]) => {
      this.documents = documents;
    });
  }

  getWorkflows() {
    this.workflowInstanceService.getallWorkflows()
      .subscribe((c: Workflow[]) => {
        if (c.length > 0) {
          this.workflows = [...c];
        }
      });
  }
  ngAfterViewInit() {
    this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 0));
    merge(this.sort.sortChange, this.paginator.page)
      .pipe(
        tap(() => {
          this.workflowsResource.skip =
            this.paginator.pageIndex * this.paginator.pageSize;
          this.workflowsResource.pageSize = this.paginator.pageSize;
          this.workflowsResource.orderBy =
            this.sort.active + ' ' + this.sort.direction;
          this.dataSource.loadWorkflows(this.workflowsResource);
        })
      )
      .subscribe();
  }

  onWorkflowStatusChange(filterValue: MatSelectChange) {
    this.workflowsResource.workflowInstanceStatus = filterValue.value;
    this.workflowsResource.workflowInstanceStatus = filterValue.value;
    this.dataSource.loadWorkflows(this.workflowsResource);
  }

  onWorkflowsChanges(filterValue: MatSelectChange) {
    if (filterValue.value) {
      this.workflowsResource.workflowId = filterValue.value;
    } else {
      this.workflowsResource.workflowId = '';
    }
    this.workflowsResource.skip = 0;
    this.paginator.pageIndex = 0;
    this.dataSource.loadWorkflows(this.workflowsResource);
  }

  onDocumentChange(filterValue: MatSelectChange) {
    if (filterValue.value) {
      this.workflowsResource.documentId = filterValue.value;
    } else {
      this.workflowsResource.documentId = '';
    }
    this.workflowsResource.skip = 0;
    this.paginator.pageIndex = 0;
    this.dataSource.loadWorkflows(this.workflowsResource);
  }
  getResourceParameter() {
    this.sub$.sink = this.dataSource.responseHeaderSubject$.subscribe(
      (c: ResponseHeader) => {
        if (c) {
          this.workflowsResource.pageSize = c.pageSize;
          this.workflowsResource.skip = c.skip;
          this.workflowsResource.totalCount = c.totalCount;
        }
      }
    );
  }


  performTransition(transition: CurrentWorkflowTransition, workflowInstance: WorkflowInstanceData): void {
    if (transition.isUploadDocumentVersion) {
      const nextTransition: NextTransition = {
        workflowInstanceId: workflowInstance.workflowInstanceId,
        transitionId: transition.id,
        workflowStepInstanceId: workflowInstance.workflowStepInstanceId,
        comment: '',
        isUploadDocumentVersion: transition.isUploadDocumentVersion,
        isSignatureRequired: transition.isSignatureRequired,
        isUserSignRequired: transition.isUserSignRequired,
        transitionName: transition.name,
        documentId: workflowInstance.documentId,
      }
      const screenWidth = window.innerWidth;
      const dialogWidth = screenWidth < 768 ? '90vw' : '60vw';
      const dialogRef = this.dialog.open(PerformTransitionComponent, {
        maxWidth: dialogWidth,
        data: Object.assign({}, nextTransition),
      });

      dialogRef.afterClosed().subscribe((result: boolean) => {
        if (result) {
          this.dataSource.loadWorkflows(this.workflowsResource);
        }
      });
      return;
    }
    if (!transition.isSignatureRequired) {
      this.performContinueWorkflow(transition, workflowInstance);
    }
    else if (!transition.isSignatureRequired && !transition.isUserSignRequired) {
      this.performContinueWorkflow(transition, workflowInstance);
    }
    else {
      this.commonService.checkDocumentIsSignedByUser(workflowInstance.documentId).subscribe({
        next: (flag: boolean) => {
          if (!flag) {
            const nextTransition: NextTransition = {
              workflowInstanceId: workflowInstance.workflowInstanceId,
              transitionId: transition.id,
              workflowStepInstanceId: workflowInstance.workflowStepInstanceId,
              comment: '',
              isUploadDocumentVersion: false,
              isSignatureRequired: transition.isSignatureRequired,
              isUserSignRequired: transition.isUserSignRequired,
              transitionName: transition.name,
              documentId: workflowInstance.documentId,
            }
            const screenWidth = window.innerWidth;
            const dialogWidth = screenWidth < 768 ? '90vw' : '60vw';
            const dialogRef = this.dialog.open(PerformTransitionComponent, {
              maxWidth: dialogWidth,
              data: Object.assign({}, nextTransition),
            });

            dialogRef.afterClosed().subscribe((result: boolean) => {
              if (result) {
                this.dataSource.loadWorkflows(this.workflowsResource);
              }
            });
          } else {
            this.performContinueWorkflow(transition, workflowInstance);
          }
        },
        error: (error) => {
        }
      });
    }
  }

  performContinueWorkflow(transition: CurrentWorkflowTransition, workflowInstance: WorkflowInstanceData): void {
    this.commonDialogService
      .deleteConfirmWithCommentDialog(
        `${this.translationService.getValue(
          'ARE_YOU_SURE_YOU_WANT_TO_PROCEED_WITH_THIS_WORKFLOW_TRANSITION'
        )}:: ${transition.name} ?`,
      )
      .subscribe((commentFlag: any) => {
        if (commentFlag && commentFlag.flag) {
          const nextTransition: NextTransition = {
            workflowInstanceId: workflowInstance.workflowInstanceId,
            transitionId: transition.id,
            workflowStepInstanceId: workflowInstance.workflowStepInstanceId,
            comment: commentFlag.comment,
            transitionName: transition.name,
            documentId: workflowInstance.documentId,
          }
          this.workflowInstanceService
            .performNextTransition(nextTransition)
            .subscribe({
              next: (data: boolean) => {
                if (data) {
                  this.toastrService.success(
                    `${transition.name} ${this.translationService.getValue(
                      'HAS_BEEN_SUCCESSFULLY_COMPLETED'
                    )}`
                  );
                  this.dataSource.loadWorkflows(this.workflowsResource);
                }
              },
              error: (error) => {
              }
            });
        }
      });
  }


  viewVisualWorkflow(workflowInstance: WorkflowInstanceData): void {
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
            this.dialog.open(VisualWorkflowGraphComponent, {
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
    this.isLoadingResults = true;
    try {
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

  cancelWorkflow(workflowInstance: any): void {
    this.commonDialogService
      .deleteConfirmtionDialog(
        `${this.translationService.getValue(
          'ARE_YOU_SURE_YOU_WANT_TO_CANCEL_THIS_WORKFLOW'
        )}:: ${workflowInstance.workflowName} ?`
      )
      .subscribe((flag: boolean) => {
        if (flag) {
          this.workflowInstanceService
            .cancelWorkflowInstance(workflowInstance.workflowInstanceId)
            .subscribe({
              next: (data: boolean) => {
                if (data) {
                  this.dataSource.loadWorkflows(this.workflowsResource);
                }
              },
              error: (error) => { },
            });
        }
      });
  }
}

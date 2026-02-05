import { Component, inject, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatStepperModule } from '@angular/material/stepper';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { RouterModule } from '@angular/router';
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
import { CommonService } from '@core/services/common.service';
import { ToastrService } from '@core/services/toastr-service';
import { WorkflowInstanceStatus } from '@core/domain-classes/workflow-instance-status.enum';
import { WorkflowStatusColorDirective } from '../workflow-status-color.directive';
import { PerformTransitionComponent } from '../perform-transition/perform-transition.component';
import { SignalrService } from '@core/services/signalr.service';
import { BaseComponent } from '../../base.component';
import { MatPaginatorModule } from '@angular/material/paginator';
import { UTCToLocalTime } from '@shared/pipes/utc-to-localtime.pipe';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { MatCardModule } from '@angular/material/card';
import { NgClass } from '@angular/common';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-manage-current-workflow',
  imports: [
    FormsModule,
    TranslateModule,
    RouterModule,
    MatButtonModule,
    ReactiveFormsModule,
    MatIconModule,
    MatStepperModule,
    MatFormFieldModule,
    MatTableModule,
    MatInputModule,
    WorkflowInstanceStatusPipe,
    WorkflowStatusColorDirective,
    MatPaginatorModule,
    MatTooltipModule,
    UTCToLocalTime,
    PageHelpTextComponent,
    MatCardModule,
    NgClass,
    MatProgressSpinnerModule
  ],
  templateUrl: './manage-current-workflow.component.html',
  styleUrl: './manage-current-workflow.component.scss'
})
export class ManageCurrentWorkflowComponent
  extends BaseComponent
  implements OnInit {
  isLoadingResults = false;
  // @ViewChild(MatPaginator) paginator: MatPaginator;
  workflowInstances: WorkflowInstanceData[] = [];
  WorkflowInstanceStatus = WorkflowInstanceStatus;
  displayedColumns: string[] = [
    'transition',
    'updatedAt',
    'workflowname',
    'workflowstatus',
    'workflowInitiatedDate',
    'initiatedUser',
    'documentname',
    'lastTransition',
    'updatedAt',
    'performBy',
  ];
  dataSource: MatTableDataSource<any>;
  footerToDisplayed = ['footer'];
  private dialog = inject(MatDialog);
  public overlay = inject(OverlayPanel);
  public workflowInstanceService = inject(WorkflowInstanceService);
  public commonDialogService = inject(CommonDialogService);
  public commonService = inject(CommonService);
  public toastrService = inject(ToastrService);
  private signalrService = inject(SignalrService);
  positionOptions: TooltipPosition[] = ['below', 'above', 'left', 'right'];

  ngOnInit(): void {
    // this.dataSource = new MatTableDataSource(this.workflowInstances);
    // this.dataSource.paginator = this.paginator;
    this.getWorkflows();
    this.sub$.sink = this.signalrService.workItemNotification$.subscribe(() => {
      this.getWorkflows();
    });
  }

  ngAfterViewInit() {
    //this.dataSource.paginator = this.paginator;
  }

  getWorkflows(): void {
    this.sub$.sink = this.workflowInstanceService
      .getCurrentWorkflowInstances()
      .subscribe(
        {
          next: (data: WorkflowInstanceData[]) => {
            this.workflowInstances = data;
            // this.dataSource.data = data;
          },
          error: (error) => {
            // Optionally handle error here
          }
        }
      );
  }

  performTransition(
    transition: CurrentWorkflowTransition,
    workflowInstance: WorkflowInstanceData
  ): void {
    if (transition.isUploadDocumentVersion) {
      const nextTransition: NextTransition = {
        workflowInstanceId: workflowInstance.workflowInstanceId,
        transitionId: transition.id,
        documentId: workflowInstance.documentId,
        workflowStepInstanceId: workflowInstance.workflowStepInstanceId,
        comment: '',
        isUploadDocumentVersion: transition.isUploadDocumentVersion,
        isSignatureRequired: transition.isSignatureRequired,
        isUserSignRequired: transition.isUserSignRequired,
        transitionName: transition.name,
      };
      const screenWidth = window.innerWidth;
      const dialogWidth = screenWidth < 768 ? '80vw' : '60vw';
      const dialogRef = this.dialog.open(PerformTransitionComponent, {
        width: dialogWidth,
        data: Object.assign({}, nextTransition),
      });

      dialogRef.afterClosed().subscribe((result: boolean) => {
        if (result) {
          this.getWorkflows();
        }
      });
      return;
    }

    if (!transition.isSignatureRequired) {
      this.performContinueWorkflow(transition, workflowInstance);
    } else if (
      !transition.isSignatureRequired &&
      !transition.isUserSignRequired
    ) {
      this.performContinueWorkflow(transition, workflowInstance);
    } else {
      this.commonService
        .checkDocumentIsSignedByUser(workflowInstance.documentId)
        .subscribe({
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
              };
              const dialogRef = this.dialog.open(PerformTransitionComponent, {
                data: Object.assign({}, nextTransition),
              });

              dialogRef.afterClosed().subscribe((result: boolean) => {
                if (result) {
                  this.getWorkflows();
                }
              });
            } else {
              this.performContinueWorkflow(transition, workflowInstance);
            }
          },
          error: (error) => { },
        });
    }
  }

  performContinueWorkflow(
    transition: CurrentWorkflowTransition,
    workflowInstance: WorkflowInstanceData
  ): void {
    this.commonDialogService
      .deleteConfirmWithCommentDialog(
        `${this.translationService.getValue(
          'ARE_YOU_SURE_YOU_WANT_TO_PROCEED_WITH_THIS_WORKFLOW_TRANSITION'
        )}:: ${transition.name} ?`
      )
      .subscribe((commentFlag: any) => {
        if (commentFlag && commentFlag.flag) {
          const nextTransition: NextTransition = {
            workflowInstanceId: workflowInstance.workflowInstanceId,
            transitionId: transition.id,
            workflowStepInstanceId: workflowInstance.workflowStepInstanceId,
            comment: commentFlag.comment,
            documentId: workflowInstance.documentId,
          };
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
                  this.getWorkflows();
                }
              },
              error: (error) => { },
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

  onPageChanged(event: any): void {
    const pageIndex = event.pageIndex;
    const pageSize = event.pageSize;
  }
}

import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { DashboradService } from '../dashboard.service';
import { WorkflowInstanceService } from '../../workflows/workflow-instance.service';
import { MatDialog } from '@angular/material/dialog';
import { WorkflowInstanceData } from '@core/domain-classes/workflow-instance-data';
import { CurrentWorkflowTransition } from '@core/domain-classes/current-workflow-transition';
import { NextTransition } from '@core/domain-classes/next-transition';
import { VisualWorkflowInstance } from '@core/domain-classes/visual-workflow-instance';
import { DocumentView } from '@core/domain-classes/document-view';
import { CommonDialogService } from '@core/common-dialog/common-dialog.service';
import { CommonService } from '@core/services/common.service';
import { ToastrService } from '@core/services/toastr-service';
import { OverlayPanel } from '@shared/overlay-panel/overlay-panel.service';
import { WorkflowInstanceStatus } from '../../core/domain-classes/workflow-instance-status.enum';
import { MatTooltipModule, TooltipPosition } from '@angular/material/tooltip';
import { SignalrService } from '@core/services/signalr.service';
import { BaseComponent } from '../../base.component';
import { PerformTransitionComponent } from '../../workflows/perform-transition/perform-transition.component';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { HasClaimDirective } from '@shared/has-claim.directive';
import { NGX_ECHARTS_CONFIG, NgxEchartsModule } from 'ngx-echarts';
import { TranslateModule } from '@ngx-translate/core';
import { MatButtonModule } from '@angular/material/button';
import { NgClass } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-document-by-category-chart',
  templateUrl: './document-by-category-chart.component.html',
  styleUrls: ['./document-by-category-chart.component.scss'],
  standalone: true,
  imports: [
    MatPaginator,
    MatTableModule,
    PageHelpTextComponent,
    HasClaimDirective,
    MatTooltipModule,
    NgxEchartsModule,
    TranslateModule,
    MatButtonModule,
    NgClass,
    MatIconModule,
    MatCardModule,
    MatProgressSpinnerModule
  ],
  providers: [
    {
      provide: NGX_ECHARTS_CONFIG,
      useValue: {
        echarts: () => import('echarts'),
      },
    }
  ],
})
export class DocumentByCategoryChartComponent
  extends BaseComponent
  implements OnInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  workflowInstances: WorkflowInstanceData[] = [];
  WorkflowInstanceStatus = WorkflowInstanceStatus;
  isLoadingResults = false;

  displayedColumns: string[] = [
    'detail',
    'workflowname',
    'documentname',
    'transition',
  ];
  dataSource: MatTableDataSource<any>;
  isLoading: boolean = true;
  private dashboardService = inject(DashboradService);
  private workflowInstanceService = inject(WorkflowInstanceService);
  private signalrService = inject(SignalrService);
  private dialog = inject(MatDialog);
  private commonDialogService = inject(CommonDialogService);
  private commonService = inject(CommonService);
  private toastrService = inject(ToastrService);
  public overlay = inject(OverlayPanel);
  positionOptions: TooltipPosition[] = ['below', 'above', 'left', 'right'];

  echartsInstance = null;

  barChartOptions: any = {
    title: {
      text: '',
      left: 'center',
    },
    tooltip: {
      trigger: 'axis',
      axisPointer: {
        type: 'shadow',
      },
    },
    grid: {
      left: '3%',
      right: '4%',
      bottom: '5%',
      containLabel: true,
    },
    xAxis: {
      type: 'category',
      data: [],
      axisTick: {
        alignWithLabel: true,
      },
      axisLabel: {
        interval: 0,
        rotate: 45, // Rotate labels if necessary to avoid overlap
        formatter: (value: string) => value,
      },
    },
    yAxis: {
      type: 'value',
      min: 0,
      max: 50,
      interval: 10,
      axisLabel: {
        formatter: (value: number) => {
          return [0, 10, 20, 30, 40, 50].includes(value)
            ? value.toString()
            : '';
        },
      },
    },
    series: [
      {
        name: 'Documents',
        type: 'bar',
        barWidth: '60%',
        data: [],
        itemStyle: {
          color: '#4252b1',
        },
      },
    ],
  };

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource(this.workflowInstances);
    this.dataSource.paginator = this.paginator;
    this.getDocumentCategoryChartData();
    this.getWorkflows();
    this.sub$.sink = this.signalrService.workItemNotification$.subscribe(() => {
      this.getWorkflows();
    });
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  getDocumentCategoryChartData() {
    this.dashboardService.getDocumentByCategory().subscribe({
      next: (data) => {
        this.isLoading = false;
        const categories = data.map((c) => c.categoryName);
        const values = data.map((c) => c.documentCount);

        this.barChartOptions = {
          ...this.barChartOptions,
          xAxis: {
            ...this.barChartOptions.xAxis,
            data: categories,
          },
          series: [
            {
              ...this.barChartOptions.series[0],
              data: values,
            },
          ],
        };
      },
      error: () => {
        this.isLoading = false;
      },
    });
  }

  updateChartData(newData: any) {
    this.barChartOptions = {
      ...this.barChartOptions,
      series: [
        {
          ...this.barChartOptions.series[0],
          data: newData, // Updated series data
        },
      ],
    };
  }

  onChartInit(ec: any) {
    this.echartsInstance = ec;
  }

  getWorkflows(): void {
    this.sub$.sink = this.workflowInstanceService
      .getCurrentWorkflowInstances()
      .subscribe({
        next: (data: WorkflowInstanceData[]) => {
          this.workflowInstances = data;
          this.dataSource.data = data;
        },
        error: (error) => { },
      });
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
        if (commentFlag.flag) {
          const nextTransition: NextTransition = {
            workflowInstanceId: workflowInstance.workflowInstanceId,
            transitionId: transition.id,
            workflowStepInstanceId: workflowInstance.workflowStepInstanceId,
            comment: commentFlag.comment,
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
              minWidth: dialogWidth,
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

  onPageChanged(event: PageEvent): void {
    const pageIndex = event.pageIndex;
    const pageSize = event.pageSize;
  }
}

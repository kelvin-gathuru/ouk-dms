import { Component, inject, OnInit } from '@angular/core';
import { BaseComponent } from '../base.component';
import { DashboradService } from './dashboard.service';
import { SecurityService } from '@core/security/security.service';
import { NGX_ECHARTS_CONFIG, NgxEchartsModule } from 'ngx-echarts';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { DashboardStats } from '@core/domain-classes/dashboard-stats';
import { DashboardChartData } from '@core/domain-classes/dashboard-chart-data';
import { DashboardRecentActivity } from '@core/domain-classes/dashboard-recent-activity';
import { DocumentByCategory } from '@core/domain-classes/document-by-category';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { OverlayPanel } from '@shared/overlay-panel/overlay-panel.service';
import { DocumentService } from '../document/document.service';
import { WorkflowInstanceService } from '../workflows/workflow-instance.service';
import { ToastrService } from '@core/services/toastr-service';
import { Router } from '@angular/router';
import { DocumentInfo } from '@core/domain-classes/document-info';
import { DocumentView } from '@core/domain-classes/document-view';
import { VisualWorkflowInstance } from '@core/domain-classes/visual-workflow-instance';
import { WorkflowShortDetail } from '@core/domain-classes/workflow-short-detail';
import { WorkflowInstanceStatus } from '@core/domain-classes/workflow-instance-status.enum';
import { WorkflowInstance } from '@core/domain-classes/workflow-instance';
import { CategoryService } from '@core/services/category.service';
import { DocumentStatusService } from '../document-status/document-status.service';
import { ClientStore } from '../client/client-store';
import { DocumentCategoryStatus } from '@core/domain-classes/document-category';
import { Category } from '@core/domain-classes/category';
import { DocumentStatus } from '../document-status/document-status';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTooltipModule, TooltipPosition } from '@angular/material/tooltip';
import { CommonService } from '@core/services/common.service';
import { HttpEvent, HttpEventType } from '@angular/common/http';
import { CurrentWorkflowTransition } from '@core/domain-classes/current-workflow-transition';
import { NextTransition } from '@core/domain-classes/next-transition';
import { CommonDialogService } from '@core/common-dialog/common-dialog.service';
import { WorkflowStatusColorDirective } from '../workflows/workflow-status-color.directive';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
  standalone: true,
  imports: [
    NgxEchartsModule,
    CommonModule,
    TranslateModule,
    MatIconModule,
    MatButtonModule,
    MatMenuModule,
    MatProgressSpinnerModule,
    MatTooltipModule,
    MatDialogModule,
    WorkflowStatusColorDirective
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
export class DashboardComponent extends BaseComponent implements OnInit {
  private dashboardService = inject(DashboradService);
  private securityService = inject(SecurityService);
  private dialog = inject(MatDialog);
  private overlay = inject(OverlayPanel);
  private documentService = inject(DocumentService);
  private workflowInstanceService = inject(WorkflowInstanceService);
  private toastrService = inject(ToastrService);
  private router = inject(Router);
  private categoryService = inject(CategoryService);
  private documentStatusService = inject(DocumentStatusService);
  private commonService = inject(CommonService);
  private commonDialogService = inject(CommonDialogService);
  public clientStore = inject(ClientStore);

  user: any;
  firstName: string = '';
  isLoadingResults = false;
  categories: Category[] = [];
  documentStatuses: DocumentStatus[] = [];
  positionOptions: TooltipPosition[] = ['below', 'above', 'left', 'right'];
  WorkflowInstanceStatus = WorkflowInstanceStatus;

  // Dynamic Data
  stats: any[] = [];
  assignedWorkflows: any[] = [];

  // Charts
  documentTrendOptions: any;
  categoryChartOptions: any;
  folderChartOptions: any;

  ngOnInit() {
    this.user = this.securityService.getUserDetail();
    this.firstName = this.user?.firstName || 'User';
    this.loadDashboardData();
    this.getCategories();
    this.getDocumentStatus();
  }

  loadDashboardData() {
    // 1. Stats
    this.dashboardService.getDashboardStats().subscribe(data => {
      this.updateStats(data);
    });

    // 2. Trend Chart
    this.dashboardService.getDocumentActivity().subscribe(data => {
      this.updateTrendChart(data);
    });

    // 3. Assigned Workflows
    this.dashboardService.getAssignedWorkflows().subscribe(data => {
      this.assignedWorkflows = data;
    });

    // 4. Categories for Charts (Doughnut & Bar)
    this.getCategoryChartData();
  }

  updateStats(data: DashboardStats) {
    // Coral Theme & Style from Screenshot
    this.stats = [
      {
        label: 'Total Documents',
        value: data.totalDocuments,
        icon: 'content_copy',
        type: 'solid',
        trendLabel: data.totalDocumentsGrowth || '0 Increased from last month'
      },
      {
        label: 'Assigned Documents',
        value: data.assignedDocuments,
        icon: 'assignment_ind',
        type: 'outlined',
        trendLabel: data.assignedDocumentsGrowth || '0 Increased from last month'
      },
      {
        label: 'Pending Reviews',
        value: data.pendingReviews,
        icon: 'rate_review',
        type: 'outlined',
        trendLabel: data.pendingReviewsFooter || '0 Pending approval'
      },
      {
        label: 'Assigned Tasks',
        value: data.assignedWorkflows,
        icon: 'assignment_turned_in',
        type: 'outlined',
        trendLabel: data.assignedWorkflowsFooter || '0 Tasks completed'
      }
    ];
  }

  updateTrendChart(data: DashboardChartData[]) {
    this.documentTrendOptions = {
      tooltip: { trigger: 'axis' },
      grid: { left: '3%', right: '4%', bottom: '3%', containLabel: true },
      xAxis: {
        type: 'category',
        boundaryGap: false,
        data: data.map(i => i.label),
        axisLine: { lineStyle: { color: '#ccc' } }
      },
      yAxis: { type: 'value', axisLine: { show: false }, splitLine: { lineStyle: { type: 'dashed' } } },
      series: [{
        name: 'Documents',
        type: 'line',
        smooth: true,
        symbol: 'none',
        lineStyle: { color: '#037b90', width: 3 },
        areaStyle: {
          color: {
            type: 'linear', x: 0, y: 0, x2: 0, y2: 1,
            colorStops: [{ offset: 0, color: '#037b90' }, { offset: 1, color: 'rgba(3, 123, 144, 0.05)' }]
          }
        },
        data: data.map(i => i.count)
      }]
    };
  }

  getCategoryChartData() {
    this.dashboardService.getDocumentByCategory().subscribe(cats => {
      // 1. Bar Chart (Documents by Category)
      this.categoryChartOptions = {
        tooltip: { trigger: 'axis', axisPointer: { type: 'shadow' } },
        grid: { left: '3%', right: '4%', bottom: '15%', containLabel: true },
        xAxis: {
          type: 'category',
          data: cats.map(c => c.categoryName),
          axisTick: { alignWithLabel: true },
          axisLabel: { rotate: 45, interval: 0, color: '#666' }
        },
        yAxis: { type: 'value', splitLine: { lineStyle: { type: 'dashed' } } },
        series: [{
          name: 'Documents',
          type: 'bar',
          barWidth: '40%',
          itemStyle: { color: '#ff7f50', borderRadius: [4, 4, 0, 0] },
          data: cats.map(c => c.documentCount)
        }]
      };

      // 2. Doughnut Chart (Documents by Folder)
      this.folderChartOptions = {
        tooltip: { trigger: 'item' },
        legend: { bottom: '0%', left: 'center' },
        series: [
          {
            name: 'Documents by Folder',
            type: 'pie',
            radius: ['40%', '60%'],
            center: ['50%', '40%'],
            avoidLabelOverlap: true,
            itemStyle: {
              borderRadius: 10,
              borderColor: '#fff',
              borderWidth: 2
            },
            label: {
              show: true,
              position: 'outside'
            },
            emphasis: {
              label: {
                show: true,
                fontSize: 20,
                fontWeight: 'bold'
              }
            },
            labelLine: {
              show: true
            },
            data: cats.map(c => ({ value: c.documentCount, name: c.categoryName }))
          }
        ]
      };
    });
  }

  getCategories(): void {
    this.categoryService.getAllCategories().subscribe((c) => {
      this.categories = [...c];
    });
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

  async onDocumentView(task: any) {
    this.isLoadingResults = true;
    try {
      const documentView: DocumentView = {
        documentId: task.documentId,
        name: task.documentName,
        url: task.documentUrl,
        extension: task.documentUrl?.split('.').pop(),
        isVersion: false,
        isFromPublicPreview: false,
        isPreviewDownloadEnabled: this.securityService.hasClaim(
          'ALL_DOWNLOAD_DOCUMENT'
        ),
        isFileRequestDocument: false,
        isSignatureExists: false, // Might need to fetch actual status if important
        isChunk: false,
        documentNumber: task.documentNumber,
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
    } finally {
      this.isLoadingResults = false;
    }
  }

  viewVisualWorkflow(task: any): void {
    this.workflowInstanceService
      .getvisualWorkflowInstance(task.workflowInstanceId)
      .subscribe({
        next: async (data: VisualWorkflowInstance) => {
          this.isLoadingResults = true;
          try {
            const { VisualWorkflowGraphComponent } = await import(
              '../workflows/visual-workflow-graph/visual-workflow-graph.component'
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

  async manageWorkflowInstance(task: any) {
    this.isLoadingResults = true;
    try {
      // We need proper DocumentInfo for this. For now we construct it from task data.
      const documentInfo: DocumentInfo = {
        id: task.documentId,
        name: task.documentName,
        documentNumber: task.documentNumber,
        url: task.documentUrl,
        createdDate: new Date(), // Dummy
        createdBy: '',
      };

      const documentCategories: DocumentCategoryStatus = {
        document: documentInfo,
        categories: this.categories,
        documentStatuses: this.documentStatuses,
        clients: this.clientStore.clients(),
      };
      const { ManageWorkflowInstanceComponent } = await import(
        '../workflows/manage-workflow-instance/manage-workflow-instance.component'
      );
      const dialogRef = this.dialog.open(ManageWorkflowInstanceComponent, {
        data: Object.assign({}, documentCategories),
      });

      dialogRef.afterClosed().subscribe((result: WorkflowInstance) => {
        if (result && result?.workflowId) {
          this.loadDashboardData();
        }
      });
    } finally {
      this.isLoadingResults = false;
    }
  }

  downloadDocument(task: any) {
    this.isLoadingResults = true;
    const docuView: DocumentView = {
      documentId: task.documentId,
      name: task.documentName,
      extension: task.documentUrl?.split('.').pop() || '',
      isVersion: false,
      isFromPublicPreview: false,
      isPreviewDownloadEnabled: false,
      isFileRequestDocument: false,
      isSignatureExists: false,
      documentNumber: task.documentNumber,
    };

    this.commonService.downloadDocument(docuView).subscribe({
      next: (event: HttpEvent<Blob>) => {
        if (event.type === HttpEventType.Response) {
          this.isLoadingResults = false;
          if (event.body) {
            const downloadedFile = new Blob([event.body], {
              type: event.body.type,
            });
            const url = window.URL.createObjectURL(downloadedFile);
            const a = document.createElement('a');
            a.href = url;
            a.download = task.documentName;
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
            window.URL.revokeObjectURL(url);
          }
        }
      },
      error: (error: any) => {
        this.isLoadingResults = false;
        this.toastrService.error('Error downloading document');
      },
    });
  }

  performTransition(transition: CurrentWorkflowTransition, task: any) {
    this.isLoadingResults = true;
    if (transition.isUploadDocumentVersion) {
      this.openPerformTransitionDialog(transition, task);
      return;
    }

    if (!transition.isSignatureRequired) {
      this.performContinueWorkflow(transition, task);
    } else {
      this.commonService.checkDocumentIsSignedByUser(task.documentId).subscribe({
        next: (flag: boolean) => {
          if (!flag) {
            this.openPerformTransitionDialog(transition, task);
          } else {
            this.performContinueWorkflow(transition, task);
          }
        },
        error: () => {
          this.isLoadingResults = false;
        }
      });
    }
  }

  async openPerformTransitionDialog(transition: CurrentWorkflowTransition, task: any) {
    const nextTransition: NextTransition = {
      workflowInstanceId: task.workflowInstanceId,
      transitionId: transition.id,
      documentId: task.documentId,
      workflowStepInstanceId: task.workflowStepInstanceId,
      comment: '',
      isUploadDocumentVersion: transition.isUploadDocumentVersion,
      isSignatureRequired: transition.isSignatureRequired,
      isUserSignRequired: transition.isUserSignRequired,
      transitionName: transition.name,
    };

    const { PerformTransitionComponent } = await import(
      '../workflows/perform-transition/perform-transition.component'
    );

    const dialogRef = this.dialog.open(PerformTransitionComponent, {
      width: window.innerWidth < 768 ? '90vw' : '60vw',
      data: nextTransition
    });

    dialogRef.afterClosed().subscribe((result) => {
      this.isLoadingResults = false;
      if (result) {
        this.loadDashboardData(); // Reload data to reflect changes
      }
    });
  }

  performContinueWorkflow(transition: CurrentWorkflowTransition, task: any) {
    this.isLoadingResults = false; // Reset loading as dialog will show
    this.commonDialogService
      .deleteConfirmWithCommentDialog(
        `${this.translationService.getValue(
          'ARE_YOU_SURE_YOU_WANT_TO_PROCEED_WITH_THIS_WORKFLOW_TRANSITION'
        )}:: ${transition.name} ?`
      )
      .subscribe((commentFlag: any) => {
        if (commentFlag && commentFlag.flag) {
          this.isLoadingResults = true;
          const nextTransition: NextTransition = {
            workflowInstanceId: task.workflowInstanceId,
            transitionId: transition.id,
            workflowStepInstanceId: task.workflowStepInstanceId,
            comment: commentFlag.comment,
            documentId: task.documentId,
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
                  this.loadDashboardData();
                }
                this.isLoadingResults = false;
              },
              error: () => {
                this.isLoadingResults = false;
              },
            });
        }
      });
  }
}

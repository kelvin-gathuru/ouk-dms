import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { Workflow } from '@core/domain-classes/workflow';
import { MatTableModule } from '@angular/material/table';
import { TranslationService } from '@core/services/translation.service';
import { WorkflowStore } from '../workflow-store';
import { CommonDialogService } from '@core/common-dialog/common-dialog.service';
import { SignalrService } from '@core/services/signalr.service';
import { MatDialog } from '@angular/material/dialog';
import { WorkflowService } from '../workflow.service';
import { VisualWorkflowInstance } from '@core/domain-classes/visual-workflow-instance';
import { WorkflowGraphComponent } from '../workflow-graph/workflow-graph.component';
import { ToastrService } from '@core/services/toastr-service';
import { HasClaimDirective } from '@shared/has-claim.directive';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';


@Component({
  selector: 'app-workflow-list',
  imports: [
    FormsModule,
    TranslateModule,
    RouterModule,
    MatTableModule,
    HasClaimDirective,
    PageHelpTextComponent,
    MatIconModule,
    MatCardModule,
    MatButtonModule
  ],
  templateUrl: './workflow-list.component.html',
  styleUrl: './workflow-list.component.scss'
})
export class WorkflowListComponent implements OnInit, OnDestroy {

  workflows: Workflow[] = [];
  displayedColumns: string[] = ['action', 'name', 'description', 'setupStatus'];

  private dialog = inject(MatDialog);
  public workflowStore = inject(WorkflowStore);
  private commonDialogService = inject(CommonDialogService);
  private translationService = inject(TranslationService);
  private signalrService = inject(SignalrService);
  private workflowService = inject(WorkflowService);
  private toastrService = inject(ToastrService);

  ngOnInit(): void {
    this.getWorkflows();
    this.signalrService.refreshWorkflowSettings$.subscribe(() => {
      this.getWorkflows();
    });
  }

  getWorkflows(): void {
    this.workflowStore.loadWorkflows();
  }

  deleteWorkflow(workflow: Workflow): void {
    this.commonDialogService
      .deleteConfirmtionDialog(`${this.translationService.getValue('ARE_YOU_SURE_YOU_WANT_TO_DELETE')} ${workflow.name}`)
      .subscribe((flag: boolean) => {
        if (flag) {
          this.workflowStore.deleteWorkflowById(workflow.id ?? '');
        }
      });
  }

  viewVisualWorkflow(workflow: Workflow): void {
    this.workflowService.getvisualWorkflow(workflow.id ?? '').subscribe({
      next: (data: VisualWorkflowInstance) => {
        const screenWidth = window.innerWidth;
        const dialogWidth = screenWidth < 768 ? '90vw' : '90vw';

        const dialogRef = this.dialog.open(WorkflowGraphComponent, {
          maxWidth: dialogWidth,
          data: { ...data },
        });
      },
      error: (error) => {
        console.error('Error loading workflow:', error);
      }
    });
  }

  ngOnDestroy(): void {
    this.workflowStore.isLoading() === false;
    this.workflowStore.setCurrentStep(0);
    this.workflowStore.commonError === null;
  }

}

import { Component, Inject, OnInit } from '@angular/core';
import {
  MAT_DIALOG_DATA,
  MatDialogModule,
  MatDialogRef,
} from '@angular/material/dialog';
import {
  ReactiveFormsModule,
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { MatSelectModule } from '@angular/material/select';
import { Workflow } from '@core/domain-classes/workflow';
import { DocumentInfo } from '@core/domain-classes/document-info';
import { WorkflowService } from '../workflow.service';
import { WorkflowInstanceService } from '../workflow-instance.service';
import { WorkflowInstance } from '@core/domain-classes/workflow-instance';

import { ToastrService } from '@core/services/toastr-service';
import { TranslationService } from '@core/services/translation.service';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { TranslateModule } from '@ngx-translate/core';
import { CommonError } from '@core/error-handler/common-error';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-manage-workflow-instance',
  imports: [
    MatDialogModule,
    ReactiveFormsModule,
    MatSelectModule,
    PageHelpTextComponent,
    TranslateModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule
  ],
  templateUrl: './manage-workflow-instance.component.html',
  styleUrl: './manage-workflow-instance.component.scss'
})
export class ManageWorkflowInstanceComponent implements OnInit {
  workflowInstanceForm: UntypedFormGroup;
  workflows: Workflow[] = [];

  constructor(
    private fb: UntypedFormBuilder,
    private toastrService: ToastrService,
    private translationService: TranslationService,
    private dialogRef: MatDialogRef<ManageWorkflowInstanceComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { document: DocumentInfo },
    private workflowService: WorkflowService,
    private workflowInstanceService: WorkflowInstanceService
  ) { }
  ngOnInit(): void {
    this.createForm();
    this.getWorkflows();
  }

  createForm(): void {
    this.workflowInstanceForm = this.fb.group({
      workflowId: ['', [Validators.required]],
      documentId: [this.data.document.id],
    });
  }



  getWorkflows(): void {
    this.workflowService.getWorkflowsNotStarted(this.data.document.id ?? '')
      .subscribe(
        {
          next: (w: Workflow[] | CommonError) => {
            if (Array.isArray(w)) {
              this.workflows = [...w];
            }
          },
          error: () => {
            this.toastrService.error(
              this.translationService.getValue('WORKFLOW_FETCH_FAILED')
            );
          }
        });
  }

  saveWorkflowInstance(): void {
    if (this.workflowInstanceForm.invalid) {
      this.workflowInstanceForm.markAllAsTouched();
      return;
    }
    const workflowId = this.workflowInstanceForm.get('workflowId')?.value;
    const workflowInstance: WorkflowInstance = {
      workflowId: workflowId,
      documentId: this.data.document.id ?? '',
    };
    this.workflowInstanceService
      .addWorkflowInstance(workflowInstance)
      .subscribe({
        next: (workflowInstance) => {
          const isSuccess = workflowInstance as WorkflowInstance;
          if (isSuccess) {
            this.toastrService.success(
              this.translationService.getValue('WORKFLOW_CREATED_SUCCESSFULLY')
            );
            this.dialogRef.close(workflowInstance);
          }
        },
        error: (error) => {
          this.toastrService.error(
            this.translationService.getValue('WORKFLOW_CREATION_FAILED')
          );
        }
      });
  }

  closeDialog() {
    this.dialogRef.close();
  }
}

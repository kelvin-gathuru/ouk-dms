import { Component, ContentChild, effect, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { WorkflowStore } from '../../workflow-store';
import { ManageStepComponent } from '../manage-step/manage-step.component';
import { ManageTransitionComponent } from '../manage-transition/manage-transition.component';
import { Workflow } from '@core/domain-classes/workflow';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-create-workflow',
  imports: [
    RouterModule,
    ReactiveFormsModule,
    TranslateModule,
    PageHelpTextComponent,
    MatIconModule,
    MatButtonModule,
    MatCardModule
  ],
  templateUrl: './create-workflow.component.html',
  styleUrl: './create-workflow.component.scss'
})
export class CreateWorkflowComponent implements OnInit {
  private fb = inject(FormBuilder);
  public workflowStore = inject(WorkflowStore);
  private router = inject(Router);
  workFormGroup: FormGroup;
  isLoading = false;
  currentWorkflow = this.workflowStore.currentWorkflow();
  currentStep = 0;
  @ContentChild(ManageStepComponent) manageStepComponent!: ManageStepComponent;
  @ContentChild(ManageTransitionComponent) manageTransitionComponent!: ManageTransitionComponent;

  constructor() {
    effect(() => {
      const nextStep = this.workflowStore.currentStep();
      if (nextStep !== this.currentStep) {
        this.currentStep = nextStep;
        this.goToWorkflowStep();
      }
    });
  }
  ngOnInit(): void {
    this.createFirstFormGroup();
    if (this.currentWorkflow) {
      this.workFormGroup.patchValue(this.currentWorkflow);
    }
  }

  createFirstFormGroup(): void {
    this.workFormGroup = this.fb.group({
      id: [''],
      name: ['', Validators.required],
      description: ['', Validators.required],
    });
  }

  goToWorkflowStep() {
    this.router.navigate(['/workflow-settings/manage/manage-steps']);
  }

  saveWorkflow(): void {
    if (this.workFormGroup.invalid) {
      this.workFormGroup.markAllAsTouched();
      return;
    }
    const workflow: Workflow = this.createWorkflowBuildObject();
    if (workflow.id) {
      this.workflowStore.updateWorkflow(workflow);
    } else {
      this.workflowStore.addWorkflow(workflow);
    }
  }

  createWorkflowBuildObject(): Workflow {
    return {
      id: this.workFormGroup.get('id')?.value,
      name: this.workFormGroup.get('name')?.value,
      description: this.workFormGroup.get('description')?.value,
      isWorkflowSetup: false,
      workflowSteps: [],
      workflowTransitions: [],
      workflowInstances:[]
    };
  }

}

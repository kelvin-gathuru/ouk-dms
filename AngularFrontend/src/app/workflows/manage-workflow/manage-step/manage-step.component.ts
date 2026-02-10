import { Component, computed, effect, inject, OnInit } from '@angular/core';
import {
  FormGroup,
  FormBuilder,
  ReactiveFormsModule,
  Validators,
  FormArray,
} from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatStepperModule } from '@angular/material/stepper';
import { RouterModule } from '@angular/router';
import { WorkflowStep } from '@core/domain-classes/workflow-step';
import { TranslateModule } from '@ngx-translate/core';
import { WorkflowStore } from '../../workflow-store';
import { ActivatedRoute, Router } from '@angular/router';

import { ToastrService } from '@core/services/toastr-service';
import { TranslationService } from '@core/services/translation.service';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-manage-step',
  imports: [
    RouterModule,
    ReactiveFormsModule,
    TranslateModule,
    MatIconModule,
    MatStepperModule,
    PageHelpTextComponent,
    MatCardModule,
    MatButtonModule,
  ],
  templateUrl: './manage-step.component.html',
  styleUrl: './manage-step.component.scss'
})
export class ManageStepComponent implements OnInit {
  private fb = inject(FormBuilder);
  private workflowStore = inject(WorkflowStore);
  private toastrService = inject(ToastrService);
  private translationService = inject(TranslationService);
  private router = inject(Router);

  private route = inject(ActivatedRoute);

  stepFormGroup: FormGroup;
  isLoading = false;
  currentStep = 1;
  currentWorkflow = computed(() => this.workflowStore.currentWorkflow());
  workflowSteps = computed(() => this.currentWorkflow()?.workflowSteps ?? []);
  workflowInstances = computed(() => this.currentWorkflow()?.workflowInstances ?? []);
  isWorkflowSetup = computed(() => this.currentWorkflow()?.isWorkflowSetup);

  get steps(): FormArray {
    return this.stepFormGroup.get('steps') as FormArray;
  }

  constructor() {
    effect(() => {
      const nextStep = this.workflowStore.currentStep();
      if (nextStep !== this.currentStep) {
        this.currentStep = nextStep;
        if (nextStep === 0) {
          this.onPreviousClick();
        } else {
          this.goToWorkflowTransitions();
        }
      }
    });

    effect(() => {
      const steps = this.workflowSteps();
      if (steps.length === 0 && this.steps.length === 0) {
        this.addStep(null);
        this.addStep(null);
      } else if (steps.length > 0 && this.steps.length === 0) {
        steps.forEach((step: WorkflowStep) => {
          this.addStep(step);
        });
      }
    });

  }

  ngOnInit(): void {
    const workflowId = this.route.snapshot.paramMap.get('id');
    if (workflowId && !this.currentWorkflow()?.id) {
      this.workflowStore.getWorkflowById(workflowId);
    }

    this.workflowStore.setCurrentStep(1);
    this.createStepFormGroup();
  }

  goToWorkflowTransitions() {
    this.router.navigate(['/workflow-settings/manage/manage-transitions', this.currentWorkflow()?.id]);
  }

  checkUniqueStepName(index: number) {
    const stepName = this.steps.at(index).get('stepName')?.value;
    const isNotUnique = this.steps.controls.some(
      (control, i) => i !== index && control.get('stepName')?.value === stepName
    );

    if (isNotUnique) {
      this.steps.at(index).get('stepName')?.setErrors({ notUnique: true });
    } else {
      const errors = this.steps.at(index).get('stepName')?.errors;

      if (errors) {

        if (Object.keys(errors).length === 0) {
          this.steps.at(index).get('stepName')?.setErrors(null);
        } else {
          this.steps.at(index).get('stepName')?.setErrors(errors);
        }
      }
    }
  }

  createStepFormGroup(): void {
    this.stepFormGroup = this.fb.group({
      steps: this.fb.array([]),
    });
  }

  addStep(workflowStep?: WorkflowStep | null): void {
    const stepForm = this.fb.group(
      {
        id: [workflowStep?.id],
        stepName: [
          { value: workflowStep?.stepName, disabled: false },
          [Validators.required],
        ],
        workflowId: [workflowStep?.workflowId],
      }
    );
    this.steps.push(stepForm);
  }

  removeStep(index: number): void {
    this.steps.removeAt(index);
  }

  saveWorkflowSteps(): void {
    if (this.stepFormGroup.invalid) {
      this.stepFormGroup.markAllAsTouched();
      return;
    }
    if (this.steps.value.length < 2) {
      this.toastrService.success(
        this.translationService.getValue('ADDED_LEAST_TWO_STEP')
      );
      return;
    }
    this.isLoading = true;
    const stepsData: WorkflowStep[] = this.steps.value.map((step: any) => ({
      ...step,
      workflowId: this.currentWorkflow()?.id ?? this.route.snapshot.paramMap.get('id') ?? '',
    }));
    // if (this.isWorkflowSetup) {
    //   this.workflowStore.updateWorkflowStep(stepsData);
    // } else {
    //   this.workflowStore.addWorkflowStep(stepsData);
    // }
    if (this.workflowInstances().length > 0) {
      this.workflowStore.updateWorkflowStep(stepsData);
    } else {
      this.workflowStore.addWorkflowStep(stepsData);
    }
  }
  onPreviousClick(): void {
    this.workflowStore.setCurrentStep(0);
    this.router.navigate(['/workflow-settings/manage', this.currentWorkflow()?.id ?? this.route.snapshot.paramMap.get('id') ?? '']);
  }
}

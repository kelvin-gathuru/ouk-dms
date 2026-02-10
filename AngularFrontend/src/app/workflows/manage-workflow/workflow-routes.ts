import { Routes } from '@angular/router';
import { createWorkFlowResolver } from './create-workflow-resolver';
import { CreateWorkflowComponent } from './create-workflow/create-workflow.component';
import { ManageStepComponent } from './manage-step/manage-step.component';
import { ManageTransitionComponent } from './manage-transition/manage-transition.component';
export const WORKFLOW_ROUTES: Routes = [
  {
    path: 'manage-steps/:id',
    component: ManageStepComponent,
  },
  {
    path: 'manage-transitions/:id',
    component: ManageTransitionComponent,
  },
  {
    path: ':id',
    resolve: { workflow: createWorkFlowResolver },
    component: CreateWorkflowComponent,
  },
];

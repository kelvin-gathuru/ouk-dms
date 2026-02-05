import { Routes } from '@angular/router';
import { AuthGuard } from '@core/security/auth.guard';

import { DashboardComponent } from './dashboard.component';

export const DASHBOARD_ROUTES: Routes = [
  {
    path: '',
    component: DashboardComponent,
    data: { claimType: 'VIEW_DASHBOARD' },
    canActivate: [AuthGuard]
  }
];



import { AuthGuard } from '@core/security/auth.guard';
import { LoginAuditListComponent } from './login-audit-list/login-audit-list.component';
import { Routes } from '@angular/router';

export const LOGIN_AUDIT_ROUTES: Routes = [
  {
    path: '',
    component: LoginAuditListComponent,
    data: { claimType: 'VIEW_LOGIN_AUDIT_LOGS' },
    canActivate: [AuthGuard]
  }
];



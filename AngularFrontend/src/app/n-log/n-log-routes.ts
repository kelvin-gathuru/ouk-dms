import { AuthGuard } from '@core/security/auth.guard';
import { LogDetailResolverService } from './log-detail-resolver';
import { NLogDetailComponent } from './n-log-detail/n-log-detail.component';
import { NLogListComponent } from './n-log-list/n-log-list.component';
import { Routes } from '@angular/router';

export const NLOG_ROUTES: Routes = [
  {
    path: '',
    component: NLogListComponent,
    data: { claimType: 'VIEW_ERROR_LOGS' },
    canActivate: [AuthGuard],
  },
  {
    path: ':id',
    component: NLogDetailComponent,
    canActivate: [AuthGuard],
    data: { claimType: 'VIEW_ERROR_LOGS' },
    resolve: { log: LogDetailResolverService },
  },
];



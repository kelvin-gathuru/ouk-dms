import { Routes } from '@angular/router';
import { AuthGuard } from '@core/security/auth.guard';
import { NotificationListComponent } from './notification-list/notification-list.component';

export const NOTIFICATION_ROUTES: Routes = [
  {
    path: '',
    component: NotificationListComponent,
    canActivate: [AuthGuard]
  }
];



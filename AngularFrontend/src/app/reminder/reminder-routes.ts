import { Routes } from '@angular/router';
import { ReminderListComponent } from './reminder-list/reminder-list.component';
import { AddReminderComponent } from './add-reminder/add-reminder.component';
import { ReminderDetailResolverService } from './add-reminder/reminder-detail.resolver';
import { AuthGuard } from '@core/security/auth.guard';

export const REMINDER_ROUTES: Routes = [
  {
    path: '',
    component: ReminderListComponent,
    data: { claimType: 'View_Reminders' },
    canActivate: [AuthGuard]
  }, {
    path: 'add',
    component: AddReminderComponent,
    data: { claimType: 'Create_Reminder' },
    canActivate: [AuthGuard]
  }, {
    path: 'manage/:id',
    resolve: { reminder: ReminderDetailResolverService },
    component: AddReminderComponent,
    data: { claimType: 'Edit_Reminder' },
    canActivate: [AuthGuard]
  }
];


import { Routes } from '@angular/router';
import { AuthGuard } from '../core/security/auth.guard';
import { ManageClientComponent } from './manage-client/manage-client.component';
import { ClientResolver } from './client.resolver';

export const CLIENT_ROUTES: Routes = [
  {
    path: '',
    canMatch: [AuthGuard],
    data: { claimType: 'VIEW_CLIENTS' },
    loadComponent: () =>
      import('./client-list/client-list.component').then(
        (m) => m.ClientListComponent
      ),
  },
  {
    path: 'manage',
    canActivate: [AuthGuard],
    component: ManageClientComponent,
  },
  {
    path: 'manage/:id',
    component: ManageClientComponent,
    resolve: { client: ClientResolver },
    canActivate: [AuthGuard],
  }
];

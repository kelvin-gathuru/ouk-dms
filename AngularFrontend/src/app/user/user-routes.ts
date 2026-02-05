import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '@core/security/auth.guard';
import { ManageUserComponent } from './manage-user/manage-user.component';
import { UserDetailResolverService } from './user-detail-resolver';
import { UserListComponent } from './user-list/user-list.component';
import { UserPermissionComponent } from './user-permission/user-permission.component';

export const USER_ROUTES: Routes = [
  {
    path: '',
    component: UserListComponent,
    data: { claimType: 'VIEW_USERS' },
    canActivate: [AuthGuard]
  }, {
    path: 'manage/:id',
    component: ManageUserComponent,
    resolve: { user: UserDetailResolverService },
    data: { claimType: 'EDIT_USER' },
    canActivate: [AuthGuard]
  }, {
    path: 'manage',
    component: ManageUserComponent,
    data: { claimType: 'CREATE_USER' },
    canActivate: [AuthGuard]
  }, {
    path: 'permission/:id',
    component: UserPermissionComponent,
    resolve: { user: UserDetailResolverService },
    data: { claimType: 'ASSIGN_PERMISSION' },
    canActivate: [AuthGuard]
  }
];


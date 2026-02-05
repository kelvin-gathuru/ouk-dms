import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '@core/security/auth.guard';
import { ManageRoleComponent } from './manage-role/manage-role.component';
import { RoleDetailResolverService } from './role-detail.resolver';
import { RoleListComponent } from './role-list/role-list.component';
import { RoleUsersComponent } from './role-users/role-users.component';

export const ROLE_ROUTES: Routes = [
  {
    path: '',
    component: RoleListComponent,
    data: { claimType: 'VIEW_ROLES' },
    canActivate: [AuthGuard]
  }, {
    path: 'manage/:id',
    component: ManageRoleComponent,
    resolve: { role: RoleDetailResolverService },
    data: { claimType: 'EDIT_ROLE' },
    canActivate: [AuthGuard]
  }, {
    path: 'manage',
    component: ManageRoleComponent,
    data: { claimType: 'CREATE_ROLE' },
    canActivate: [AuthGuard]
  }, {
    path: 'users',
    component: RoleUsersComponent,
    data: { claimType: 'ASSIGN_USER_ROLE' },
    canActivate: [AuthGuard]
  }
];



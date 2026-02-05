import { Routes } from '@angular/router';
import { AuthGuard } from '@core/security/auth.guard';
import { CategoryListComponent } from './category-list/category-list.component';

export const CATEGORY_ROUTES: Routes = [
  {
    path: '',
    component: CategoryListComponent,
    data: { claimType: 'VIEW_CATEGORIES' },
    canActivate: [AuthGuard]
  }
];



import { Routes } from '@angular/router';
import { PageHelperListComponent } from './page-helper-list/page-helper-list.component';
import { ManagePageHelperComponent } from './manage-page-helper/manage-page-helper.component';
import { PageHelperDetailResolverService } from './page-helper-detail-resolver';

export const PAGE_HELPER_ROUTES: Routes = [
  {
    path: '',
    component: PageHelperListComponent,
    data: { claimType: 'MANAGE_PAGE_HELPER' },
  }, {
    path: 'manage/:id',
    component: ManagePageHelperComponent,
    resolve: { pageHelper: PageHelperDetailResolverService },
    data: { claimType: 'MANAGE_PAGE_HELPER' },
  }, {
    path: 'manage',
    component: ManagePageHelperComponent,
    data: { claimType: 'MANAGE_PAGE_HELPER' },
  }
];



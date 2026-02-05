import { Routes } from '@angular/router';
import { DocumentPermissionListComponent } from './document-permission-list/document-permission-list.component';

export const DOCUMENT_PERMISSION_ROUTES: Routes = [
  {
    path: ':id',
    component: DocumentPermissionListComponent,
  }
];


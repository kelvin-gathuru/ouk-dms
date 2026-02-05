import { Routes } from '@angular/router';
import { DocumentLibraryListComponent } from './document-library-list/document-library-list.component';
import { DocumentLibraryGuard } from './document-library-guard';
import { TableSettingsComponent } from './table-settings/table-settings.component';

export const DOCUMENT_LIBRARY_ROUTES: Routes = [
  {
    path: '',
    component: DocumentLibraryListComponent,
    canActivate: [DocumentLibraryGuard]
  },
  {
    path: 'table-settings',
    component: TableSettingsComponent,
    canActivate: [DocumentLibraryGuard]
  }
];



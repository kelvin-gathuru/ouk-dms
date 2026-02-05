import { Routes } from '@angular/router';
import { AuthGuard } from '@core/security/auth.guard';
import { DocumentListComponent } from './document-list/document-list.component';
import { DocumentManageResolver } from './document-manage/document-manage-resolver';
import { DocumentGuard } from './document-list/document-guard';

export const DOCUMENT_ROUTES: Routes = [
  {
    path: 'list-view', component: DocumentListComponent,
    data: { claimType: 'ALL_VIEW_DOCUMENTS' },
    canActivate: [AuthGuard, DocumentGuard]
  },
  {
    path: 'deep-search',
    data: { claimType: 'DEEP_SEARCH' },
    canActivate: [AuthGuard],
    loadComponent: () =>
      import(
        './deep-document-search/deep-document-search.component'
      ).then((m) => m.DeepDocumentSearchComponent),
  },
  {
    path: 'add',
    data: { claimType: 'ALL_CREATE_DOCUMENT' },
    canActivate: [AuthGuard],
    loadComponent: () =>
      import(
        './document-manage/document-manage.component'
      ).then((m) => m.DocumentManageComponent),
  },
  {
    path: 'ocr_content_extractor',
    data: { claimType: 'OCR_CONTENT_EXTRACTOR' },
    canMatch: [AuthGuard],
    loadComponent: () =>
      import(
        '../ocr-content-extractor/ocr-content-extractor.component'
      ).then((m) => m.OcrContentExtractorComponent),
  },
  {
    path: 'table-settings',
    loadComponent: () =>
      import(
        '../document/document-list/table-settings/table-settings.component'
      ).then((m) => m.TableSettingsComponent),
  },
  {
    path: 'folder-view',
    data: { claimType: 'All_View_Documents' },
    canMatch: [AuthGuard],
    loadComponent: () =>
      import('../document/folders-view/folders-view.component').then(
        (m) => m.FoldersViewComponent
      ),
  },
  {
    path: ':id',
    loadComponent: () =>
      import(
        './document-manage/document-manage.component'
      ).then((m) => m.DocumentManageComponent),
    resolve: {
      document: DocumentManageResolver
    },
    data: { claimType: 'All_Edit_Document' },
    canActivate: [AuthGuard]
  }, {
    path: 'permission',
    data: { claimType: 'ALL_Share_Document' },
    loadChildren: () =>
      import('./document-permission/document-permission-routes')
        .then(m => m.DOCUMENT_PERMISSION_ROUTES)
  }
];

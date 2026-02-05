import { Routes } from '@angular/router';
import { ManageDocumentMetaTagComponent } from './manage-document-meta-tag/manage-document-meta-tag.component';
import { DocumentMetaTagResolver } from './document-meta-tag.resolver';

export const DOCUMENT_META_TAG_ROUTES: Routes = [
  {
    path: '',
    //canActivate: [AuthGuard],
    loadComponent() {
      return import('./manage-document-meta-tag/manage-document-meta-tag.component').then((m) => m.ManageDocumentMetaTagComponent);
    },  
  },
  {
    path: ':id',
    component: ManageDocumentMetaTagComponent,
    resolve: { documentMetaTag: DocumentMetaTagResolver },
    //canActivate: [AuthGuard],
  },
];

import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '@core/security/auth.guard';
import { DocumentAuditTrailComponent } from './document-audit-trail.component';

export const DOCUMENT_AUDIT_TRAIL_ROUTES: Routes = [
  {
    path: '',
    component: DocumentAuditTrailComponent,
    data: { claimType: 'VIEW_DOCUMENT_AUDIT_TRAIL' },
    canActivate: [AuthGuard]
  }
];


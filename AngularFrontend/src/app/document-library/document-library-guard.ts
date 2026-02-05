import { inject } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivateFn,
  Router,
  RouterStateSnapshot,
} from '@angular/router';
import { DocumentLibraryTableSettingsStore } from './document-library-list/document-library-table-settings-store';

export const DocumentLibraryGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot
) => {
  const router = inject(Router);
  const tableSettingsStore = inject(DocumentLibraryTableSettingsStore);
  tableSettingsStore.loadTableSettingByScreenName("documents_library");
  return true;
};

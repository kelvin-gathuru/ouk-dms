import { inject } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivateFn,
  Router,
  RouterStateSnapshot,
} from '@angular/router';
import { DocumentStore } from './document-store';


export const DocumentGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot
) => {
  const router = inject(Router);
  const tableSettingsStore = inject(DocumentStore);
  tableSettingsStore.loadTableSettingByScreenName("documents");
  return true;
};

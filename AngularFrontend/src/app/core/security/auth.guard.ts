import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  Router,
  Route,
} from '@angular/router';

import { ToastrService } from '@core/services/toastr-service';
import { Observable } from 'rxjs';
import { SecurityService } from './security.service';
import { TranslationService } from '@core/services/translation.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard {
  constructor(
    private securityService: SecurityService,
    private router: Router,
    private toastr: ToastrService,
    private translationService: TranslationService
  ) { }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {
    if (this.securityService.isUserAuthenticate()) {
      let claimType: any = next.data['claimType'];
      if (claimType) {
        if (!this.securityService.hasClaim(claimType)) {
          this.toastr.error(
            this.translationService.getValue('ACCESS_FORBIDDEN')
          );
          this.router.navigate(['/my-profile']);
          return false;
        }
      }
      return true;
    } else {
      this.router.navigate(['/login']);
      return false;
    }
  }

  canActivateChild(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {
    // Get property name on security object to check
    // let claimType: string = next.data['claimType'];
    if (this.securityService.isUserAuthenticate()) {
      let claimType: any = next.data['claimType'];
      if (claimType) {
        if (!this.securityService.hasClaim(claimType)) {
          this.toastr.error(
            this.translationService.getValue('ACCESS_FORBIDDEN')
          );
          return false;
        }
      }
      return true;
    } else {
      this.router.navigate(['login'], {
        queryParams: { returnUrl: state.url },
      });
      return false;
    }
  }
  canLoad(route: Route): boolean {
    if (this.securityService.isUserAuthenticate()) {
      let claimType: any = route.data?.['claimType'];
      if (claimType) {
        if (!this.securityService.hasClaim(claimType)) {
          this.toastr.error(
            this.translationService.getValue('ACCESS_FORBIDDEN')
          );
          this.router.navigate(['/my-profile']);
          return false;
        }
      }
      return true;
    } else {
      this.router.navigate(['/login']);
      return false;
    }
  }
  canMatch(route: Route): boolean {
    if (this.securityService.isUserAuthenticate()) {
      let claimType: any = route.data?.['claimType'];
      if (claimType) {
        if (!this.securityService.hasClaim(claimType)) {
          this.toastr.error(
            this.translationService.getValue('ACCESS_FORBIDDEN')
          );
          this.router.navigate(['/my-profile']);
          return false;
        }
      }
      return true;
    } else {
      this.router.navigate(['/login']);
      return false;
    }
  }
}

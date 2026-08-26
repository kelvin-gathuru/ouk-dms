import { Component, OnInit, Renderer2, AfterViewInit } from '@angular/core';
import {
  ReactiveFormsModule,
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { BaseComponent } from '../base.component';
import { Router, RouterModule } from '@angular/router';
import { UserAuth } from '@core/domain-classes/user-auth';
import { SecurityService } from '@core/security/security.service';

import { ToastrService } from '@core/services/toastr-service';
import { CommonError } from '@core/error-handler/common-error';
import { CommonService } from '@core/services/common.service';
import { environment } from '@environments/environment';
import { TranslateModule } from '@ngx-translate/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  standalone: true,
  imports: [
    ReactiveFormsModule,
    RouterModule,
    TranslateModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule
  ]
})
export class LoginComponent extends BaseComponent implements OnInit, AfterViewInit {
  logoUrl = '/images/ouk-logo.png';
  loginFormGroup: UntypedFormGroup;
  isLoading = false;
  lat: number;
  lng: number;
  googleClientId = environment.googleClientId;
  isGoogleReady = false;

  constructor(
    private fb: UntypedFormBuilder,
    private router: Router,
    private securityService: SecurityService,
    private toastr: ToastrService,
    private renderer: Renderer2,
    private commonService: CommonService
  ) {
    super();
  }

  ngOnInit(): void {
    this.createFormGroup();
    this.getCompanyProfile();
    navigator.geolocation.getCurrentPosition((position) => {
      this.lat = position.coords.latitude;
      this.lng = position.coords.longitude;
    });
  }

  ngAfterViewInit(): void {
    this.setBackgroundImage('/images/ouk-banner.jpg');
    this.initGoogleSignIn();
  }

  private initGoogleSignIn(): void {
    const tryInit = () => {
      const g = (window as any).google;
      if (g && g.accounts && g.accounts.id) {
        g.accounts.id.initialize({
          client_id: this.googleClientId,
          callback: (resp: any) => this.handleGoogleCredential(resp),
          itp_support: true,
        });
        this.isGoogleReady = true;
      } else {
        setTimeout(tryInit, 300);
      }
    };
    tryInit();
  }

  onGoogleSignIn(): void {
    const g = (window as any).google;
    if (!g || !g.accounts || !g.accounts.id) {
      this.toastr.error(this.translationService.getValue('ERROR_FROM_SERVICE'));
      return;
    }
    g.accounts.id.prompt();
  }

  handleGoogleCredential(response: any): void {
    const idToken = response && response.credential;
    if (!idToken) {
      return;
    }
    this.isLoading = true;
    this.sub$.sink = this.securityService.googleLogin(idToken).subscribe({
      next: (c: UserAuth) => {
        this.isLoading = false;
        this.getAllAllowFileExtension();
        this.toastr.success(
          this.translationService.getValue('USER_LOGIN_SUCCESSFULLY')
        );
        if (this.securityService.hasClaim('view_dashboard')) {
          this.router.navigate(['/dashboard']);
        } else if (this.securityService.hasClaim('view_documents')) {
          this.router.navigate(['/documents/list-view']);
        } else {
          this.router.navigate(['/assign/list-view']);
        }
      },
      error: (err: any) => {
        this.isLoading = false;
        if (err?.error?.messages && Array.isArray(err.error.messages)) {
          err.error.messages.forEach((msg: string) => {
            this.toastr.error(msg);
          });
        } else {
          this.toastr.error(this.translationService.getValue('ERROR_FROM_SERVICE'));
        }
      },
    });
  }

  onLoginSubmit() {
    if (this.loginFormGroup.valid) {
      this.isLoading = true;
      const userObject = {
        ...this.loginFormGroup.value,
        latitude: this.lat,
        longitude: this.lng,
      };
      this.sub$.sink = this.securityService.login(userObject).subscribe({
        next: (c: UserAuth) => {
          this.isLoading = false;
          this.getAllAllowFileExtension();
          this.toastr.success(
            this.translationService.getValue('USER_LOGIN_SUCCESSFULLY')
          );
          if (this.securityService.hasClaim('view_dashboard')) {
            this.router.navigate(['/dashboard']);
          } else if (this.securityService.hasClaim('view_documents')) {
            this.router.navigate(['/documents/list-view']);
          } else {
            this.router.navigate(['/assign/list-view']);
          }
        },
        error: (err: any) => {
          this.isLoading = false;
          if (err?.error?.messages && Array.isArray(err.error.messages)) {
            err.error.messages.forEach((msg: string) => {
              this.toastr.error(msg);
            });
          } else {
            this.toastr.error(this.translationService.getValue('ERROR_FROM_SERVICE'));
          }
        },
      });
    }
  }

  createFormGroup(): void {
    this.loginFormGroup = this.fb.group({
      userName: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
    });
  }

  onRegistrationClick(): void {
    this.router.navigate(['/registration']);
  }

  setBackgroundImage(url: string): void {
    const authBg = document.querySelector('.auth-bg');
    if (authBg) {
      this.renderer.setStyle(authBg, 'background-image', `url(${url})`);
    }
  }

  getCompanyProfile(): void {
    this.securityService.companyProfile.subscribe((c) => {
      this.logoUrl = '/images/ouk-logo.png';
      this.setBackgroundImage('/images/ouk-banner.jpg');
    });
  }

  getAllAllowFileExtension() {
    this.commonService
      .getAllowFileExtensions()
      .subscribe();
  }
}

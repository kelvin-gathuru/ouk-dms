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
  logoUrl = '/images/logo.png';
  loginFormGroup: UntypedFormGroup;
  isLoading = false;
  lat: number;
  lng: number;

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
    this.setBackgroundImage('/images/banner.jpg');
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
          err.error.messages.forEach((msg: string) => {
            this.toastr.error(msg);
          });
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
      this.logoUrl = '/images/logo.png';
      this.setBackgroundImage('/images/banner.jpg');
    });
  }

  getAllAllowFileExtension() {
    this.commonService
      .getAllowFileExtensions()
      .subscribe();
  }
}

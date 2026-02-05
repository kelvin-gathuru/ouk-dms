import { Component, OnInit } from '@angular/core';
import {
  ReactiveFormsModule,
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router, RouterModule } from '@angular/router';
import { User } from '@core/domain-classes/user';
import { UserAuth } from '@core/domain-classes/user-auth';
import { SecurityService } from '@core/security/security.service';
import { ToastrService } from '@core/services/toastr-service';
import { BaseComponent } from '../../base.component';
import { ChangePasswordComponent } from '../change-password/change-password.component';
import { UserService } from '../user.service';
import { TranslateModule } from '@ngx-translate/core';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-my-profile',
  templateUrl: './my-profile.component.html',
  styleUrls: ['./my-profile.component.scss'],
  standalone: true,
  imports: [
    ReactiveFormsModule,
    RouterModule,
    TranslateModule,
    MatIconModule,
    MatCardModule,
    MatButtonModule
  ]
})
export class MyProfileComponent extends BaseComponent implements OnInit {
  userForm!: UntypedFormGroup;
  user!: UserAuth;
  constructor(
    private router: Router,
    private fb: UntypedFormBuilder,
    private userService: UserService,
    private toastrService: ToastrService,
    private dialog: MatDialog,
    private securityService: SecurityService

  ) {
    super();
  }

  ngOnInit(): void {
    this.createUserForm();
    const user = this.securityService.getUserDetail();
    if (user) {
      this.user = user;
      this.userForm.patchValue(this.user);
    }
  }

  createUserForm() {
    this.userForm = this.fb.group({
      id: [''],
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      email: [
        { value: '', disabled: true },
        '',
        [Validators.required, Validators.email],
      ],
      phoneNumber: ['', [Validators.required]],
    });
  }

  updateProfile() {
    if (this.userForm.valid) {
      const user = this.createBuildObject();
      this.sub$.sink = this.userService
        .updateUserProfile(user)
        .subscribe({
          next: (result: any) => {
            // Check if result is a User (has firstName, lastName, phoneNumber)
            if (result && typeof result === 'object' && 'firstName' in result && 'lastName' in result && 'phoneNumber' in result) {
              this.user.firstName = result.firstName ?? '';
              this.user.lastName = result.lastName ?? '';
              this.user.phoneNumber = result.phoneNumber ?? '';
              this.toastrService.success(
                this.translationService.getValue('PROFILE_UPDATED_SUCCESSFULLY')
              );
              this.securityService.setUserDetail(this.user);
              this.router.navigate(['/']);
            } else {
              // Handle error response
              this.toastrService.error(
                this.translationService.getValue('PROFILE_UPDATE_FAILED')
              );
            }
          },
          error: (err: unknown) => {
            this.toastrService.error(
              this.translationService.getValue('PROFILE_UPDATE_FAILED')
            );
          }
        });
    } else {
      this.toastrService.error(
        this.translationService.getValue('PLEASE_ENTER_PROPER_DATA')
      );
    }
  }

  createBuildObject(): User {
    const id = this.userForm.get('id')?.value ?? '';
    const firstName = this.userForm.get('firstName')?.value ?? '';
    const lastName = this.userForm.get('lastName')?.value ?? '';
    const email = this.userForm.get('email')?.value ?? '';
    const phoneNumber = this.userForm.get('phoneNumber')?.value ?? '';
    const user: User = {
      id,
      firstName,
      lastName,
      email,
      phoneNumber,
      userName: email,
    };
    return user;
  }

  changePassword(): void {
    this.dialog.open(ChangePasswordComponent, {
      width: '350px',
      data: Object.assign({}, this.user),
    });
  }
}

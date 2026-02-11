import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { Role } from '@core/domain-classes/role';
import { User } from '@core/domain-classes/user';
import { CommonService } from '@core/services/common.service';
import { ToastrService } from '@core/services/toastr-service';
import { BaseComponent } from '../../base.component';
import { UserService } from '../user.service';
import { UserAuth } from '@core/domain-classes/user-auth';
import { SecurityService } from '@core/security/security.service';
import { UserStore } from '../store/user.store';
import { MatIconModule } from '@angular/material/icon';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { HasClaimDirective } from '@shared/has-claim.directive';
import { TranslateModule } from '@ngx-translate/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-manage-user',
  templateUrl: './manage-user.component.html',
  styleUrls: ['./manage-user.component.scss'],
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatIconModule,
    PageHelpTextComponent,
    MatSelectModule,
    MatCheckboxModule,
    HasClaimDirective,
    RouterModule,
    TranslateModule,
    MatButtonModule,
    MatCardModule
  ]
})
export class ManageUserComponent extends BaseComponent implements OnInit {
  user: User;
  userForm: FormGroup;
  roleList: Role[];
  isEditMode = false;
  selectedRoles: Role[] = [];
  securityObject: UserAuth;
  UserStore = inject(UserStore);
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private activeRoute: ActivatedRoute,
    private userService: UserService,
    private toastrService: ToastrService,
    private commonService: CommonService,
    private securityService: SecurityService
  ) {
    super();
  }

  ngOnInit(): void {
    this.getAuthObj();
    this.createUserForm();
    this.sub$.sink = this.activeRoute.data.subscribe((data: any) => {
      if (data && data.user) {
        this.isEditMode = true;
        this.userForm.patchValue(data.user);
        this.user = data.user;
        const emailControl = this.userForm.get('email');
        if (emailControl) {
          emailControl.disable();
        }
      } else {
        const emailControl = this.userForm.get('email');
        if (emailControl) {
          emailControl.enable();
        }
      }
    });
    this.getRoles();
  }

  getAuthObj() {
    this.sub$.sink = this.securityService.SecurityObject.subscribe((c) => {
      if (c)
        this.securityObject = c;
    });
  }

  createUserForm() {
    this.userForm = this.fb.group({
      id: [''],
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.required]],
      isSuperAdmin: [false]
    });
  }



  private markFormGroupTouched(formGroup: FormGroup) {
    (<any>Object).values(formGroup.controls).forEach((control: any) => {
      control.markAsTouched();

      if (control.controls) {
        this.markFormGroupTouched(control);
      }
    });
  }

  saveUser() {
    if (this.userForm.valid) {
      const user = this.createBuildObject();
      if (this.isEditMode) {
        this.sub$.sink = this.userService.updateUser(user).subscribe(() => {
          this.toastrService.success(this.translationService.getValue('USER_UPDATED_SUCCESSFULLY'));
          this.router.navigate(['/users']);
        });
      } else {
        this.sub$.sink = this.userService.addUser(user).subscribe(() => {
          this.toastrService.success(this.translationService.getValue('USER_CREATED_SUCCESSFULLY'));
          this.router.navigate(['/users']);
        });
      }
      this.UserStore.getUsers();
    } else {
      this.markFormGroupTouched(this.userForm);
    }
  }

  createBuildObject(): User {
    const user: User = {
      id: this.userForm.get('id')?.value,
      firstName: this.userForm.get('firstName')?.value,
      lastName: this.userForm.get('lastName')?.value,
      email: this.userForm.get('email')?.value,
      phoneNumber: this.userForm.get('phoneNumber')?.value,
      isSuperAdmin: this.userForm.get('isSuperAdmin')?.value,
      userName: this.userForm.get('email')?.value,
      userRoles: this.getSelectedRoles()
    }
    return user;
  }

  getSelectedRoles() {
    return this.selectedRoles
      .filter((role) => !!role.id)
      .map((role) => {
        return {
          userId: this.userForm.get('id')?.value,
          roleId: role.id as string
        }
      });
  }

  getRoles() {
    this.sub$.sink = this.commonService.getRoles().subscribe((roles: Role[]) => {
      this.roleList = roles;
      if (this.isEditMode) {
        const selectedRoleIds = this.user.userRoles?.map(c => c.roleId) ?? [];
        this.selectedRoles = this.roleList?.filter(c => selectedRoleIds?.indexOf(c.id ?? '') > -1);
      }
    });
  }
}

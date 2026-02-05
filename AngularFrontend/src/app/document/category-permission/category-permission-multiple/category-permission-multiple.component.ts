import { Component, OnInit, Inject, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatChipsModule } from '@angular/material/chips';
import { BaseComponent } from '../../../base.component';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxChange, MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { Category } from '../../../core/domain-classes/category';
import { CategoryPermission } from '../../../core/domain-classes/category-permission';
import { Role } from '../../../core/domain-classes/role';
import { Subscription } from 'rxjs';
import { CategoryPermissionService } from '../../../core/services/category-permission.service';

import { ToastrService } from '@core/services/toastr-service';
import { CommonService } from '../../../core/services/common.service';
import { CategoryPermissionUserRole } from '../../../core/domain-classes/category-permission-user-role';
import { ServiceResponse } from '../../../core/domain-classes/service-response';
import { UserStore } from '../../../user/store/user.store';
import { TranslateModule } from '@ngx-translate/core';


@Component({
  selector: 'app-category-permission-multiple',
  imports: [
    MatDialogModule,
    MatTableModule,
    MatIconModule,
    MatButtonModule,
    MatChipsModule,
    MatSelectModule,
    MatCheckboxModule,
    MatDatepickerModule,
    ReactiveFormsModule,
    TranslateModule
  ],
  templateUrl: './category-permission-multiple.component.html',
  styleUrl: './category-permission-multiple.component.scss'
})
export class CategoryPermissionMultipleComponent extends BaseComponent
  implements OnInit {
  categoryPermissions: CategoryPermission[] = [];
  categories: Category[];
  userStore = inject(UserStore);
  roles: Role[] = [];
  permissionForm: UntypedFormGroup;
  subscriptions = new Subscription();
  smtpConfigured: boolean = false;
  minDate: Date = new Date();
  constructor(
    private categoryPermissionService: CategoryPermissionService,
    private toastrService: ToastrService,
    private commonService: CommonService,
    @Inject(MAT_DIALOG_DATA) public data: Category[],
    private dialogRef: MatDialogRef<CategoryPermissionMultipleComponent>,
    private fb: UntypedFormBuilder
  ) {
    super();
  }
  ngOnInit() {
    this.categories = this.data;
    this.getRoles();
    this.createFormGroup();

    this.sub$.sink = this.commonService.IsSmtpConfigured.subscribe((isConfigured) => {
      if (isConfigured == null) {
        this.checkEmailSMTPSettings();
        return;
      }
      this.smtpConfigured = isConfigured;
    })

  }
  createFormGroup() {
    this.permissionForm = this.fb.group({
      roles: [],
      users: [],
      isTimeBound: [false],
      startDate: [],
      endDate: [],
      isAllowDownload: [false],
      isAllowEmailNotification: [false],
    });
  }

  timeBoundChange(event: MatCheckboxChange) {
    if (event.checked) {
      this.permissionForm?.get('startDate')?.setValidators([Validators.required]);
      this.permissionForm?.get('endDate')?.setValidators([Validators.required]);
      this.permissionForm.updateValueAndValidity();
    } else {
      this.permissionForm?.get('startDate')?.clearValidators();
      this.permissionForm?.get('endDate')?.clearValidators();
      this.permissionForm.updateValueAndValidity();
    }
  }

  buildObject() {
    const permissionUserRole: CategoryPermissionUserRole = {
      roles: this.permissionForm?.get('roles')?.value,
      users: this.permissionForm.get('users')?.value,
      isTimeBound: this.permissionForm.get('isTimeBound')?.value,
      startDate: this.permissionForm.get('startDate')?.value,
      endDate: this.permissionForm.get('endDate')?.value,
      isAllowDownload: this.permissionForm.get('isAllowDownload')?.value,
      categories: this.categories.length > 0 ? this.categories.map((c) => c.id ?? '') : [],
      isAllowEmailNotification: this.permissionForm.get(
        'isAllowEmailNotification'
      )?.value,
    };
    return permissionUserRole;
  }

  saveCategoryUserPermission() {
    if (!this.permissionForm.valid) {
      this.permissionForm.markAllAsTouched();
      return;
    }
    const permissionUserRole = this.buildObject();
    if (!permissionUserRole.roles && !permissionUserRole.users) {
      this.toastrService.error(
        this.translationService.getValue('PLEASE_SELECT_EITHER_ROLES_OR_USERS')
      );
      return;
    }
    this.sub$.sink = this.categoryPermissionService
      .multipleCategorysToUsersAndRoles(permissionUserRole)
      .subscribe((c) => {
        this.toastrService.success(
          this.translationService.getValue(
            'CATEGORY_OR_FOLDER_PERMISSION_ASSIGN_TO_USERS_AND_ROLES'
          )
        );
        this.dialogRef.close();
      });
  }

  getRoles() {
    this.sub$.sink = this.commonService
      .getRoles()
      .subscribe((roles: Role[]) => (this.roles = roles));
  }

  closeDialog() {
    this.dialogRef.close();
  }

  checkEmailSMTPSettings() {
    this.sub$.sink = this.commonService.checkEmailSMTPSetting().subscribe({
      next: (response: ServiceResponse<boolean>) => {
        if (response && response?.data) {
        }
      },
      error: (error) => {
        this.toastrService.error(
          this.translationService.getValue('SMTP_SETTINGS_ERROR')
        );
      },
    });
  }
}

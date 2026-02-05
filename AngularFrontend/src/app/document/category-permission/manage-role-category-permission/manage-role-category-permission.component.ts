import { Component, Inject, OnInit } from '@angular/core';
import { ReactiveFormsModule, UntypedFormBuilder, UntypedFormControl, UntypedFormGroup, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxChange, MatCheckboxModule } from '@angular/material/checkbox';
import { MatChipsModule } from '@angular/material/chips';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';
import { CategoryRolePermission } from '@core/domain-classes/category-role-permission';
import { Role } from '@core/domain-classes/role';
import { ServiceResponse } from '@core/domain-classes/service-response';
import { CategoryPermissionService } from '@core/services/category-permission.service';
import { CommonService } from '@core/services/common.service';
import { TranslateModule } from '@ngx-translate/core';
import { ToastrService } from '@core/services/toastr-service';
import { Subscription } from 'rxjs';
import { BaseComponent } from '../../../base.component';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-manage-role-category-permission',
  imports: [
    MatDialogModule,
    MatTableModule,
    MatButtonModule,
    MatChipsModule,
    MatSelectModule,
    MatCheckboxModule,
    MatDatepickerModule,
    ReactiveFormsModule,
    TranslateModule,
    MatIconModule,
    MatCardModule
  ],
  templateUrl: './manage-role-category-permission.component.html',
  styleUrl: './manage-role-category-permission.component.scss'
})
export class ManageRoleCategoryPermissionComponent extends BaseComponent implements OnInit {
  selectedRoles: Role[] = [];
  minDate: Date;
  permissionForm: UntypedFormGroup;
  subscriptions = new Subscription();
  smtpConfigured: boolean;
  constructor(
    private categoryPermissionService: CategoryPermissionService,
    private toastrService: ToastrService,
    @Inject(MAT_DIALOG_DATA) public data: { roles: Role[], categoryId: string },
    private dialogRef: MatDialogRef<ManageRoleCategoryPermissionComponent>,
    private fb: UntypedFormBuilder,
    private commonService: CommonService
  ) {
    super();
    this.minDate = new Date();
  }

  ngOnInit(): void {
    this.createUserPermissionForm();
    this.subscriptions.add(
      this.commonService.IsSmtpConfigured.subscribe((isConfigured) => {
        if (isConfigured == null) {
          this.checkEmailSMTPSettings();
          return;
        }
        this.smtpConfigured = isConfigured;
      })
    );
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

  createUserPermissionForm() {
    this.permissionForm = this.fb.group({
      isTimeBound: new UntypedFormControl(false),
      startDate: [''],
      endDate: [''],
      isAllowDownload: new UntypedFormControl(false),
      isAllowEmailNotification: new UntypedFormControl(false)
    });
  }

  timeBoundChange(event: MatCheckboxChange) {
    if (event.checked) {
      this.permissionForm.get('startDate')?.setValidators([Validators.required]);
      this.permissionForm.get('endDate')?.setValidators([Validators.required])
    } else {
      this.permissionForm.get('startDate')?.clearValidators();
      this.permissionForm.get('startDate')?.updateValueAndValidity();
      this.permissionForm.get('endDate')?.clearValidators();
      this.permissionForm.get('endDate')?.updateValueAndValidity();
    }
  }

  saveCategoryRolePermission() {
    if (!this.permissionForm.valid) {
      this.permissionForm.markAllAsTouched();
      return;
    }
    if (this.selectedRoles.length == 0) {
      this.toastrService.error(this.translationService.getValue('PLEASE_SELECT_ATLEAST_ONE_ROLE'));
      return
    }

    let categoryRolePermission: CategoryRolePermission[] = this.selectedRoles.map(role => {
      return Object.assign({}, {
        id: '',
        categoryId: this.data.categoryId,
        roleId: role.id,
      }, this.permissionForm.value)
    });

    this.sub$.sink = this.categoryPermissionService.addCategoryRolePermission(categoryRolePermission).subscribe(() => {
      this.toastrService.success(this.translationService.getValue('PERMISSION_ADDED_SUCCESSFULLY'));
      this.dialogRef.close(true);
    });
  }

  onNoClick() {
    this.dialogRef.close();
  }
}

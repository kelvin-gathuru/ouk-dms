import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { ReactiveFormsModule, UntypedFormBuilder, UntypedFormControl, UntypedFormGroup, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxChange, MatCheckboxModule } from '@angular/material/checkbox';
import { MatChipsModule } from '@angular/material/chips';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';
import { ServiceResponse } from '@core/domain-classes/service-response';
import { CategoryPermissionService } from '@core/services/category-permission.service';
import { CommonService } from '@core/services/common.service';
import { ToastrService } from '@core/services/toastr-service';
import { BaseComponent } from '../../../base.component';
import { User } from '@core/domain-classes/user';
import { CategoryUserPermission } from '@core/domain-classes/category-user-permission';
import { TranslateModule } from '@ngx-translate/core';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';


@Component({
  selector: 'app-manage-user-category-permission',
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
    MatCardModule,
    MatIconModule,
    MatButtonModule
  ],
  templateUrl: './manage-user-category-permission.component.html',
  styleUrl: './manage-user-category-permission.component.scss'
})
export class ManageUserCategoryPermissionComponent extends BaseComponent implements OnInit {
  selectedUsers: User[] = [];
  minDate: Date;
  smtpConfigured: boolean;
  permissionForm: UntypedFormGroup;
  filteredUsers: User[] = [];
  constructor(
    private categoryPermissionService: CategoryPermissionService,
    private toastrService: ToastrService,
    @Inject(MAT_DIALOG_DATA) public data: { users: User[], categoryId: string },
    private dialogRef: MatDialogRef<ManageUserCategoryPermissionComponent>,
    private fb: UntypedFormBuilder,
    private commonService: CommonService
  ) {
    super();
    this.minDate = new Date();
  }

  ngOnInit(): void {
    this.createUserPermissionForm();
    this.filteredUsers = [...this.data.users];
    this.sub$.sink = this.commonService.IsSmtpConfigured.subscribe((isConfigured) => {
      if (isConfigured == null) {
        this.checkEmailSMTPSettings();
        return;
      }
      this.smtpConfigured = isConfigured;
    });
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

  filterUsers(event: Event) {
    const filterValue = (event.target as HTMLInputElement)?.value.trim().toLowerCase();

    if (!filterValue) {
      this.filteredUsers = [...this.data.users];
      return;
    }

    this.filteredUsers = this.data.users.filter(user =>
      (user.firstName?.toLowerCase().includes(filterValue)) ||
      (user.lastName?.toLowerCase().includes(filterValue)) ||
      (user.email?.toLowerCase().includes(filterValue)) ||
      (user.userName?.toLowerCase().includes(filterValue))
    );

    this.filteredUsers = Array.from(new Set([...this.selectedUsers, ...this.filteredUsers]));
  }

  compareUsers = (u1: User, u2: User) => u1 && u2 && u1.id === u2.id;


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

  saveCategoryUserPermission() {
    if (!this.permissionForm.valid) {
      this.permissionForm.markAllAsTouched();
      return;
    }
    if (this.selectedUsers.length == 0) {
      this.toastrService.error(this.translationService.getValue('PLEASE_SELECT_ATLEAST_ONE_USER'));
      return
    }
    let categoryUserPermission: CategoryUserPermission[] = this.selectedUsers.map(user => {
      return Object.assign({}, {
        id: '',
        categoryId: this.data.categoryId,
        userId: user.id
      }, this.permissionForm.value)
    });

    this.sub$.sink = this.categoryPermissionService.addCategoryUserPermission(categoryUserPermission)
      .subscribe(() => {
        this.toastrService.success(this.translationService.getValue('PERMISSION_ADDED_SUCCESSFULLY'));
        this.dialogRef.close(true);
      });
  }

  onNoClick() {
    this.dialogRef.close();
  }
}

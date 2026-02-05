import { Component, Inject, OnInit } from '@angular/core';
import { ReactiveFormsModule, UntypedFormBuilder, UntypedFormControl, UntypedFormGroup, Validators } from '@angular/forms';
import { MatCheckboxChange, MatCheckboxModule } from '@angular/material/checkbox';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { DocumentUserPermission } from '@core/domain-classes/document-user-permission';
import { User } from '@core/domain-classes/user';
import { ToastrService } from '@core/services/toastr-service';
import { BaseComponent } from '../../../base.component';
import { DocumentPermissionService } from '../document-permission.service';
import { Subscription } from 'rxjs';
import { CommonService } from '@core/services/common.service';
import { ServiceResponse } from '@core/domain-classes/service-response';
import { MatSelectModule } from '@angular/material/select';
import { TranslateModule } from '@ngx-translate/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-manage-user-permission',
  templateUrl: './manage-user-permission.component.html',
  styleUrls: ['./manage-user-permission.component.scss'],
  standalone: true,
  imports: [
    MatCheckboxModule,
    MatDialogModule,
    ReactiveFormsModule,
    MatSelectModule,
    TranslateModule,
    MatDatepickerModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule
  ]
})
export class ManageUserPermissionComponent extends BaseComponent implements OnInit {
  selectedUsers: User[] = [];
  minDate: Date;
  subscriptions = new Subscription();
  smtpConfigured: boolean;
  permissionForm: UntypedFormGroup;
  filteredUsers: User[] = [];
  searchTerm: string = '';
  users: User[] = [];
  constructor(
    private documentPermissionService: DocumentPermissionService,
    private toastrService: ToastrService,
    @Inject(MAT_DIALOG_DATA) public data: { users: User[], documentId: string },
    private dialogRef: MatDialogRef<ManageUserPermissionComponent>,
    private fb: UntypedFormBuilder,
    private commonService: CommonService
  ) {
    super();
    this.minDate = new Date();
  }

  ngOnInit(): void {
    this.filteredUsers = [...this.data.users];
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
  filteredOptions(): User[] {
    return this.data.users.filter(option => {
      option.firstName?.toLowerCase().includes(this.searchTerm.toLowerCase())
    }
    );
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

  saveDocumentUserPermission() {
    if (!this.permissionForm.valid) {
      this.permissionForm.markAllAsTouched();
      return;
    }
    if (this.selectedUsers.length == 0) {
      this.toastrService.error(this.translationService.getValue('PLEASE_SELECT_ATLEAST_ONE_USER'));
      return
    }
    let documentUserPermission: DocumentUserPermission[] = this.selectedUsers.map(user => {
      return Object.assign({}, {
        id: '',
        documentId: this.data.documentId,
        userId: user.id
      }, this.permissionForm.value)
    });

    this.sub$.sink = this.documentPermissionService.addDocumentUserPermission(documentUserPermission).subscribe(() => {
      this.toastrService.success(this.translationService.getValue('PERMISSION_ADDED_SUCCESSFULLY'));
      this.dialogRef.close(true);
    });
  }

  onNoClick() {
    this.dialogRef.close();
  }
}

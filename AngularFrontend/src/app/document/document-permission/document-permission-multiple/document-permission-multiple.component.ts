import { Component, inject, Inject, OnInit } from '@angular/core';
import {
  ReactiveFormsModule,
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { MatCheckboxChange, MatCheckboxModule } from '@angular/material/checkbox';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { DocumentInfo } from '@core/domain-classes/document-info';
import { DocumentPermission } from '@core/domain-classes/document-permission';
import { PermissionUserRole } from '@core/domain-classes/permission-user-role';
import { Role } from '@core/domain-classes/role';
import { User } from '@core/domain-classes/user';
import { CommonService } from '@core/services/common.service';
import { ToastrService } from '@core/services/toastr-service';
import { BaseComponent } from '../../../base.component';
import { DocumentPermissionService } from '../document-permission.service';
import { ServiceResponse } from '@core/domain-classes/service-response';
import { Subscription } from 'rxjs';
import { UserStore } from '../../../user/store/user.store';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { TranslateModule } from '@ngx-translate/core';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-document-permission-multiple',
  templateUrl: './document-permission-multiple.component.html',
  styleUrls: ['./document-permission-multiple.component.scss'],
  standalone: true,
  imports: [
    MatTableModule,
    MatPaginatorModule,
    MatDialogModule,
    PageHelpTextComponent,
    MatIconModule,
    MatChipsModule,
    TranslateModule,
    MatSelectModule,
    MatCheckboxModule,
    MatDatepickerModule,
    ReactiveFormsModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule
  ],
})
export class DocumentPermissionMultipleComponent
  extends BaseComponent
  implements OnInit {
  documentPermissions: DocumentPermission[] = [];
  documents: DocumentInfo[];
  users: User[] = [];
  filteredUsers: User[] = [];
  selectedUsers: User[] = [];
  roles: Role[] = [];
  permissionForm: UntypedFormGroup;
  subscriptions = new Subscription();
  smtpConfigured: boolean = false;
  minDate: Date = new Date();
  userStore = inject(UserStore);
  constructor(
    private documentPermissionService: DocumentPermissionService,
    private toastrService: ToastrService,
    private commonService: CommonService,
    @Inject(MAT_DIALOG_DATA) public data: DocumentInfo[],
    private dialogRef: MatDialogRef<DocumentPermissionMultipleComponent>,
    private fb: UntypedFormBuilder
  ) {
    super();
  }
  ngOnInit() {
    this.documents = this.data;
    this.filteredUsers = [...this.users];
    this.getRoles();
    this.createFormGroup();
    this.subscriptions.add(
      this.commonService.IsSmtpConfigured.subscribe((isConfigured) => {
        if (isConfigured == null) {
          this.checkEmailSMTPSettings();
          return;
        }
        this.smtpConfigured = isConfigured;
      })
    );
    this.filteredUsers = [...this.userStore.users()];
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

  filterUsers(event: Event) {
    const filterValue = (event.target as HTMLInputElement)?.value.trim().toLowerCase();

    if (!filterValue) {
      this.filteredUsers = [...this.userStore.users()];
      return;
    }

    this.filteredUsers = this.users.filter(user =>
      (user.firstName?.toLowerCase().includes(filterValue)) ||
      (user.lastName?.toLowerCase().includes(filterValue)) ||
      (user.email?.toLowerCase().includes(filterValue)) ||
      (user.userName?.toLowerCase().includes(filterValue))
    );

    this.filteredUsers = Array.from(new Set([...this.selectedUsers, ...this.filteredUsers]));
  }

  compareUsers = (u1: User, u2: User) => u1 && u2 && u1.id === u2.id;

  timeBoundChange(event: MatCheckboxChange) {
    if (event.checked) {
      this.permissionForm.get('startDate')?.setValidators([Validators.required]);
      this.permissionForm.get('endDate')?.setValidators([Validators.required]);
      this.permissionForm.get('endDate')?.updateValueAndValidity();
      this.permissionForm.get('startDate')?.updateValueAndValidity();
    } else {
      this.permissionForm.get('startDate')?.clearValidators();
      this.permissionForm.get('endDate')?.clearValidators();
      this.permissionForm.get('endDate')?.updateValueAndValidity();
      this.permissionForm.get('startDate')?.updateValueAndValidity();
    }
  }

  buildObject() {
    const userIds = this.permissionForm.get('users')?.value && this.permissionForm.get('users')?.value.length > 0 ? this.permissionForm.get('users')?.value.map((user: User) => user.id) : [];
    const permissionUserRole: PermissionUserRole = {
      roles: this.permissionForm.get('roles')?.value ?? [],
      users: userIds ?? [],
      isTimeBound: this.permissionForm.get('isTimeBound')?.value,
      startDate: this.permissionForm.get('startDate')?.value,
      endDate: this.permissionForm.get('endDate')?.value,
      isAllowDownload: this.permissionForm.get('isAllowDownload')?.value,
      documents: this.documents.map((c) => c.id).filter((id): id is string => typeof id === 'string'),
      isAllowEmailNotification: this.permissionForm.get(
        'isAllowEmailNotification'
      )?.value,
    };
    return permissionUserRole;
  }

  saveDocumentUserPermission() {
    if (!this.permissionForm.valid) {
      this.permissionForm.markAllAsTouched();
      return;
    }
    const permissionUserRole = this.buildObject();
    if (permissionUserRole.roles.length === 0 && permissionUserRole.users.length === 0) {
      this.toastrService.error(
        this.translationService.getValue('PLEASE_SELECT_EITHER_ROLES_OR_USERS')
      );
      return;
    }
    this.sub$.sink = this.documentPermissionService
      .multipleDocumentsToUsersAndRoles(permissionUserRole)
      .subscribe((c) => {
        this.toastrService.success(
          this.translationService.getValue(
            'DOCUMENTS_PERMISSION_ASSIGN_TO_USERS_AND_ROLES'
          )
        );
        this.dialogRef.close('loaded');
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

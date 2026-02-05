import { Component, Inject, OnInit } from '@angular/core';
import { ReactiveFormsModule, UntypedFormBuilder, UntypedFormControl, UntypedFormGroup, Validators } from '@angular/forms';
import { MatCheckboxChange, MatCheckboxModule } from '@angular/material/checkbox';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { DocumentRolePermission } from '@core/domain-classes/document-role-permission';
import { Role } from '@core/domain-classes/role';
import { ToastrService } from '@core/services/toastr-service';
import { BaseComponent } from '../../../base.component';
import { DocumentPermissionService } from '../document-permission.service';
import { Subscription } from 'rxjs';
import { CommonService } from '../../../core/services/common.service';
import { ServiceResponse } from '@core/domain-classes/service-response';
import { MatSelectModule } from '@angular/material/select';
import { TranslateModule } from '@ngx-translate/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-manage-role-permission',
  templateUrl: './manage-role-permission.component.html',
  styleUrls: ['./manage-role-permission.component.scss'],
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
export class ManageRolePermissionComponent extends BaseComponent implements OnInit {
  selectedRoles: Role[] = [];
  minDate: Date;
  permissionForm: UntypedFormGroup;
  subscriptions = new Subscription();
  smtpConfigured: boolean;
  constructor(
    private documentPermissionService: DocumentPermissionService,
    private toastrService: ToastrService,
    @Inject(MAT_DIALOG_DATA) public data: { roles: Role[], documentId: string },
    private dialogRef: MatDialogRef<ManageRolePermissionComponent>,
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

  saveDocumentRolePermission() {
    if (!this.permissionForm.valid) {
      this.permissionForm.markAllAsTouched();
      return;
    }
    if (this.selectedRoles.length == 0) {
      this.toastrService.error(this.translationService.getValue('PLEASE_SELECT_ATLEAST_ONE_ROLE'));
      return
    }

    let documentRolePermission: DocumentRolePermission[] = this.selectedRoles.map(role => {
      return Object.assign({}, {
        id: '',
        documentId: this.data.documentId,
        roleId: role.id,
      }, this.permissionForm.value)
    });

    this.sub$.sink = this.documentPermissionService.addDocumentRolePermission(documentRolePermission).subscribe(() => {
      this.toastrService.success(this.translationService.getValue('PERMISSION_ADDED_SUCCESSFULLY'));
      this.dialogRef.close(true);
    });
  }

  onNoClick() {
    this.dialogRef.close();
  }
}

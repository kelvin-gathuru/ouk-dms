import { Component, inject, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { Router, RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { StorageType } from '../storage-type-enum';

import { ToastrService } from '@core/services/toastr-service';
import { StorageSettingService } from '../storage-setting.service';
import {
  AWSS3storage,
  CloudflareStorage,
  StorageSetting,
} from '../storage-setting';
import { StorageTypePipe } from '../storage-type.pipe';
import { MatSelectChange, MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { TranslationService } from '@core/services/translation.service';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-manage-storage-setting',
  imports: [
    RouterModule,
    TranslateModule,
    ReactiveFormsModule,
    StorageTypePipe,
    MatSelectModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
    MatCheckboxModule,
    PageHelpTextComponent,
    MatCardModule
  ],
  templateUrl: './manage-storage-setting.component.html',
  styleUrls: ['./manage-storage-setting.component.scss']
})
export class ManageStorageSettingComponent implements OnInit {
  storageSettingForm: FormGroup;
  storageType = StorageType;
  storageTypes = Object.keys(StorageType)
    .filter((key) => isNaN(Number(key)))
    .map((key) => ({
      key,
      value: StorageType[key as keyof typeof StorageType]
    }));

  isLoading = false;

  public dialogRef = inject(MatDialogRef<ManageStorageSettingComponent>);
  private storageSettingService = inject(StorageSettingService);
  private toastrService = inject(ToastrService);
  private router = inject(Router);
  private translationService = inject(TranslationService);

  constructor(private fb: FormBuilder) {
    this.storageSettingForm = this.fb.group({
      storageType: [StorageType.LOCAL.toString(), Validators.required],
      name: ['', Validators.required],
      isDefault: [false],
      enableEncryption: [false],
    });
  }

  ngOnInit(): void {
    this.createStorageSettingForm();
  }

  createStorageSettingForm() {
    this.storageSettingForm = this.fb.group({
      storageType: [this.storageType.LOCAL.toString(), Validators.required],
      name: ['', Validators.required],
      isDefault: [false],
      enableEncryption: [false],
    });
  }

  checkControlExists(controlName: string): boolean {
    return !!this.storageSettingForm.get(controlName);
  }

  addCloudflareStorageControls() {
    if (this.checkControlExists('accessKey')) {
      return;
    }
    this.storageSettingForm.addControl(
      'accessKey',
      this.fb.control('', [Validators.required])
    );
    this.storageSettingForm.addControl(
      'accountId',
      this.fb.control('', [Validators.required])
    );
    this.storageSettingForm.addControl(
      'secretKey',
      this.fb.control('', [Validators.required])
    );
    this.storageSettingForm.addControl(
      'bucketName',
      this.fb.control('', [Validators.required])
    );
    this.storageSettingForm.addControl(
      'region',
      this.fb.control('', [Validators.required])
    );
  }

  addAwsCloudStorageControls() {
    if (this.checkControlExists('accessKey')) {
      return;
    }
    this.storageSettingForm.addControl(
      'accessKey',
      this.fb.control('', [Validators.required])
    );
    this.storageSettingForm.addControl(
      'secretKey',
      this.fb.control('', [Validators.required])
    );
    this.storageSettingForm.addControl(
      'bucketName',
      this.fb.control('', [Validators.required])
    );
    this.storageSettingForm.addControl(
      'region',
      this.fb.control('', [Validators.required])
    );
  }
  removeAwsCloudStorageControls() {
    if (this.checkControlExists('accessKey')) {
      this.storageSettingForm.removeControl('accessKey');
      this.storageSettingForm.removeControl('secretKey');
      this.storageSettingForm.removeControl('bucketName');
      this.storageSettingForm.removeControl('region');
    }
  }

  removeCloudflareStorageControls() {
    if (this.checkControlExists('accessKey')) {
      this.storageSettingForm.removeControl('accessKey');
      this.storageSettingForm.removeControl('secretKey');
      this.storageSettingForm.removeControl('bucketName');
      this.storageSettingForm.removeControl('region');
      this.storageSettingForm.removeControl('accountId');
    }
  }

  onStorageTypeChange(event: MatSelectChange) {
    const selectedType = event.value;
    if (selectedType == StorageType.AWS.toString()) {
      this.removeAwsCloudStorageControls();
      this.removeCloudflareStorageControls();
      this.addAwsCloudStorageControls();
    } else if (selectedType == StorageType.CLOUDFLARE.toString()) {
      this.removeAwsCloudStorageControls();
      this.removeCloudflareStorageControls();
      this.addCloudflareStorageControls();
    } else if (selectedType == StorageType.LOCAL.toString()) {
      this.removeAwsCloudStorageControls();
      this.removeCloudflareStorageControls();
    }
  }

  buildObject(): StorageSetting<any> {
    if (
      this.storageSettingForm.get('storageType')?.value.toString() ===
      StorageType.AWS.toString()
    ) {
      const awsData: AWSS3storage = {
        accessKey: this.storageSettingForm.get('accessKey')?.value,
        secretKey: this.storageSettingForm.get('secretKey')?.value,
        bucketName: this.storageSettingForm.get('bucketName')?.value,
        region: this.storageSettingForm.get('region')?.value,
      };
      const storageSetting: StorageSetting<string> = {
        storageType: this.storageSettingForm.get('storageType')?.value,
        name: this.storageSettingForm.get('name')?.value,
        isDefault: this.storageSettingForm.get('isDefault')?.value,
        enableEncryption:
          this.storageSettingForm.get('enableEncryption')?.value,
        jsonValue: JSON.stringify(awsData),
      };
      return storageSetting;
    } else if (
      this.storageSettingForm.get('storageType')?.value.toString() ===
      StorageType.CLOUDFLARE.toString()
    ) {
      const cloudData: CloudflareStorage = {
        accessKey: this.storageSettingForm.get('accessKey')?.value,
        secretKey: this.storageSettingForm.get('secretKey')?.value,
        bucketName: this.storageSettingForm.get('bucketName')?.value,
        region: this.storageSettingForm.get('region')?.value,
        accountID: this.storageSettingForm.get('accountId')?.value,
      };
      const storageSetting: StorageSetting<string> = {
        storageType: this.storageSettingForm.get('storageType')?.value,
        name: this.storageSettingForm.get('name')?.value,
        isDefault: this.storageSettingForm.get('isDefault')?.value,
        enableEncryption:
          this.storageSettingForm.get('enableEncryption')?.value,
        jsonValue: JSON.stringify(cloudData),
      };
      return storageSetting;
    } else if (
      this.storageSettingForm.get('storageType')?.value.toString() ===
      StorageType.LOCAL.toString()
    ) {
      const storageSetting: StorageSetting<string> = {
        storageType: this.storageSettingForm.get('storageType')?.value,
        name: this.storageSettingForm.get('name')?.value,
        isDefault: this.storageSettingForm.get('isDefault')?.value,
        enableEncryption:
          this.storageSettingForm.get('enableEncryption')?.value,
        jsonValue: '',
      };
      return storageSetting;
    } else {
      return {} as StorageSetting<any>;
    }
  }

  saveSettings() {
    if (!this.storageSettingForm.valid) {
      this.storageSettingForm.markAllAsTouched();
      return;
    }
    this.storageSettingService.addStorageSetting(this.buildObject()).subscribe({
      next: () => {
        this.toastrService.success(
          this.translationService.getValue(
            'STORAGE_SETTING_UPDATED_SUCCESSFULLY'
          )
        );
        this.dialogRef.close();
        this.router.navigate(['/storage-settings']);
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
      },
    });
  }

  closeDialog() {
    this.dialogRef.close();
  }
}
